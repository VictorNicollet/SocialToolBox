using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// Receives events for processing, commits results for persistence.
    /// </summary>
    public interface IProjector<T> where T : class
    {
        /// <summary>
        /// The name of the projector. Used to save/load the vector clock
        /// for the corresponding projection.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Given the current state of the projector, a commit is recommended.
        /// </summary>
        bool CommitRecommended { get; }

        /// <summary>
        /// Process one event. This is allowed to have side-effects,
        /// as long as those side-effecs are only made permanent 
        /// after <see cref="Commit()"/> is called.
        /// </summary>
        void ProcessEvent(EventInStream<T> ev);

        /// <summary>
        /// Make all operations performed so far permanent.
        /// </summary>
        Task Commit();

        /// <summary>
        /// The streams from which this projector reads.
        /// </summary>
        IEventStream[] Streams { get; }
    }
}
