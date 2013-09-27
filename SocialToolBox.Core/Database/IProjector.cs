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
        /// Process one event. 
        /// </summary>
        Task ProcessEvent(EventInStream<T> ev, IProjectTransaction t);

        /// <summary>
        /// The streams from which this projector reads.
        /// </summary>
        IEventStream[] Streams { get; }
    }
}
