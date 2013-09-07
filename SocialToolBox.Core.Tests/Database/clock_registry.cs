using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Core.Tests.Database
{
    [TestFixture]
    public class clock_registry
    {
        public IClockRegistry Registry;
        public IEventStream A;
        public IEventStream B;

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            A = driver.GetEventStream("A", true);
            B = driver.GetEventStream("B", true);
            Registry = driver.ClockRegistry;
        }

        [Test]
        public void empty_clock()
        {
            var clock = Registry.LoadProjection("1").Result;
            Assert.AreEqual(new VectorClock(), clock);
        }

        [Test]
        public void load_separate_mutable()
        {
            Registry.SaveProjection("1", new VectorClock()).Wait();

            var clock = Registry.LoadProjection("1").Result;
            clock.Advance(A, 10);

            var oldClock = Registry.LoadProjection("1").Result;

            Assert.AreNotEqual(clock, oldClock);
        }

        [Test]
        public void save_and_reload()
        {
            var clock = new VectorClock();
            clock.Advance(A, 10);

            Registry.SaveProjection("1", clock).Wait();

            var newClock = Registry.LoadProjection("1").Result;

            Assert.AreEqual(clock, newClock);
        }

        [Test]
        public void save_separate_mutable()
        {
            var clock = new VectorClock();
            clock.Advance(A, 10);

            Registry.SaveProjection("1", clock).Wait();
            clock.Advance(B, 10);

            var newClock = Registry.LoadProjection("1").Result;

            Assert.AreNotEqual(clock, newClock);
        }

        [Test]
        public void save_distinct_mutable()
        {
            var clock = new VectorClock();
            clock.Advance(A, 10);

            Registry.SaveProjection("1", clock).Wait();

            var newClock = Registry.LoadProjection("2").Result;

            Assert.AreNotEqual(clock, newClock);
        }
    }
}
