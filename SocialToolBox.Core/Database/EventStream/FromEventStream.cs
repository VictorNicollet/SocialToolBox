using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.EventStream
{
    /// <summary>
    /// Allows iterating through one or more event streams.
    /// </summary>
    public static class FromEventStream
    {
        /// <summary>
        /// Filtering iterators read only events of the 
        /// specified type from all input streams.
        /// </summary>
        private class FilterIterator<T> : IMultiStreamIterator<T> where T : class
        {
            /// <summary>
            /// The streams from which events are read (by order of 
            /// priority).
            /// </summary>
            private readonly IEventStream[] _streams;

            /// <summary>
            /// The current (internal) vector clock of this iterator. Advances
            /// with fetch queries.
            /// </summary>
            private readonly VectorClock _clock;

            /// <summary>
            /// The current public vector clock of this iterator. Advances 
            /// when events are returned.
            /// </summary>
            private readonly VectorClock _publicClock;

            /// <summary>
            /// The number of events fetched by each query.
            /// </summary>
            private const int FetchCount = 1000;

            /// <summary>
            /// The events that have been fetched from the stream with the
            /// lowest index that did not return an empty list. Reversed
            /// to allow faster reading from the end.
            /// </summary>
            private readonly List<EventInStream<T>> _currentFetchedEvents;

            public VectorClock VectorClock { get { return _publicClock; } }

            public FilterIterator(VectorClock clock, IEventStream[] streams)
            {
                _currentFetchedEvents = new List<EventInStream<T>>();
                _streams = streams;
                _clock = clock;
                _publicClock = clock.Clone();
            }

            public async Task<EventInStream<T>> NextAsync()
            {
                // If no cached events, try fetching some
                if (_currentFetchedEvents.Count == 0)
                {
                    foreach (var stream in _streams)
                    {
                        while (_currentFetchedEvents.Count == 0)
                        {
                            var events = await stream.GetEventsOfType<T>(_clock.GetNextInStream(stream), FetchCount);
                            
                            // No more events left in stream : give up and try next stream
                            if (events.RealCount == 0) break;

                            // To avoid re-fetching this set of values again
                            _clock.Advance(stream, events.NextPosition);
                            
                            _currentFetchedEvents.InsertRange(0, Enumerable.Reverse(events));
                        }

                        if (_currentFetchedEvents.Count > 0) break;
                    }
                }

                // If still no cached events, give up
                if (_currentFetchedEvents.Count == 0) return null;
                
                // Cached events found, pick the first
                var first = _currentFetchedEvents[_currentFetchedEvents.Count - 1];
                _currentFetchedEvents.RemoveAt(_currentFetchedEvents.Count - 1);
                _publicClock.Advance(first);
                return first;
            }

            public EventInStream<T> Next()
            {
                return NextAsync().Result;                
            }

            public IEnumerator<EventInStream<T>> GetEnumerator()
            {
                while (true)
                {
                    var next = Next();
                    if (next == null) yield break;
                    yield return next;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// Returns an iterator that traverses all provided streams, returning only events of the
        /// specified type.
        /// </summary>
        /// <remarks>
        /// Streams are traversed in order. That is, stream 0 will be entirely processed
        /// before stream 1 starts being processed, and so on.
        /// </remarks>
        public static IMultiStreamIterator<T> EachOfType<T>(VectorClock clock, params IEventStream[] streams) where T : class
        {
            return new FilterIterator<T>(clock, streams);    
        }
    }
}
