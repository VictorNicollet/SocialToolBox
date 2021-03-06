﻿using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// A given projection allows the system to combine several 
    /// projectors into one, and to update several views at once
    /// from an individual event. It also allows cross-view 
    /// queries within the same projection.
    /// </summary>
    public interface IProjection<T> where T : class
    {
        /// <summary>
        /// The name of the projection, as defined when it was created.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The database driver from which this projection was created.
        /// </summary>
        IDatabaseDriver Driver { get; }

        /// <summary>
        /// Create an entity store using the specified projection.
        /// </summary>
        IStore<TEn> Create<TEn>(string name, IStoreProjection<T, TEn> proj,
            IEventStream[] streams)
            where TEn : class;

        /// <summary>
        /// Create an index using the specified projection.
        /// </summary>
        IIndex<TSet, TSort> Create<TSet, TSort>(string name, IIndexProjection<T, TSet, TSort> proj,
            IEventStream[] streams)
            where TSet : class
            where TSort : class;

        /// <summary>
        /// Create a manually updated index. This is usually updated by reacting to 
        /// changes on other objects within this projection.
        /// </summary>
        IWritableIndex<TSet, TSort> CreateManual<TSet, TSort>(string name)
            where TSet : class
            where TSort : class;

        /// <summary>
        /// Compiles the projection. This means using the projection contents
        /// becomes allowed, but adding new views or projectors becomes 
        /// forbidden. 
        /// </summary>
        void Compile();
    }
}
