using NUnit.Framework;
using SocialToolBox.Core.Mocks.Web;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;

namespace SocialToolBox.Core.Tests.Web.Args
{
    [TestFixture]
    public class any_args
    {
        public AnyArgs Args;

        [SetUp]
        public void SetUp()
        {
            Args = new AnyArgs();
        }

        [Test]
        public void appends_segments()
        {
            var url = new WebUrl("localhost", new[] {"test"}, true);
            var args = new AnyArgs("foo", "bar");
            args.AddTo(url);
            Assert.AreEqual("https://localhost/test/foo/bar", url.ToString());
        }

        [Test]
        public void parses_segments()
        {
            var url = new WebUrl("localhost", new[] { "test", "foo", "bar" }, true);
            var req = WebRequest.Get(url).UnmatchOne().UnmatchOne();
            Assert.IsTrue(Args.TryParse(req));
            CollectionAssert.AreEqual(new[] { "foo", "bar"}, Args.Values);
        }
    }
}
