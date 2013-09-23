using System;
using System.Threading.Tasks;
using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Extensions for the projection extensions that implement more complex behavior
    /// on top of the shared interface.
    /// </summary>
    public static class ProjectionExtensions
    {
        /// <summary>
        /// Create a store view using a mutator function. Events are expected to have an
        /// identifier property used to match them up with the target.
        /// </summary>
        public static IStore<TEn> CreateStore<TEv, TEn>(
            this IProjection<TEv> projection, 
            string name, 
            Func<TEv, TEn, TEn> mutate,
            IEventStream[] streams)
 
            where TEv : class, IEventWithId
            where TEn : class
        {
            return projection.Create(name, new StoreWithMutatorProjection<TEv, TEn>(mutate), streams);
        }

        /// <summary>
        /// Create a store view using a mutator visitor. Events are expected to have an
        /// identifier property used to match them up with the target.
        /// </summary>
        public static IStore<TEn> CreateStore<TEv, TEn>(
            this IProjection<TEv> projection,
            string name,
            Visitor<TEn, TEn> mutate,
            IEventStream[] streams)

            where TEv : class, IEventWithId
            where TEn : class
        {
            return projection.Create(name, new StoreWithMutatorProjection<TEv, TEn>(mutate.Visit), streams);
        }

        /// <summary>
        /// Private implementation for a store projection using a mutator function.
        /// </summary>
        private class StoreWithMutatorProjection<TEv, TEn> : IStoreProjection<TEv, TEn>
            where TEv : class, IEventWithId
            where TEn : class
        {
            private readonly Func<TEv, TEn, TEn> _mutator;

            public StoreWithMutatorProjection(Func<TEv, TEn, TEn> mutator)
            {
                _mutator = mutator;
            }

            public async Task Process(IWritableStore<TEn> store, EventInStream<TEv> ev)
            {
                var id = ev.Event.Id;
                var current = await store.Get(id);
                await store.Set(id, _mutator(ev.Event,current));
            }
        }
 
    }
}
