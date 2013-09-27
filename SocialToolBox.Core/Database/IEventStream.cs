using System.Threading.Tasks;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// An event stream from a database. Stores a sequence of events.
    /// </summary>
    public interface IEventStream
    {
        /// <summary>
        /// The driver from which this event stream was obtained.
        /// </summary>
        IDatabaseDriver Driver { get; }

        /// <summary>
        /// The name of the event stream.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Appends a new event to the event stream.
        /// </summary>
        Task AddEvent(object e, ICursor t);

        /// <summary>
        /// Grab the event at the specified position, if it exists.
        /// </summary>
        /// <remarks>
        /// Throws an exception if unserialization or cast to the specified
        /// type fails. Returns <code>null</code> if there is no event at
        /// the specified position.
        /// </remarks>
        Task<EventInStream<T>> GetEvent<T>(long position, IProjectCursor t) 
            where T : class;

        /// <summary>
        /// Grab up to the maximum number of events from the stream started 
        /// at the specified position. 
        /// </summary>
        /// <remarks>
        /// Throws an exception if unserialization or cast to the specified
        /// type fails. 
        /// </remarks>
        Task<EventListInStream<T>> GetEvents<T>(long startPosition, int count, IProjectCursor t) 
            where T : class;

        /// <summary>
        /// Grab up to the maximum number of events from the stream started
        /// at the specified position
        /// </summary>
        /// <remarks>
        /// Events that cannot be cast to the specified type are discarded
        /// from the returned list.
        /// </remarks>
        Task<EventListInStream<T>> GetEventsOfType<T>(long startPosition, int count, IProjectCursor t) 
            where T : class;

        /// <summary>
        /// The number of events in this stream. Also, the position of the
        /// next event to be inserted in this stream.
        /// </summary>
        Task<long> NextPosition(IReadCursor t);
    }
}
