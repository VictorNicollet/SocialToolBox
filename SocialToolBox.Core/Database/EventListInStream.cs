using System.Collections.Generic;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// A list of events returned by a database request.
    /// </summary>
    /// <remarks>
    /// There is no requirement for event positions to be consecutive, though
    /// they are expected to be increasing and smaller than the next position.
    /// This is to allow filtering requests (get only events of a certain type,
    /// for instance).
    /// </remarks>
    public class EventListInStream<T> : List<EventInStream<T>> where T : class
    {
        /// <summary>
        /// The position of the next event (right after the last event 
        /// returned by the request that generated this list).
        /// </summary>
        public readonly long NextPosition;

        /// <summary>
        /// The number of events that were read from the database to
        /// generate this result, including the discarded ones.
        /// </summary>
        public readonly int RealCount;

        public EventListInStream(IEnumerable<EventInStream<T>> events, int nextPos, int realCount)
            : base(events)
        {
            NextPosition = nextPos;
            RealCount = realCount;
        }
    }
}
