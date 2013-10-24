using NUnit.Framework;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Tests.Present
{
    [TestFixture]
    public sealed class pagination
    {
        public WebUrl UrlA = new WebUrl("localhost", new []{"a"});
        public WebUrl UrlB = new WebUrl("localhost", new []{"b"});

        [Test]
        public void prevnext()
        {
            var p = Pagination.PrevNext(UrlA, UrlB);
            
            Assert.IsTrue(p.IsPrevNext);
            Assert.AreEqual(2, p.Links.Count);
            Assert.AreEqual(UrlA, p.Links[0].Url);
            Assert.AreEqual("«", p.Links[0].Label);
            Assert.IsFalse(p.Links[0].IsCurrent);            
            Assert.AreEqual(UrlB, p.Links[1].Url);
            Assert.AreEqual("»", p.Links[1].Label);
            Assert.IsFalse(p.Links[1].IsCurrent);
            Assert.AreEqual(Pagination.Position.Both, p.Where);
        }

        [Test]
        public void prevnext_no_prev()
        {
            var p = Pagination.PrevNext(null, UrlB);
            Assert.AreEqual(null, p.Links[0].Url);
        }

        [Test]
        public void prevnext_above()
        {
            var p = Pagination.PrevNext(UrlA, UrlB, Pagination.Position.Above);
            Assert.AreEqual(Pagination.Position.Above, p.Where);
        }
    }
}
