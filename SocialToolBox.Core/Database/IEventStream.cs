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
    }
}
