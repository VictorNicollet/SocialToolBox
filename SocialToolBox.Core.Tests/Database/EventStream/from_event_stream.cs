using NUnit.Framework;
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

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            A = driver.GetEventStream("A", true);
            B = driver.GetEventStream("B", true);
            Clock = new VectorClock();
        }

        [Test]
        public void empty_does_nothing()
        {
            var iter = FromEventStream.EachOfType<string>(Clock, A, B);
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void one_stream()
        {
            var iter = FromEventStream.EachOfType<Event>(Clock, A);
            A.AddEvent(new Event("A1"));
            A.AddEvent(new Event("A2"));

            Assert.AreEqual("A1", iter.Next().ToString());
            Assert.AreEqual("A2", iter.Next().ToString());
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void one_stream_bad_type()
        {
            var iter = FromEventStream.EachOfType<MockAccount>(Clock, A);
            A.AddEvent(new Event("A1"));
            A.AddEvent(MockAccount.Bob);
            A.AddEvent(new Event("A2"));

            Assert.AreEqual(MockAccount.Bob, iter.Next());
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void two_streams()
        {
            var iter = FromEventStream.EachOfType<Event>(Clock, A, B);
            B.AddEvent(new Event("B1"));
            B.AddEvent(new Event("B2"));
            A.AddEvent(new Event("A1"));
            A.AddEvent(new Event("A2"));

            Assert.AreEqual("A1", iter.Next().ToString());
            Assert.AreEqual("A2", iter.Next().ToString()); 
            Assert.AreEqual("B1", iter.Next().ToString());
            Assert.AreEqual("B2", iter.Next().ToString());
            Assert.IsNull(iter.Next());
        }

        [Test]
        public void two_streams_reinsert()
        {
            var iter = FromEventStream.EachOfType<Event>(Clock, A, B);
            B.AddEvent(new Event("B1"));
            A.AddEvent(new Event("A1"));

            Assert.AreEqual("A1", iter.Next().ToString());
            Assert.AreEqual("B1", iter.Next().ToString());
            
            B.AddEvent(new Event("B2"));
            A.AddEvent(new Event("A2"));

            Assert.AreEqual("A2", iter.Next().ToString());
            Assert.AreEqual("B2", iter.Next().ToString());
            Assert.IsNull(iter.Next());
        }
    }
}
