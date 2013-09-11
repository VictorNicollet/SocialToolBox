using SocialToolBox.Core.Database.Event;

namespace SocialToolBox.Core.User.Event
{
    /// <summary>
    /// Events concerning the user.
    /// </summary>
    public interface IUserEvent : IEventWithTime, IEventWithId
    {
    }
}
