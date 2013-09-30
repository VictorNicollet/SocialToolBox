using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Events;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Database.Projection.ProjectionExtensions
{
    [TestFixture]
    public class store_with_mutator_projection
    {
        public IEventStream Stream;
        public IStore<MockAccount> Accounts;
        public IProjection<IMockEvent> Projection;
        public ProjectionEngine Projections;
        public ICursor Cursor;

        public readonly Id IdA = Id.Parse("aaaaaaaaaaa");
        public readonly Id IdB = Id.Parse("bbbbbbbbbbb");

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Projection = driver.CreateProjection<IMockEvent>("Accounts");
            Stream = driver.GetEventStream("Accounts", true);
            Accounts = Projection.CreateStore("Store", MockAccount.ApplyEvent, new[] { Stream });
            Projections = driver.Projections;
            Cursor = driver.OpenReadWriteCursor();
        }

        [Test]
        public void create_before_compile()
        {
            Assert.IsNotNull(Projection.CreateStore("Store2", MockAccount.ApplyEvent, new[] { Stream }));
        }

        [Test]
        public void cannot_create_after_compile()
        {
            Projection.Compile();
            Assert.Throws<InvalidOperationException>(() =>
                Assert.IsNull(Projection.CreateStore("Store2", MockAccount.ApplyEvent, new[] { Stream })));
        }

        [Test]
        public void initially_empty()
        {
            Projection.Compile();
            Assert.IsNull(Accounts.Get(IdA, Cursor).Result);
        }

        [Test]
        public void after_creation()
        {
            Projection.Compile();
            Stream.AddEvent(new MockAccountCreated(IdA, "Name", DateTime.Parse("2013/07/12")), Cursor);
            Projections.Run();

            var current = Accounts.Get(IdA, Cursor).Result;
            Assert.AreEqual(new MockAccount { Name = "Name" }, current);
        }

        [Test]
        public void after_update()
        {
            Projection.Compile();
            Stream.AddEvent(new MockAccountCreated(IdA, "Bob", DateTime.Parse("2013/07/12")), Cursor);
            Stream.AddEvent(new MockAccountPasswordUpdated(IdA, DateTime.Parse("2013/07/12"), MockAccount.Bob.Password), Cursor);
            Projections.Run();

            var current = Accounts.Get(IdA, Cursor).Result;
            Assert.AreEqual(MockAccount.Bob, current);
        }

        [Test]
        public void after_update_delete()
        {
            Projection.Compile();
            Stream.AddEvent(new MockAccountCreated(IdA, "Bob", DateTime.Parse("2013/07/12")), Cursor);
            Stream.AddEvent(new MockAccountPasswordUpdated(IdA, DateTime.Parse("2013/07/12"), MockAccount.Bob.Password), Cursor);
            Stream.AddEvent(new MockAccountDeleted(IdA, DateTime.Parse("2013/07/12")), Cursor);
            Projections.Run();

            Assert.IsNull(Accounts.Get(IdA, Cursor).Result);
        }
    }
}
