﻿using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Database.EventStream
{
    [TestFixture]
    public class from_event_stream
    {
        [Persist("SocialToolBox.Core.Tests.Database.EventStream.from_event_stream.Event")]
        public class Event
        {
            [PersistMember(0)] 
            public string Value;

            public Event() {}

            public Event(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        public IEventStream A;
        public IEventStream B;
        public VectorClock Clock;
        public IProjectTransaction PTransaction;
        public ITransaction Transaction;

        private string Next<T>(IMultiStreamIterator<T> iter) where T : class
        {
            return iter.Next().Event.ToString();
        }

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            A = driver.GetEventStream("A", true);
            B = driver.GetEventStream("B", true);
            Clock = new VectorClock();
            PTransaction = driver.StartProjectorTransaction();
            Transaction = driver.StartReadWriteTransaction();
        }

        [Test]
        public void empty_does_nothing()
        {
            var iter = FromEventStream.EachOfType<string>(Clock, PTransaction, A, B);
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void one_stream()
        {
            var iter = FromEventStream.EachOfType<Event>(Clock, PTransaction, A);
            A.AddEvent(new Event("A1"), Transaction);
            A.AddEvent(new Event("A2"), Transaction);

            Assert.AreEqual("A1", Next(iter));
            Assert.AreEqual("A2", Next(iter));
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void one_stream_bad_type()
        {
            var iter = FromEventStream.EachOfType<MockAccount>(Clock, PTransaction, A);
            A.AddEvent(new Event("A1"), Transaction);
            A.AddEvent(MockAccount.Bob, Transaction);
            A.AddEvent(new Event("A2"), Transaction);

            Assert.AreEqual(MockAccount.Bob, iter.Next().Event);
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void two_streams()
        {
            var iter = FromEventStream.EachOfType<Event>(Clock, PTransaction, A, B);
            B.AddEvent(new Event("B1"), Transaction);
            B.AddEvent(new Event("B2"), Transaction);
            A.AddEvent(new Event("A1"), Transaction);
            A.AddEvent(new Event("A2"), Transaction);

            Assert.AreEqual("A1", Next(iter));
            Assert.AreEqual("A2", Next(iter)); 
            Assert.AreEqual("B1", Next(iter));
            Assert.AreEqual("B2", Next(iter));
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void two_streams_reinsert()
        {
            var iter = FromEventStream.EachOfType<Event>(Clock, PTransaction, A, B);
            B.AddEvent(new Event("B1"), Transaction);
            A.AddEvent(new Event("A1"), Transaction);

            Assert.AreEqual("A1", Next(iter));
            Assert.AreEqual("B1", Next(iter));
            
            B.AddEvent(new Event("B2"), Transaction);
            A.AddEvent(new Event("A2"), Transaction);

            Assert.AreEqual("A2", Next(iter));
            Assert.AreEqual("B2", Next(iter));
            Assert.IsNull(iter.Next());
        }
    }
}
