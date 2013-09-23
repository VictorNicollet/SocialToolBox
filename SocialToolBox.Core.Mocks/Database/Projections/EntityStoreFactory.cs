using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
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
            var projector = new Projector<TEv, TEn>(name, store, proj, streams);
            InnerDriver.Projections.Register(projector);
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

            /// <summary>
            /// All streams used by this projector.
            /// </summary>
            public IEventStream[] Streams { get; private set; }

            public Projector(string name, EntityStore<TEn> entityStore, IEntityStoreProjection<TEv, TEn> projection, IEventStream[] streams)
            {
                _name = name;
                _entityStore = entityStore;
                _projection = projection;
                _pendingEvents = new List<KeyValuePair<Id, TEv>>();
                Streams = streams;
            }

            public string Name { get { return _name; } }

            public bool CommitRecommended { get { return _pendingEvents.Count > 100; } }

            public void ProcessEvent(EventInStream<TEv> ev)
            {
                var id = _projection.EventIdentifier(ev.Event);
                if (id == null) return;
                _pendingEvents.Add(new KeyValuePair<Id, TEv>((Id)id, ev.Event));
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
