using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Crm.Contact.Event
{
    /// <summary>
    /// A contact was deleted and should not appear anymore.
    /// </summary>
    [Persist("SocialToolBox.Crm.Contact.Event.ContactDeleted")]
    public class ContactDeleted : IContactEvent, IEntityPageEvent
    {
        /// <summary>
        /// The identifier of the deleted contact.
        /// </summary>
        [PersistMember(0)]
        public Id Id { get; private set; }

        /// <summary>
        /// When the contact was deleted.
        /// </summary>
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        /// <summary>
        /// Who deleted the contact.
        /// </summary>
        [PersistMember(2)]
        public Id AuthorId { get; private set; }
        
        public ContactDeleted() {}

        public ContactDeleted(Id id, DateTime time, Id author)
        {
            Id = id;
            Time = time;
            AuthorId = author;
        }

        public Id EntityId { get { return Id; } }
    }
}
