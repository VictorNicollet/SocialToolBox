using SocialToolBox.Core.Database;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Entity.Web;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Present.Bootstrap3;
using SocialToolBox.Core.Present.RenderingStrategy;
using SocialToolBox.Core.Web;
using SocialToolBox.Crm.Contact;

namespace SocialToolBox.Sample.Web
{
    /// <summary>
    /// All the modules enabled on this project.
    /// </summary>
    public class SocialModules
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

        private SocialModules()
        {
            Database = new DatabaseDriver();
            Web = new WebDriver(new NaiveRenderingStrategy<IWebRequest>(new PageNodeRenderer()));

            // Instantiate all modules
            Contacts = new ContactModule(Database);
            Entities = new EntityModule(Database);

            // Set up bridges between modules
            Contacts.RegisterContactsAsEntities(Entities);

            // Compile all modules, start background thread
            Contacts.Compile();
            Entities.Compile();

            Database.Projections.StartBackgroundThread();

            // Install facets
            EntityPages = new EntityPageFacet(Web, Entities);

            // Initialize modules
            InitialData.AddTo(this);
        }

        public static SocialModules Instance = new SocialModules();
    }
}