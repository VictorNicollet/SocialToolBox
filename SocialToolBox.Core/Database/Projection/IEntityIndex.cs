using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Entity indexing uses binds a key to a list of entities.
    /// </summary>
    /// <typeparam name="TSetKey">
    /// A key parameter to identify a set of entities by equality,
    /// supporting queries like "what entities have this exact
    /// set key?"
    /// </typeparam>
    /// <typeparam name="TSortKey">
    /// A key parameter to sort a set of entities (within a given
    /// set), supporting queries like "what entities have a
    /// set key between these two?"
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The entity stored in this set.
    /// </typeparam>
    public interface IEntityIndex<TSetKey,TSortKey,TEntity> 
        where TEntity : class
        where TSetKey : class
        where TSortKey : class
    {
        /// <summary>
        /// Lists all sets in which an entity appears, and the sort order in each 
        /// of these sets.
        /// </summary>
        IEnumerable<IPair<TSetKey, TSortKey>> Sets(Id id, TEntity entity);

        /// <summary>
        /// Get the entity with the given identifier, or null if no such 
        /// entity exists.
        /// </summary>
        Task<TEntity> Get(Id id);

        /// <summary>
        /// Grabs several identifiers at the same time. Returned
        /// dictionary only includes existing entities. 
        /// </summary>
        Task<IDictionary<Id, TEntity>> GetAll(IEnumerable<Id> ids);

        /// <summary>
        /// Get entities from a set, starting at an optional offset, up to 
        /// a certain count (in the order specified by the sort key).
        /// </summary>
        /// <param name="set">Query entities in this set.</param>
        /// <param name="count">Maximum number of entities to return.</param>
        /// <param name="offset">Skip this number of entities.</param>
        /// <param name="greaterThan">
        /// Entities should have a sort key greater than or equal to this value.
        /// Ignored if null.
        /// </param>
        /// <param name="lessThan">
        /// Entities should have a sort key less than or equal to this value.
        /// Ignored if null.
        /// </param>
        /// <param name="descending">
        /// Reverse the order of the sort keys.
        /// </param>
        Task<IPair<Id,TEntity>[]> Query(TSetKey set, int count, int offset = 0, 
            TSortKey greaterThan = null,
            TSortKey lessThan = null,
            bool descending = false);
    }
}
