using NUnit.Framework;
using SocialToolBox.Core.Mocks.Web;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;

namespace SocialToolBox.Core.Tests.Web.Args
{
    [TestFixture]
    public class no_args
    {
        public NoArgs Args;
        public WebUrl Url;

        [SetUp]
        public void SetUp()
        {
            Args = new NoArgs();
        }

        [Test]
        public void appends_nothing()
        {
            var url = new WebUrl("localhost", new string[0], true);
            Args.AddTo(url);
            Assert.AreEqual("https://localhost", url.ToString());
        }

        [Test]
        public void parse_works_on_empty_request()
        {
            var req = WebRequest.Get(new WebUrl("localhost", new[] { "hello" }, true));
            Assert.IsTrue(Args.TryParse(req));
        }

        [Test]
        public void parse_fails_on_non_empty_request()
        {
            var req = WebRequest.Get(new WebUrl("localhost", new[] { "hello" }, true)).UnmatchOne();
            Assert.IsFalse(Args.TryParse(req));
        }
    }
}
