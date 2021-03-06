﻿using SocialToolBox.Cms.Page;
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
    public sealed class SocialModules
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
        /// CMS Pages.
        /// </summary>
        public readonly PageModule Pages;

        /// <summary>
        /// Registered web endpoints for entity pages.
        /// </summary>
        public readonly EntityPageFacet EntityPages;

        private SocialModules()
        {
            Database = new DatabaseDriver();

            var renderingStrategy = new NaiveRenderingStrategy<IWebRequest>(new PageNodeRenderer());
            Web = new WebDriver(Database, renderingStrategy);

            // Instantiate all modules
            Contacts = new ContactModule(Database);
            Entities = new EntityModule(Database);
            Pages = new PageModule(Database);

            // Set up bridges between modules
            Contacts.RegisterContactsAsEntities(Entities);
            Pages.RegisterPagesAsEntities(Entities);

            // Compile all modules, start background thread
            Contacts.Compile();
            Entities.Compile();
            Pages.Compile();

            Database.Projections.StartBackgroundThread();

            // Install facets
            EntityPages = new EntityPageFacet(Web, Entities);

            // Initialize modules
            InitialData.AddTo(this);
        }

        public static SocialModules Instance = new SocialModules();
    }
}