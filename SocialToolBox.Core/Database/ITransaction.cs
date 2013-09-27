namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// A transaction which supports both read and write operations.
    /// </summary>
    public interface ITransaction : IReadTransaction
    {
        /// <summary>
        /// Commits the operations performed under the current 
        /// transaction.
        /// </summary>
        void Commit();
    }
}
