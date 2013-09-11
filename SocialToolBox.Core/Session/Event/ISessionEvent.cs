using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Event;

namespace SocialToolBox.Core.Session.Event
{
    /// <summary>
    /// Session events apply to sessions at specific times.
    /// </summary>
    public interface ISessionEvent : IEventWithTime
    {
        /// <summary>
        /// A session identifier.
        /// </summary>
        Id Session { get; }
    }
}
