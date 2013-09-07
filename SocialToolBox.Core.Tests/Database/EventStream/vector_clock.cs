using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Core.Tests.Database.EventStream
{
    [TestFixture]
    public class vector_clock
    {
        public VectorClock Clock;
        public IEventStream A;
        public IEventStream B;

        [SetUp]
        public void SetUp()
        {
            Clock = new VectorClock();
            var driver = new DatabaseDriver();
            A = driver.GetEventStream("A", true);
            B = driver.GetEventStream("B", true);
        }

        [Test]
        public void initial_is_empty()
        {
            Assert.AreEqual(0, Clock.GetNextInStream("A"));
            Assert.AreEqual(0, Clock.GetNextInStream(A));
        }

        [Test]
        public void advance_using_name()
        {
            Clock.Advance("A", 10);
            Assert.AreEqual(10, Clock.GetNextInStream("A"));
            Assert.AreEqual(10, Clock.GetNextInStream(A));
            Assert.AreEqual(0, Clock.GetNextInStream("B"));
            Assert.AreEqual(0, Clock.GetNextInStream(B));
        }

        [Test]
        public void advance_using_stream()
        {
            Clock.Advance(A, 10);
            Assert.AreEqual(10, Clock.GetNextInStream("A"));
            Assert.AreEqual(10, Clock.GetNextInStream(A));
            Assert.AreEqual(0, Clock.GetNextInStream("B"));
            Assert.AreEqual(0, Clock.GetNextInStream(B));
        }

        [Test]
        public void advance_using_event()
        {
            var e = new EventInStream<string>(A, "", 9);
            Clock.Advance(e);
            Assert.AreEqual(10, Clock.GetNextInStream("A"));
            Assert.AreEqual(10, Clock.GetNextInStream(A));
            Assert.AreEqual(0, Clock.GetNextInStream("B"));
            Assert.AreEqual(0, Clock.GetNextInStream(B));
        }

        [Test]
        public void equality()
        {
            var clock = new VectorClock();
            clock.Advance(A, 10);
            Clock.Advance(A, 10);
            Assert.AreEqual(clock, Clock);

            clock.Advance(B, 0);
            Assert.AreEqual(clock, Clock);

            clock.Advance(B, 1);
            Assert.AreNotEqual(clock, Clock);
        }

        [Test]
        public void clone()
        {
            var clock = Clock.Clone();
            Assert.AreEqual(clock, Clock);

            clock.Advance(B, 1);
            Assert.AreNotEqual(clock, Clock);
        }

        [Test]
        public void serialize()
        {
            Clock.Advance(A, 10);
            Clock.Advance(B, 5);
            Clock.Advance("X\nY\\Z", 12);
            Clock.Advance("X:Y:Z", 13337);

            Assert.AreEqual(Clock, VectorClock.Unserialize(Clock.ToString()));
        }

        [Test]
        public void serialization_format()
        {
            Clock.Advance(A, 10);
            Clock.Advance(B, 5);
            Clock.Advance("X\nY\\Z", 12);
            Clock.Advance("X:Y:Z", 13337);

            Assert.AreEqual("A:10\nB:5\nX\\nY\\\\Z:12\nX:Y:Z:13337\n", Clock.ToString());
        }
    }
}
