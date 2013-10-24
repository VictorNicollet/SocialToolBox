using System.Threading.Tasks;
using NUnit.Framework;
using SocialToolBox.Core.Mocks.Web;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;
using SocialToolBox.Core.Web.Dispatch;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Dispatch
{
    [TestFixture]
    public class web_request_dispatcher
    {
        public WebUrl A = new WebUrl("localhost", new[]{ "a" });

        public WebRequestDispatcher Dispatcher;
        public IWebDriver Driver;

        /// <summary>
        /// A request handler that simply echoes its payload as JSON.
        /// </summary>
        class JsonEcho<T> : WebRequestHandler<T> where T : class, IWebUrlArgument, new()
        {
            private readonly string _echo;

            public JsonEcho(string echo)
            {
                _echo = echo;
            }

            protected override Task<WebResponse> Process()
            {
                return Task.FromResult(Json(_echo));
            }
        }

        [SetUp]
        public void SetUp()
        {
            Dispatcher = new WebRequestDispatcher();
            Driver = new WebDriver(null, null);
        }

        [Test]
        public void empty_dispatcher()
        {
            var response = Dispatcher.Dispatch(WebRequest.Get(A)).Result;
            Assert.IsNull(response);
        }

        [Test]
        public void with_exact_path()
        {
            Dispatcher.Register(Driver, "a", HttpVerb.Get, () => new JsonEcho<NoArgs>("null"));
            var response = Dispatcher.Dispatch(WebRequest.Get(A)).Result;
            var asJson = response as WebResponseJson;

            Assert.IsNotNull(asJson);
            Assert.AreEqual("null",asJson.Json);
        }

        [Test]
        public void with_exact_path_and_verb()
        {
            Dispatcher.Register(Driver, "a", HttpVerb.Post, () => new JsonEcho<NoArgs>("{}"));
            Dispatcher.Register(Driver, "a", HttpVerb.Get, () => new JsonEcho<NoArgs>("null"));
            var response = Dispatcher.Dispatch(WebRequest.Get(A)).Result;
            var asJson = response as WebResponseJson;

            Assert.IsNotNull(asJson);
            Assert.AreEqual("null", asJson.Json);
        }

        [Test]
        public void with_inexact_path()
        {
            Dispatcher.Register(Driver, "a", HttpVerb.Post, () => new JsonEcho<NoArgs>("{}"));
            Dispatcher.Register(Driver, "", HttpVerb.Get, () => new JsonEcho<NoArgs>("[]"));
            Dispatcher.Register(Driver, "", HttpVerb.Get,() =>  new JsonEcho<AnyArgs>("null"));
            var response = Dispatcher.Dispatch(WebRequest.Get(A)).Result;
            var asJson = response as WebResponseJson;

            Assert.IsNotNull(asJson);
            Assert.AreEqual("null", asJson.Json);
        }
    }
}
