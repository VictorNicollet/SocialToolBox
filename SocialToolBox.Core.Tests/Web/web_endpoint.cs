using System.Threading.Tasks;
using NUnit.Framework;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web
{
    [TestFixture]
    public class web_endpoint
    {
        public class SimpleArgument : IWebUrlArgument
        {
            public string First;
            public string Second;

            public WebUrl AddTo(WebUrl url)
            {
                url.AddPathSegment(First);
                url.AddParameter("second", Second);
                return url;
            }

            public bool TryParse(IWebRequest request)
            {
                return true;
            }
        }

        public class SimpleHandler : WebRequestHandler<SimpleArgument>
        {
            protected override Task<WebResponse> Process() { return null; }
        }

        public IWebDriver Driver;
        public SimpleHandler Handler;
        public WebEndpoint<SimpleArgument, SimpleHandler> Endpoint;

        [SetUp]
        public void SetUp()
        {
            Driver = new WebDriver(null,null);
            Endpoint = new WebEndpoint<SimpleArgument, SimpleHandler>(
                Driver, () => Handler, HttpVerb.Get, "foobar.com", new[] { "a", "b" }, false, 80);
            // The above lambda always returns "Handler" for testing purposes,
            // this would actually break down and die in a real situation
        }

        [Test]
        public void has_handler()
        {
            Assert.AreSame(Handler, Endpoint.RequestHandler());
        }

        [Test]
        public void has_domain()
        {
            Assert.AreEqual("foobar.com", Endpoint.Domain);
        }

        [Test]
        public void has_path()
        {
            CollectionAssert.AreEqual(new[]{ "a", "b"}, Endpoint.BasePath);
        }

        [Test]
        public void has_secure()
        {
            Assert.IsFalse(Endpoint.IsSecure);
            Endpoint = new WebEndpoint<SimpleArgument, SimpleHandler>(
                Driver, () => new SimpleHandler(), HttpVerb.Get, "", new string[] { }, true, 443);
            Assert.IsTrue(Endpoint.IsSecure);
        }

        [Test]
        public void has_port()
        {
            Assert.AreEqual(80, Endpoint.Port);
            Endpoint = new WebEndpoint<SimpleArgument, SimpleHandler>(
                Driver, () => new SimpleHandler(), HttpVerb.Get, "", new string[] { }, true, 8080);
            Assert.AreEqual(8080, Endpoint.Port);
            Endpoint = new WebEndpoint<SimpleArgument, SimpleHandler>(
                Driver, () => new SimpleHandler(), HttpVerb.Get, "", new string[] { }, false, 8088);
            Assert.AreEqual(8088, Endpoint.Port);
        }

        [Test]
        public void generates_url()
        {
            var args = new SimpleArgument {First = "FIRST", Second = "SECOND"};
            var url = Endpoint.Url(args);
            var expect = new WebUrl("foobar.com", new [] { "a", "b", "FIRST" }, false, 80);
            expect.AddParameter("second","SECOND");
            Assert.AreEqual(expect, url);
        }
    }
}
