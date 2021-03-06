﻿using System;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Mocks.Database.Projections;

namespace SocialToolBox.Core.Mocks.Database
{
    /// <summary>
    /// An in-memory implementation of a projection.
    /// </summary>
    public class Projection<T> : IProjection<T> where T : class
    {
        public string Name { get; private set; }

        /// <summary>
        /// Whether this projection has already been compiled.
        /// </summary>
        private bool _isCompiled;

        /// <summary>
        /// The actual database, of its known "mock" type.
        /// </summary>
        public readonly DatabaseDriver RealDriver;

        public IDatabaseDriver Driver { get { return RealDriver; } }

        /// <summary>
        /// A multi-projector that holds all registered projectors.
        /// </summary>
        public MultiProjector<T> Projector;

        public Projection(string name, DatabaseDriver driver)
        {
            Name = name;
            RealDriver = driver;
            Projector = new MultiProjector<T>(name);    
        }

        public IStore<TEn> Create<TEn>(string name, IStoreProjection<T, TEn> proj, IEventStream[] streams) 
            where TEn : class
        {
            if (_isCompiled)
                throw new InvalidOperationException(
                    string.Format("Projection {0} is already compiled.", Name));

            var store = new InMemoryStore<TEn>(Driver);
            Projector.Register(new StoreProjector<TEn>(proj, store) {Name = name, Streams = streams});
            return store;
        }

        /// <summary>
        /// A projector for store views.
        /// </summary>
        private class StoreProjector<TEn> : IProjector<T> where TEn : class
        {
            public StoreProjector(IStoreProjection<T, TEn> proj, IWritableStore<TEn> store)
            {
                _projection = proj;
                _store = store;
            }

            public string Name { get; set; }

            private readonly IStoreProjection<T, TEn> _projection;

            private readonly IWritableStore<TEn> _store;

            // ReSharper disable CSharpWarnings::CS1998
            public async Task ProcessEvent(EventInStream<T> ev, IProjectCursor t)
            // ReSharper restore CSharpWarnings::CS1998
            {
                _projection.Process(_store, ev, t).Wait();
            }

            public IEventStream[] Streams { get; set; }
        }

        public IIndex<TSet, TSort> Create<TSet, TSort>(string name, IIndexProjection<T, TSet, TSort> proj, IEventStream[] streams) 
            where TSet : class where TSort : class
        {
            if (_isCompiled)
                throw new InvalidOperationException(
                    string.Format("Projection {0} is already compiled.", Name));

            var index = new InMemoryIndex<TSet, TSort>();
            Projector.Register(new IndexProjector<TSet,TSort>(proj, index){Name = name, Streams = streams});
            return index;
        }

        public IWritableIndex<TSet, TSort> CreateManual<TSet, TSort>(string name) where TSet : class where TSort : class
        {
            if (_isCompiled)
                throw new InvalidOperationException(
                    string.Format("Projection {0} is already compiled.", Name));

            Projector.RegisterManual(name);
            return new InMemoryIndex<TSet, TSort>();
        }

        /// <summary>
        /// A projector for store views.
        /// </summary>
        private class IndexProjector<TSet,TSort> : IProjector<T> where TSet : class where TSort : class
        {
            public IndexProjector(IIndexProjection<T, TSet, TSort> proj, IWritableIndex<TSet,TSort> index)
            {
                _projection = proj;
                _index = index;
            }

            public string Name { get; set; }

            private readonly IIndexProjection<T, TSet, TSort> _projection;

            private readonly IWritableIndex<TSet, TSort> _index;

            // ReSharper disable CSharpWarnings::CS1998
            public async Task ProcessEvent(EventInStream<T> ev, IProjectCursor t)
            // ReSharper restore CSharpWarnings::CS1998
            {
                _projection.Process(_index, ev, t).Wait();
            }

            public IEventStream[] Streams { get; set; }
        }


        public void Compile()
        {
            if (_isCompiled) return;
            _isCompiled = true;

            Driver.Projections.Register(Projector);
        }
    }
}
