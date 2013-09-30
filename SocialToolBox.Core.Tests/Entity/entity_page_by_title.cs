using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Entity
{
    [TestFixture]
    public class entity_page_by_title
    {
        public EntityModule Module;
        public IEventStream Stream;
        public ProjectionEngine Projections;
        public ICursor Cursor;

        public readonly Id IdA = Id.Parse("aaaaaaaaaaa");

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Stream = driver.GetEventStream("MockAccountEvents", true);
            Projections = driver.Projections;

            Module = new EntityModule(driver);
            Module.AddEventStream(Stream);
            MockAccountAsEntityPage.ExtendVisitor(Module.PageEventVisitor);
            Module.Compile();

            Cursor = driver.OpenReadWriteCursor();
        }

        [Test]
        public void initially_empty()
        {
            Assert.AreEqual(0, Module.PageByTitle.Count(Cursor).Result);
        }
    }
}
