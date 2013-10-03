using NUnit.Framework;
using SocialToolBox.Core.Mocks.Web;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;

namespace SocialToolBox.Core.Tests.Web.Args
{
    [TestFixture]
    public class page_args
    {
        public PageArgs Args;

        [SetUp]
        public void SetUp()
        {
            Args = new PageArgs();
        }

        [Test]
        public void default_is_zero()
        {
            Assert.AreEqual(0, Args.Page);
        }

        [Test]
        public void constructor()
        {
            var args = new PageArgs(5);
            Assert.AreEqual(5, args.Page);
        }

        [Test]
        public void zero_is_empty()
        {
            var url = new WebUrl("localhost", new[] {"all"}, true);
            Args.AddTo(url);
            Assert.AreEqual("https://localhost/all", url.ToString());
        }

        [Test]
        public void non_zero_is_output()
        {
            var args = new PageArgs(5);
            var url = new WebUrl("localhost", new[] { "all" }, true);
            args.AddTo(url);
            Assert.AreEqual("https://localhost/all/6", url.ToString());
        }

        [Test]
        public void empty_is_zero()
        {
            var url = new WebUrl("localhost", new[] { "all" }, true);
            var req = WebRequest.Get(url);
            Assert.IsTrue(Args.TryParse(req));
            Assert.AreEqual(0, Args.Page);
        }

        [Test]
        public void one_is_zero()
        {
            var url = new WebUrl("localhost", new[] { "all", "1" }, true);
            var req = WebRequest.Get(url).UnmatchOne();
            Assert.IsTrue(Args.TryParse(req));
            Assert.AreEqual(0, Args.Page);
        }

        [Test]
        public void positive_is_parsed()
        {
            var url = new WebUrl("localhost", new[] { "all", "6" }, true);
            var req = WebRequest.Get(url).UnmatchOne();
            Assert.IsTrue(Args.TryParse(req));
            Assert.AreEqual(5, Args.Page);
        }

        [Test]
        public void negative_fails()
        {
            var url = new WebUrl("localhost", new[] { "all", "-3" }, true);
            var req = WebRequest.Get(url).UnmatchOne();
            Assert.IsFalse(Args.TryParse(req));
        }

        [Test]
        public void not_number_fails()
        {
            var url = new WebUrl("localhost", new[] { "all" }, true);
            var req = WebRequest.Get(url).UnmatchOne();
            Assert.IsFalse(Args.TryParse(req));
        }
    }
}
