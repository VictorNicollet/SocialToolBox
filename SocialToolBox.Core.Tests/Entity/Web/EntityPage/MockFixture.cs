using NUnit.Framework;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Entity.Web;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Entity;
using SocialToolBox.Core.Mocks.Present;
using SocialToolBox.Core.Present.RenderingStrategy;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Tests.Entity.Web.EntityPage
{
    /// <summary>
    /// Used by all entity page facet tests, spawns an entity module and fills
    /// it with mock data.
    /// </summary>
    public abstract class MockFixture
    {
        /// <summary>
        /// An entity module filled with mock data.
        /// </summary>
        public EntityModule Module;

        /// <summary>
        /// The entity page facet.
        /// </summary>
        public EntityPageFacet Facet;

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            
            Module = new EntityModule(driver);
            EntityModuleMock.Fill(Module);

            Module.Compile();
            driver.Projections.Run();

            var web = new WebDriver(driver, new NaiveRenderingStrategy<IWebRequest>(new NodeRenderer()));
            Facet = new EntityPageFacet(web, Module);
        }
    }
}
