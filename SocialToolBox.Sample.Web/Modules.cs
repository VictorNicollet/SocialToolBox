using SocialToolBox.Core.Database;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Entity.Web;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Present;
using SocialToolBox.Core.Present;
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

        /// <summary>
        /// Registered web endpoints for entity pages.
        /// </summary>
        public readonly EntityPageFacet EntityPages;

        public Modules()
        {
            Database = new DatabaseDriver();
            Web = new WebDriver(new NaiveRenderingStrategy<IWebRequest>(new NodeRenderer()));

            // Instantiate all modules
            Contacts = new ContactModule(Database);
            Entities = new EntityModule(Database);

            // Set up bridges between modules
            Contacts.RegisterContactsAsEntities(Entities);

            // Compile all modules
            Contacts.Compile();
            Entities.Compile();

            // Install facets
            EntityPages = new EntityPageFacet(Web, Entities);

            // Initialize modules
            InitialData.AddTo(this);
        }
    }
}