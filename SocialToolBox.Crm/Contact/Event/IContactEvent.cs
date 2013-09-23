using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Crm.Contact.Event
{
    /// <summary>
    /// An event related to a contact, as part of the CRM module set.
    /// </summary>
    public interface IContactEvent : IEventWithId, IEventWithTime, IEventByUser
    {
    }
}
