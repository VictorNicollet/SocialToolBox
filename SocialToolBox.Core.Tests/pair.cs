using NUnit.Framework;

namespace SocialToolBox.Core.Tests
{
    [TestFixture]
    public sealed class pair
    {
        [Test]
        public void strings()
        {
            var p = Pair.Make("a", "b");
            Assert.AreEqual("a", p.First);
            Assert.AreEqual("b", p.Second);
        }

        [Test]
        public void equality()
        {
            var p1 = Pair.Make("a", "bb");
            var p2 = Pair.Make("a", "b" + "b");
            Assert.AreEqual(p1, p2);
        }
    }
}
