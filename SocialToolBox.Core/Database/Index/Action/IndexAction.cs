using System.Threading.Tasks;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Database.Index.Action
{
    /// <summary>
    /// An action applied to a writable index at a specified id, with a projection
    /// cursor.
    /// </summary>
    public delegate Task IndexAction<out TSet, TSort>(
        IWritableIndex<TSet, TSort> index, Id id, IProjectCursor cursor)
        where TSet : class where TSort : class;

    /// <summary>
    /// Provides several standard index actions.
    /// </summary>
    public static class IndexAction
    {
        /// <summary>
        /// An action that deletes the value from the index.
        /// </summary>
        public static IndexAction<TSet, TSort> Delete<TSet,TSort>()
            where TSet : class where TSort : class
        {
            return (index, id, cursor) => index.Delete(id, cursor);
        }

        /// <summary>
        /// An action that inserts or updates a binding in the index.
        /// </summary>
        public static IndexAction<TSet, TSort> Set<TSet, TSort>(TSet set, TSort sort)
            where TSet : class where TSort : class
        {
            return (index, id, cursor) => index.Set(id, set, sort, cursor);
        }
    }
}
