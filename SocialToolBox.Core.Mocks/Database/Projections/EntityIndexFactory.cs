using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Mocks.Database.Projections
{
    /// <summary>
    /// Mock in-memory implementation of an entity index factory.
    /// </summary>
    public class EntityIndexFactory : IEntityIndexFactory
    {
        /// <summary>
        /// The actual inner database driver, with its known type.
        /// </summary>
        public readonly DatabaseDriver InnerDriver;

        public EntityIndexFactory(DatabaseDriver driver)
        {
            InnerDriver = driver;
        }

        public IDatabaseDriver Driver { get { return InnerDriver; } }

        public IEntityIndex<TSet, TSort, TEn> Create<TEv, TEn, TSet, TSort>(string name, IEntityIndexProjection<TEv, TEn, TSet, TSort> proj, IEventStream[] streams) where TEv : class where TEn : class where TSet : class where TSort : class
        {
            var store = new EntityIndex<TSet, TSort, TEn>(proj.Sets);
            var projector = new Projector<TEv, TSet, TSort, TEn>(name, store, proj);
            InnerDriver.Projections.Register(projector, streams);
            return store;
        }

        /// <summary>
        /// The projector is registered with the database driver to keep entity stores updated.
        /// </summary>
        private class Projector<TEv, TSet, TSort, TEn> : IProjector<TEv>
            where TEv : class
            where TSet : class
            where TSort : class
            where TEn : class
        {
            /// <summary>
            /// The entity store updated by this projector.
            /// </summary>
            private readonly EntityIndex<TSet, TSort, TEn> _entityIndex;

            /// <summary>
            /// The projection used by this projector.
            /// </summary>
            private readonly IEntityIndexProjection<TEv, TEn, TSet, TSort> _projection;

            /// <summary>
            /// Events which are associated with an ID are stored in this list, to 
            /// be processed when a commit happens.
            /// </summary>
            private readonly List<IPair<Id, TEv>> _pendingEvents;

            /// <summary>
            /// The name of this projector, used to persist the vector clock.
            /// </summary>
            private readonly string _name;

            public Projector(string name, EntityIndex<TSet,TSort,TEn> entityIndex, 
                IEntityIndexProjection<TEv, TEn, TSet, TSort> projection)
            {
                _name = name;
                _entityIndex = entityIndex;
                _projection = projection;
                _pendingEvents = new List<IPair<Id, TEv>>();
            }

            public string Name { get { return _name; } }

            public bool CommitRecommended { get { return _pendingEvents.Count > 100; } }

            public void ProcessEvent(TEv ev)
            {
                var id = _projection.EventIdentifier(ev);
                if (id == null) return;
                _pendingEvents.Add(Pair.Make((Id)id, ev));
            }

            public async Task Commit()
            {
                await Task.Yield();
                foreach (var kv in _pendingEvents)
                {
                    var oldValue = await _entityIndex.Get(kv.First);
                    var newValue = _projection.Update(kv.First, kv.Second, oldValue);
                    _entityIndex.Update(kv.First,newValue);
                }
            }
        }

    }
}
