using System;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Response
{
    /// <summary>
    /// A helper base class for test fixtures that involve response visitors.
    /// </summary>
    public class ResponseVisitorFixture
    {
        public ResponseVisitorHelper Visitor { get { return new ResponseVisitorHelper(); } }

        private class RequestHandler : WebRequestHandler<string>
        {
            private readonly Func<WebRequestHandler<string>, WebResponse> _action;

            public RequestHandler(Func<WebRequestHandler<string>, WebResponse> action)
            {
                _action = action;
            }

            protected override string Parse()
            {
                return "";
            }

            public override WebUrl Serialize(string t, WebUrl baseUrl)
            {
                return null;
            }

            protected override WebResponse Process()
            {
                return _action(this);
            }
        }

        private class Request : IWebRequest
        {
            public HttpVerb Verb { get; private set; }
            public string Domain { get; private set; }
            public string Path { get; private set; }
            public string MatchedPath { get; private set; }
            public string[] UnmatchedPath { get; private set; }
            public IWebRequest UnmatchOne() { return this; }
            public string Cookie(string name) { return null; }
            public string Post(string name) { return null; }
            public string Get(string name) { return null; }
            public string Payload { get; private set; }
            public IWebResponseVisitor ResponseSender { get; set; }
        }

        public IWebRequest Req;

        public void WithVisitor(ResponseVisitorHelper visitor)
        {
            Req = new Request { ResponseSender = visitor };   
        }

        public void Do(Func<WebRequestHandler<string>, WebResponse> action)
        {
            var handler = new RequestHandler(action);
            using (var response = handler.Process(Req, null))
                response.Send();            
        }
    }
}
