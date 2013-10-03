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
        /// The database driver from which this store was spawned.
        /// </summary>
        IDatabaseDriver Driver { get; }

        /// <summary>
        /// Get the item using its identifier. Returns null if no item
        /// was found.
        /// </summary>
        Task<T> Get(Id id, IReadCursor cursor);

        /// <summary>
        /// Event triggered when the value at a specific id changes
        /// while the projection is being built. Handlers should only
        /// be attached within the same projection.
        /// </summary>
        event ValueChangedEvent<T> ValueChanged;
    }
}
