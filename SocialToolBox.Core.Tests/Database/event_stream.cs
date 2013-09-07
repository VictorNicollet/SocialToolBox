using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Core.Tests.Database
{
    [TestFixture]
    public class event_stream
    {
        public IEventStream EventStream;

        [SetUp]
        public void SetUp()
        {
            EventStream = new EventStream("TheCorrectName", new DatabaseDriver());
        }

        [Test]
        public void has_correct_name()
        {
            Assert.AreEqual("TheCorrectName", EventStream.Name);
        }

        [Test]
        public void initially_at_zero()
        {
            Assert.AreEqual(0, EventStream.NextPosition.Result);
        }
    }
}
