using System.Threading.Tasks;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// A transaction which supports both read and write operations, used
    /// by projectors.
    /// </summary>
    public interface IProjectCursor : IReadCursor
    {
        /// <summary>
        /// Commits the operations performed under the current 
        /// transaction.
        /// </summary>
        Task Commit();

        /// <summary>
        /// The load on this transaction, approximating the number of
        /// queries so far. When this number is high enough, a commit
        /// should be forced.
        /// </summary>
        int Load { get; }
    }
}
