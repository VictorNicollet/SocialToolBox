using SocialToolBox.Core.Entity;
using SocialToolBox.Crm.Contact.Projection;

namespace SocialToolBox.Crm.Contact
{
    /// <summary>
    /// Extensions involved in making the contact module connect to
    /// the entity module.
    /// </summary>
    public static class ContactModuleEntityExtensions
    {
        /// <summary>
        /// Register all contacts as entities : these will appear as entity
        /// pages, in entity searches, and so on.
        /// </summary>
        public static void RegisterContactsAsEntities(this ContactModule cmodule, EntityModule emodule)
        {
            emodule.AddEventStream(cmodule.Stream);
            ContactAsEntityPage.ExtendVisitor(emodule.PageEventVisitor);
        }
    }
}
