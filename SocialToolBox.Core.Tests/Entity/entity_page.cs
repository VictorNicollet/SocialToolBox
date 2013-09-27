using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
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
        public ITransaction Transaction;

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

            Transaction = driver.StartReadWriteTransaction();
        }

        [Test]
        public void initially_empty()
        {
            Assert.IsNull(Module.Pages.Get(IdA).Result);
        }

        [Test]
        public void create_provides_title()
        {           
            Stream.AddEvent(new MockAccountCreated(IdA, "The Title", DateTime.Parse("2011/05/14")), Transaction);
            Projections.Run();

            Assert.AreEqual(typeof(MockAccountAsEntityPage), Module.Pages.Get(IdA).Result.GetType());
            Assert.AreEqual("The Title", Module.Pages.Get(IdA).Result.Title);
        }

        [Test]
        public void update_changes_title()
        {
            Stream.AddEvent(new MockAccountCreated(IdA, "Old Title", DateTime.Parse("2011/05/14")), Transaction);
            Stream.AddEvent(new MockAccountNameUpdated(IdA, DateTime.Parse("2011/05/14"), "New Title"), Transaction);
            Projections.Run();

            Assert.AreEqual("New Title", Module.Pages.Get(IdA).Result.Title);
        }

        [Test]
        public void delete_removes_page()
        {
            Stream.AddEvent(new MockAccountCreated(IdA, "Old Title", DateTime.Parse("2011/05/14")), Transaction);
            Stream.AddEvent(new MockAccountDeleted(IdA, DateTime.Parse("2011/05/14")), Transaction);
            Projections.Run();

            Assert.IsNull(Module.Pages.Get(IdA).Result);
        }

        [Test]
        public void missing_without_create()
        {
            Stream.AddEvent(new MockAccountNameUpdated(IdA, DateTime.Parse("2011/05/14"), "New Title"), Transaction);
            Projections.Run();

            Assert.IsNull(Module.Pages.Get(IdA).Result);
        }
    }
}
