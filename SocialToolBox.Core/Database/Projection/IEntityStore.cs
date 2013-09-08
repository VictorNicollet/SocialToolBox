using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Entity stores map <see cref="Id"/> to entities, and keep these
    /// entities updated. This represents the reading interface to an
    /// entity store.
    /// </summary>
    public interface IEntityStore<T> where T : class
    {
        /// <summary>
        /// Gets the entity with the given identifier, or null if 
        /// no such entity exists.
        /// </summary>
        Task<T> Get(Id id);

        /// <summary>
        /// Grabs several identifiers at the same time. Returned
        /// dictionary only includes existing entities. 
        /// </summary>
        Task<IDictionary<Id, T>> GetAll(IEnumerable<Id> ids);

        /// <summary>
        /// Determines whether the specified entity exists.
        /// </summary>
        Task<bool> Exists(Id id);
    }
}
