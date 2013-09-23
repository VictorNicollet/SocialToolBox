using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.User.Event
{
    /// <summary>
    /// An event which was performed by an user (called
    /// the author).
    /// </summary>
    public interface IEventByUser
    {
        Id AuthorId { get; }
    }
}
