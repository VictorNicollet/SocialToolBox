using NUnit.Framework;
using SocialToolBox.Core.Mocks.Web;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Dispatch;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Dispatch
{
    [TestFixture]
    public class web_request_dispatcher
    {
        public WebUrl A = new WebUrl("localhost", new[]{ "a" });

        public WebRequestDispatcher Dispatcher;

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

            protected override WebResponse Process()
            {
                return Json(_echo);
            }
        }

        [SetUp]
        public void SetUp()
        {
            Dispatcher = new WebRequestDispatcher();
        }

        [Test]
        public void empty_dispatcher()
        {
            var response = Dispatcher.Dispatch(WebRequest.Get(A));
            Assert.IsNull(response);
        }

        [Test]
        public void with_exact_path()
        {
            Dispatcher.Register("a", HttpVerb.Get, new JsonEcho<NoArgs>("null"));
            var response = Dispatcher.Dispatch(WebRequest.Get(A));
            var asJson = response as WebResponseJson;

            Assert.IsNotNull(asJson);
            Assert.AreEqual("null",asJson.Json);
        }

        [Test]
        public void with_exact_path_and_verb()
        {
            Dispatcher.Register("a", HttpVerb.Post, new JsonEcho<NoArgs>("{}"));
            Dispatcher.Register("a", HttpVerb.Get, new JsonEcho<NoArgs>("null"));
            var response = Dispatcher.Dispatch(WebRequest.Get(A));
            var asJson = response as WebResponseJson;

            Assert.IsNotNull(asJson);
            Assert.AreEqual("null", asJson.Json);
        }

        [Test]
        public void with_inexact_path()
        {
            Dispatcher.Register("a", HttpVerb.Post, new JsonEcho<NoArgs>("{}"));
            Dispatcher.Register("", HttpVerb.Get, new JsonEcho<AnyArgs>("null"));
            var response = Dispatcher.Dispatch(WebRequest.Get(A));
            var asJson = response as WebResponseJson;

            Assert.IsNotNull(asJson);
            Assert.AreEqual("null", asJson.Json);
        }
    }
}
