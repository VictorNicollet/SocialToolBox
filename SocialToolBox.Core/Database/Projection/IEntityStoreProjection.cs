namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Implement this interface to configure a projection that
    /// keeps an entity store updated.
    /// </summary>
    public interface IEntityStoreProjection<in TEvent,TEntity> : IIdExtractor<TEvent> 
        where TEvent : class where TEntity : class
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
    }
}
