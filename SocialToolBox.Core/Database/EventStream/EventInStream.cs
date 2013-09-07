using System.Diagnostics;

namespace SocialToolBox.Core.Database.EventStream
{
    /// <summary>
    /// An event inside an <see cref="IEventStream"/>. Includes the actual event, 
    /// and the event's position inside the original stream.
    /// </summary>
    public class EventInStream<T> where T : class
    {
        public readonly IEventStream Stream;
        public readonly T Event;
        public readonly long Position;

        public EventInStream(IEventStream stream, T e, long pos)
        {
            Debug.Assert(stream != null);
            Debug.Assert(e != null);
            Debug.Assert(pos >= 0);

            Stream = stream;
            Event = e;
            Position = pos;
        }
    }
}
