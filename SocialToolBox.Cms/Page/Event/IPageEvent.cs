using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Cms.Page
{
    /// <summary>
    /// An individual event about a CMS page.
    /// </summary>
    public interface IPageEvent : IEventWithId, IEventWithTime, IEventByUser
    {
    }
}
