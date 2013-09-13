using System.Collections.Generic;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Implement this interface to configure a projection that
    /// keeps an entity index updated.
    /// </summary>
    public interface IEntityIndexProjection<in TEvent, TEntity, out TSet, out TSort> : IIdExtractor<TEvent>
        where TEvent : class
        where TEntity : class
        where TSet : class 
        where TSort : class
    {
        /// <summary>
        /// Updates an entity based on an event.
        /// </summary> 
        /// <remarks>
        /// The entity is loaded by identifier (using the identifier 
        /// extracted by <see cref="IIdExtractor{T}"/>) and is null if missing.
        /// The returned entity will be written back to the store.
        /// </remarks>
        TEntity Update(Id id, TEvent ev, TEntity old);

        /// <summary>
        /// Lists all sets in which an entity appears, and the sort order in each 
        /// of these sets.
        /// </summary>
        IEnumerable<IPair<TSet, TSort>> Sets(Id id, TEntity entity);
    }
}
