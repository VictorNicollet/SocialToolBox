using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// The projection engine registers projectors and runs them.
    /// </summary>
    public class ProjectionEngine
    {
        /// <summary>
        /// The database driver to which this projection engine is bound.
        /// </summary>
        public readonly IDatabaseDriver Driver;

        /// <summary>
        /// A registered projector is a function which, when called, runs one
        /// projector and commits it.
        /// </summary>
        private readonly List<Func<Task>> _registeredProjectors;

        public ProjectionEngine(IDatabaseDriver driver)
        {
            Driver = driver;
            _registeredProjectors = new List<Func<Task>>();
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
            Task.WaitAll(_registeredProjectors.Select(f => f()).ToArray());
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
            var iterator = FromEventStream.EachOfType<TEv>(vectorClock, proj.Streams);

            var sinceLastCommit = 0;            

            while (true)
            {
                var ev = await iterator.NextAsync();
                
                if (ev == null && sinceLastCommit > 0 || proj.CommitRecommended)
                {
                    await Driver.ClockRegistry.SaveProjection(proj.Name, iterator.VectorClock);
                    await proj.Commit();
                    sinceLastCommit = 0;                    
                }

                if (ev == null) break;

                proj.ProcessEvent(ev);
                sinceLastCommit++;
            }
        }
    }
}
