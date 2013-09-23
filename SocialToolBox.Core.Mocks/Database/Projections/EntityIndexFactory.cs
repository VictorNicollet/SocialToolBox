using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
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
            var projector = new Projector<TEv, TSet, TSort, TEn>(name, store, proj, streams);
            InnerDriver.Projections.Register(projector);
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

            /// <summary>
            /// All event streams read by this projector.
            /// </summary>
            public IEventStream[] Streams { get; private set; }

            public Projector(string name, EntityIndex<TSet,TSort,TEn> entityIndex, 
                IEntityIndexProjection<TEv, TEn, TSet, TSort> projection, IEventStream[] streams)
            {
                _name = name;
                _entityIndex = entityIndex;
                _projection = projection;
                _pendingEvents = new List<IPair<Id, TEv>>();
                Streams = streams;
            }

            public string Name { get { return _name; } }

            public bool CommitRecommended { get { return _pendingEvents.Count > 100; } }

            public void ProcessEvent(EventInStream<TEv> ev)
            {
                var id = _projection.EventIdentifier(ev.Event);
                if (id == null) return;
                _pendingEvents.Add(Pair.Make((Id)id, ev.Event));
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
