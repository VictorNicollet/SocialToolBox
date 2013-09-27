using System;
using System.Runtime.Remoting.Services;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Database
{
    [TestFixture]
    public class event_stream
    {
        [Serializable]
        private class IncorrectSerializable
        {
        }

        public IEventStream EventStream;
        public ICursor Cursor;
        public IProjectCursor PCursor;

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            EventStream = driver.GetEventStream("TheCorrectName", true);
            Cursor = driver.OpenReadWriteCursor();
            PCursor = driver.OpenProjectionCursor();
        }

        [Test]
        public void has_correct_name()
        {
            Assert.AreEqual("TheCorrectName", EventStream.Name);
        }

        [Test]
        public void initially_at_zero()
        {
            Assert.AreEqual(0, EventStream.NextPosition(Cursor).Result);
        }

        [Test]
        public void initially_contains_nothing()
        {
            Assert.IsNull(EventStream.GetEvent<MockAccount>(0, PCursor).Result);
            Assert.IsEmpty(EventStream.GetEvents<MockAccount>(0, 10, PCursor).Result);
        }

        public void after_one_event()
        {
            EventStream.AddEvent(MockAccount.Bob, Cursor).RunSynchronously();
        }

        [Test]
        public void after_one_event_at_one()
        {
            after_one_event();
            Assert.AreEqual(1, EventStream.NextPosition(Cursor).Result);
        }

        [Test]
        public void after_one_event_contains_bob()
        {
            after_one_event();
            var eventInStream = EventStream.GetEvent<MockAccount>(0, PCursor).Result;
            Assert.AreEqual(0, eventInStream.Position);
            Assert.AreEqual(MockAccount.Bob, eventInStream.Event);
        }

        [Test]
        public void after_one_event_query_bob()
        {
            after_one_event();
            var events = EventStream.GetEvents<MockAccount>(0, 10, PCursor).Result;
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual(MockAccount.Bob, events[0].Event);
            Assert.AreEqual(0, events[0].Position);
        }

        [Test]
        public void after_one_event_query_nothing()
        {
            after_one_event();
            Assert.IsEmpty(EventStream.GetEvents<MockAccount>(1, 10, PCursor).Result);   
        }

        [Test]
        public void after_one_event_query_wrong_type_throws()
        {
            after_one_event();
            Assert.Throws<AggregateException>(() =>
                EventStream.GetEvent<IncorrectSerializable>(0, PCursor).Wait());
            Assert.Throws<AggregateException>(() =>
                EventStream.GetEvents<IncorrectSerializable>(0, 10, PCursor).Wait());
        }

        [Test]
        public void after_one_event_query_wrong_type_discards()
        {
            after_one_event();
            var list = EventStream.GetEventsOfType<IncorrectSerializable>(0, 10, PCursor).Result;
            Assert.IsEmpty(list);
            Assert.AreEqual(1, list.RealCount);
            Assert.AreEqual(1, list.NextPosition);
        }
    }
}
