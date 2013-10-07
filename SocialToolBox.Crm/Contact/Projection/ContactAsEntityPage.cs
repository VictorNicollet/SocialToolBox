using SocialToolBox.Core;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;
using SocialToolBox.Core.Entity.Projection;
using SocialToolBox.Crm.Contact.Event;

namespace SocialToolBox.Crm.Contact.Projection
{
    /// <summary>
    /// A contact, seen as an entity page.
    /// </summary>
    [Persist("SocialToolBox.Crm.Contact.Projection.ContactAsEntityPage")]
    public class ContactAsEntityPage : IEntityPage
    {
        /// <summary>
        /// The name of the contact.
        /// </summary>
        [PersistMember(0)]
        public string Name { get; private set; }

        public ContactAsEntityPage() {}

        public string Title { get { return Name; } }

        /// <summary>
        /// Extends an entity page visitor to react to contact events.
        /// </summary>
        public static void ExtendVisitor(Visitor<IEntityPageEvent, IEntityPage, IEntityPage> visitor)
        {
            visitor.On<ContactCreated>((e,i) => new ContactAsEntityPage());
            visitor.On<ContactDeleted>((e,i) => null);

            visitor.On<ContactNameUpdated>((e, i) =>
            {
                var old = i == null ? null : i as ContactAsEntityPage;
                if (old != null) old.Name = e.Name;
                return i;
            });
        }
    }
}
