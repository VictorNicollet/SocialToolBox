using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Crm.Contact.Event
{
    /// <summary>
    /// A new contact is created. It does not yet have a name or e-mail.
    /// </summary>
    [Persist("SocialToolBox.Crm.Contact.Event.ContactCreated")]
    public class ContactCreated : IContactEvent, IEntityPageEvent
    {
        /// <summary>
        /// The identifier of the created contact.
        /// </summary>
        [PersistMember(0)]
        public Id Id { get; private set; }

        /// <summary>
        /// The time when the contact was created.
        /// </summary>
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        /// <summary>
        /// The author who created the contact.
        /// </summary>
        [PersistMember(2)]
        public Id AuthorId { get; private set; }
        
        public ContactCreated() {}

        public ContactCreated(Id id, DateTime time, Id author)
        {
            Id = id;
            Time = time;
            AuthorId = author;
        }

        public Id EntityId { get { return Id; } }
        
        public bool EntityTitleChanged { get { return true; } }
    }
}
