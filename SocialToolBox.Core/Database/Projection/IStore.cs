using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// A read-only dictionary, stores items of the specified type by their
    /// identifier. Items must be persistable.
    /// </summary>
    public interface IStore<T> where T : class
    {
        /// <summary>
        /// Get the item using its identifier. Returns null if no item
        /// was found.
        /// </summary>
        Task<T> Get(Id id);
    }
}
