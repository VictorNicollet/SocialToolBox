﻿using System;
using System.Threading.Tasks;
using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Database.Index.Action;

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
            Visitor<TEv, TEn, TEn> mutate,
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

            public async Task Process(IWritableStore<TEn> store, EventInStream<TEv> ev, IProjectCursor cursor)
            {
                var id = ev.Event.Id;
                var current = await store.Get(id, cursor);
                await store.Set(id, _mutator(ev.Event,current), cursor);
            }
        }

        /// <summary>
        /// Create a store view using a mutator function and an identifier extractor that
        /// returns the identifier used for each event.
        /// </summary>
        public static IStore<TEn> CreateStore<TEv, TEn>(
            this IProjection<TEv> projection,
            string name,
            Func<TEv,Id?> getId,
            Func<TEv, TEn, TEn> mutate,
            IEventStream[] streams)

            where TEv : class
            where TEn : class
        {
            return projection.Create(name, new StoreWithIdAndMutatorProjection<TEv, TEn>(getId,mutate), streams);
        }

        /// <summary>
        /// Create a store view using a mutator visitor and an identifier extractor that
        /// returns the identifier used for each event.
        /// </summary>
        public static IStore<TEn> CreateStore<TEv, TEn>(
            this IProjection<TEv> projection,
            string name,
            Func<TEv, Id?> getId,
            Visitor<TEv, TEn, TEn> mutate,
            IEventStream[] streams)

            where TEv : class
            where TEn : class
        {
            return projection.Create(name, new StoreWithIdAndMutatorProjection<TEv, TEn>(getId,mutate.Visit), streams);
        }

        /// <summary>
        /// Private implementation for a store projection using a mutator function.
        /// </summary>
        private class StoreWithIdAndMutatorProjection<TEv, TEn> : IStoreProjection<TEv, TEn>
            where TEv : class
            where TEn : class
        {
            private readonly Func<TEv, TEn, TEn> _mutator;
            private readonly Func<TEv, Id?> _getId;

            public StoreWithIdAndMutatorProjection(Func<TEv,Id?> getId,Func<TEv, TEn, TEn> mutator)
            {
                _mutator = mutator;
                _getId = getId;
            }

            public async Task Process(IWritableStore<TEn> store, EventInStream<TEv> ev, IProjectCursor cursor)
            {
                var id = _getId(ev.Event);
                if (id == null) return;                
                var current = await store.Get((Id)id, cursor);
                await store.Set((Id)id, _mutator(ev.Event, current), cursor);
            }
        }

        /// <summary>
        /// Create an index from an updater function that is called on every event
        /// and has access to the writable index.
        /// </summary>
        public static IIndex<TSet, TSort> CreateIndex<TEv, TSet, TSort>(
            this IProjection<TEv> proj,
            string name,
            Func<IWritableIndex<TSet, TSort>, TEv, IProjectCursor, Task> updater,
            IEventStream[] streams)

            where TEv : class
            where TSet : class
            where TSort : class
        {
            var projection = new IndexWithUpdater<TEv, TSet, TSort>(updater);
            return proj.Create(name, projection, streams);
        }

        /// <summary>
        /// An index projection that forwards events to an updater function.
        /// </summary>
        private class IndexWithUpdater<TEv, TSet, TSort> : IIndexProjection<TEv, TSet, TSort>
            where TEv : class
            where TSet : class
            where TSort : class
        {
            private readonly Func<IWritableIndex<TSet, TSort>, TEv, IProjectCursor, Task> _updater;

            public IndexWithUpdater(Func<IWritableIndex<TSet, TSort>, TEv, IProjectCursor, Task> updater)
            {
                _updater = updater;
            }

            public Task Process(IWritableIndex<TSet, TSort> index, EventInStream<TEv> ev, IProjectCursor cursor)
            {
                return _updater(index, ev.Event, cursor);
            }
        }

        /// <summary>
        /// Create an index that is updated when changes are applied to the
        /// provided store.
        /// </summary>
        public static IIndex<TSet, TSort> CreateIndex<TEv, TEn, TSet, TSort>(
            this IProjection<TEv> proj,
            string name,
            IStore<TEn> store,
            Func<ValueChangedEventArgs<TEn>, IndexAction<TSet,TSort>> extract)

            where TEv : class
            where TEn : class
            where TSet : class
            where TSort : class
        {
            var index = proj.CreateManual<TSet, TSort>(name);
            store.ValueChanged += args =>
            {
                var action = extract(args);
                if (action != null)
                {
                    var task = action(index, args.Id, args.Cursor);
                    if (task.Status == TaskStatus.WaitingToRun) task.Start();
                }        
                    
            };
            return index;
        }

        /// <summary>
        /// Uses an extractor to obtain a key from every entity in the store. 
        /// Updates the index when the key changes.
        /// </summary>
        public static IIndex<TSet, TSort> CreateIndex<TEv, TEn, TSet, TSort>(
            this IProjection<TEv> proj,
            string name,
            IStore<TEn> store,
            Func<TEn, IPair<TSet, TSort>> extract)

            where TEv : class
            where TEn : class
            where TSet : class
            where TSort : class
        {
            var setC = new IndexKeyComparer<TSet>();
            var sortC = new IndexKeyComparer<TSort>();

            return proj.CreateIndex(name, store, args =>
            {
                var oldKeys = args.OldValue == null ? null : extract(args.OldValue);
                var newKeys = args.NewValue == null ? null : extract(args.NewValue);

                // Nothing changed, return a no-operation
                if (newKeys == null && oldKeys == null)
                    return null;

                // Object was deleted, return deletion
                if (oldKeys != null && newKeys == null) 
                    return IndexAction.Delete<TSet, TSort>();

                // Object was created, return standard set
                if (oldKeys == null) 
                    return IndexAction.Set(newKeys.First, newKeys.Second);

                // Object was not changed, return a no-operation
                if (setC.Compare(oldKeys.First, newKeys.First) == 0
                    && sortC.Compare(oldKeys.Second, newKeys.Second) == 0)
                    return null;

                // Object was updated, return updater
                return IndexAction.Set(newKeys.First, newKeys.Second);
            });
        }
    }
}
