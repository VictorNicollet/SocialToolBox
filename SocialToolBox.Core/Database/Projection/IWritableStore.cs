using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// A writable version of <see cref="IStore{T}"/>
    /// </summary>
    public interface IWritableStore<T> : IStore<T> where T : class
    {
        /// <summary>
        /// Writes an item to the store. If the specified item is null,
        /// the corresponding id is unbound instead.
        /// </summary>
        Task Write(Id id, T item);
    }
}
