using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Projections;

namespace SocialToolBox.Core.Tests.Database.Projection
{
    [TestFixture]
    public class in_memory_index
    {
        public readonly Id IdA = Id.Parse("aaaaaaaaaaa");
        public readonly Id IdB = Id.Parse("bbbbbbbbbbb");
        public InMemoryIndex<StringKey, StringKey> Index;
        public IProjectCursor Cursor;

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Index = new InMemoryIndex<StringKey, StringKey>();
            Cursor = driver.OpenProjectionCursor();
        }

        public StringKey K(string k) { return new StringKey(k); }

        [Test]
        public void default_is_empty()
        {
            Assert.AreEqual(0, Index.Count(K("A"), Cursor).Result);
        }

        [Test]
        public void count_one_is_correct()
        {
            Index.Add(IdA, K("A"), K("B"), Cursor).Wait();
            Assert.AreEqual(1, Index.Count(K("A"), Cursor).Result);
            Assert.AreEqual(0, Index.Count(K("B"), Cursor).Result);
        }

        [Test]
        public void count_two_is_correct()
        {
            Index.Add(IdA, K("A"), K("B"), Cursor).Wait();
            Index.Add(IdB, K("A"), K("B"), Cursor).Wait();
            Assert.AreEqual(2, Index.Count(K("A"), Cursor).Result);
            Assert.AreEqual(0, Index.Count(K("B"), Cursor).Result);
        }
    }
}
