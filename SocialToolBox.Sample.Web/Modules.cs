using SocialToolBox.Core.Database;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Web;
using SocialToolBox.Crm.Contact;

namespace SocialToolBox.Sample.Web
{
    /// <summary>
    /// All the modules enabled on this project.
    /// </summary>
    public class Modules
    {
        /// <summary>
        /// The database driver used by all modules.
        /// </summary>
        public IDatabaseDriver Database;

        /// <summary>
        /// The web driver used for registering actions.
        /// </summary>
        public IWebDriver Web;

        /// <summary>
        /// CRM Contacts.
        /// </summary>
        public readonly ContactModule Contacts;
        
        /// <summary>
        /// Entities (especially entity pages).
        /// </summary>
        public readonly EntityModule Entities;

        public Modules()
        {
            Database = new DatabaseDriver();
            Web = new WebDriver();

            // Instantiate all modules
            Contacts = new ContactModule(Database);
            Entities = new EntityModule(Database);

            // Set up bridges between modules
            Contacts.RegisterContactsAsEntities(Entities);

            // Compile all modules
            Contacts.Compile();
            Entities.Compile();
        }
    }
}