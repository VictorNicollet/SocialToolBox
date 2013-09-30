using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Entity;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Events;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Entity
{
    [TestFixture]
    public class entity_page
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
            Assert.IsNull(Module.Pages.Get(IdA, Cursor).Result);
        }

        [Test]
        public void create_provides_title()
        {           
            Stream.AddEvent(new MockAccountCreated(IdA, "The Title", DateTime.Parse("2011/05/14")), Cursor);
            Projections.Run();

            Assert.AreEqual(typeof(MockAccountAsEntityPage), Module.Pages.Get(IdA, Cursor).Result.GetType());
            Assert.AreEqual("The Title", Module.Pages.Get(IdA, Cursor).Result.Title);
        }

        [Test]
        public void update_changes_title()
        {
            Stream.AddEvent(new MockAccountCreated(IdA, "Old Title", DateTime.Parse("2011/05/14")), Cursor);
            Stream.AddEvent(new MockAccountNameUpdated(IdA, DateTime.Parse("2011/05/14"), "New Title"), Cursor);
            Projections.Run();

            Assert.AreEqual("New Title", Module.Pages.Get(IdA, Cursor).Result.Title);
        }

        [Test]
        public void delete_removes_page()
        {
            Stream.AddEvent(new MockAccountCreated(IdA, "Old Title", DateTime.Parse("2011/05/14")), Cursor);
            Stream.AddEvent(new MockAccountDeleted(IdA, DateTime.Parse("2011/05/14")), Cursor);
            Projections.Run();

            Assert.IsNull(Module.Pages.Get(IdA, Cursor).Result);
        }

        [Test]
        public void missing_without_create()
        {
            Stream.AddEvent(new MockAccountNameUpdated(IdA, DateTime.Parse("2011/05/14"), "New Title"), Cursor);
            Projections.Run();

            Assert.IsNull(Module.Pages.Get(IdA, Cursor).Result);
        }
    }
}
