using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Crm.Contact.Event
{
    /// <summary>
    /// The name (first and last) of a contact was updated.
    /// </summary>
    [Persist("SocialToolBox.Crm.Contact.Event.ContactNameUpdated")]
    public class ContactNameUpdated : IContactEvent, IEntityPageEvent
    {
        /// <summary>
        /// The Id of the renamed contact.
        /// </summary>
        [PersistMember(0)]
        public Id Id { get; private set; }

        /// <summary>
        /// The time when the rename was performed.
        /// </summary>
        [PersistMember(1)]
        public DateTime Time { get; private set; }

        /// <summary>
        /// The person who performed the rename.
        /// </summary>
        [PersistMember(2)]
        public Id AuthorId { get; private set; }

        /// <summary>
        /// The new first name.
        /// </summary>
        [PersistMember(3)]
        public string Firstname { get; private set; }

        /// <summary>
        /// The new last name.
        /// </summary>
        [PersistMember(4)]
        public string Lastname { get; private set; }

        /// <summary>
        /// The complete name, as a concatenation of the first and last name,
        /// if available.
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Firstname)) return Lastname;
                if (string.IsNullOrWhiteSpace(Lastname)) return Firstname;
                return string.Format("{0} {1}", Firstname.TrimEnd(' '), Lastname.TrimStart(' '));
            }
        }

        public ContactNameUpdated() {}

        public ContactNameUpdated(Id id, DateTime time, Id author, string firstname, string lastname)
        {
            Id = id;
            Time = time;
            AuthorId = author;
            Firstname = firstname;
            Lastname = lastname;
        }

        public Id EntityId { get { return Id; } }
        
        public bool EntityTitleChanged { get { return true; } }
    }
}
