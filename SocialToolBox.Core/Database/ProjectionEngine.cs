using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// The projection engine registers projectors and runs them.
    /// </summary>
    public class ProjectionEngine
    {
        /// <summary>
        /// The maximum transaction load before a commit is forced.
        /// </summary>
        public int MaxLoad = 100;

        /// <summary>
        /// The database driver to which this projection engine is bound.
        /// </summary>
        public readonly IDatabaseDriver Driver;

        /// <summary>
        /// A registered projector is a function which, when called, runs one
        /// projector and commits it.
        /// </summary>
        private readonly List<Func<Task>> _registeredProjectors;

        /// <summary>
        /// The transaction used for projection.
        /// </summary>
        private IProjectTransaction _transaction;

        /// <summary>
        /// Responsible for running <see cref="Run"/> in a loop.
        /// </summary>
        private readonly ProjectionEngineThread _thread;

        public ProjectionEngine(IDatabaseDriver driver)
        {
            Driver = driver;
            _registeredProjectors = new List<Func<Task>>();
            _thread = new ProjectionEngineThread(this);
        }

        /// <summary>
        /// Runs all projectors until they each run out of work. Does not
        /// look back to see if more work appeared during processing.
        /// </summary>
        /// <remarks>
        /// Thrown exceptions break out of the function. 
        /// </remarks>
        public void Run()
        {
            if (_transaction == null) _transaction = Driver.StartProjectorTransaction();
            Task.WaitAll(_registeredProjectors.Select(f => f()).ToArray());
            _transaction.Commit();
        }

        /// <summary>
        /// Spawns a thread that calls 
        /// </summary>
        public void StartBackgroundThread()
        {
            _transaction = Driver.StartProjectorTransaction();
            _thread.Start();
        }

        /// <summary>
        /// Registers a projector with the engine. The projector reads from the specified
        /// streams using a vector clock associated with the specified name.
        /// </summary>
        public void Register<TEv>(IProjector<TEv> proj)
            where TEv : class
        {
            _registeredProjectors.Add(() => Process(proj));
        }

        /// <summary>
        /// Process all events through a projector, asynchronously.
        /// </summary>
        private async Task Process<TEv>(IProjector<TEv> proj)
            where TEv : class
        {
            var vectorClock = await Driver.ClockRegistry.LoadProjection(proj.Name);
            var iterator = FromEventStream.EachOfType<TEv>(vectorClock, _transaction, proj.Streams);

            while (true)
            {
                var ev = await iterator.NextAsync();
                
                if (ev == null || _transaction.Load >= MaxLoad)
                {
                    await Driver.ClockRegistry.SaveProjection(proj.Name, iterator.VectorClock);
                    await _transaction.Commit();          
                }

                if (ev == null) break;

                await proj.ProcessEvent(ev, _transaction);
            }
        }
    }
}
