using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Mocks.Database.Projections
{
    public class EntityStoreFactory : IEntityStoreFactory
    {
        /// <summary>
        /// The actual inner database driver, with its known type.
        /// </summary>
        public readonly DatabaseDriver InnerDriver;

        public EntityStoreFactory(DatabaseDriver driver)
        {
            InnerDriver = driver;
        }

        public IDatabaseDriver Driver { get { return InnerDriver; } }

        public IEntityStore<TEn> Create<TEv, TEn>(string name, IEntityStoreProjection<TEv, TEn> proj, IEventStream[] streams) 
            where TEv : class where TEn : class
        {
            var store = new EntityStore<TEn>();
            var projector = new Projector<TEv, TEn>(name, store, proj);
            InnerDriver.Projections.Register(projector, streams);
            return store;
        }

        /// <summary>
        /// The projector is registered with the database driver to keep entity stores updated.
        /// </summary>
        private class Projector<TEv, TEn> : IProjector<TEv> where TEv : class where TEn : class
        {
            /// <summary>
            /// The entity store updated by this projector.
            /// </summary>
            private readonly EntityStore<TEn> _entityStore;

            /// <summary>
            /// The projection used by this projector.
            /// </summary>
            private readonly IEntityStoreProjection<TEv, TEn> _projection;

            /// <summary>
            /// Events which are associated with an ID are stored in this list, to 
            /// be processed when a commit happens.
            /// </summary>
            private readonly List<KeyValuePair<Id,TEv>> _pendingEvents;

            /// <summary>
            /// The name of this projector, used to persist the vector clock.
            /// </summary>
            private readonly string _name;

            public Projector(string name, EntityStore<TEn> entityStore, IEntityStoreProjection<TEv, TEn> projection)
            {
                _name = name;
                _entityStore = entityStore;
                _projection = projection;
                _pendingEvents = new List<KeyValuePair<Id, TEv>>();
            }

            public string Name { get { return _name; } }

            public bool CommitRecommended { get { return _pendingEvents.Count > 100; } }

            public void ProcessEvent(TEv ev)
            {
                var id = _projection.EventIdentifier(ev);
                if (id == null) return;
                _pendingEvents.Add(new KeyValuePair<Id, TEv>((Id)id, ev));
            }

            public async Task Commit()
            {
                await Task.Yield();
                foreach (var kv in _pendingEvents)
                {
                    var oldValue = await _entityStore.Get(kv.Key);
                    var newValue = _projection.Update(kv.Key, kv.Value, oldValue);
                    _entityStore.Entities.Remove(kv.Key);
                    if (newValue != null) _entityStore.Entities.Add(kv.Key, newValue);
                }
            }
        }
    }
}
