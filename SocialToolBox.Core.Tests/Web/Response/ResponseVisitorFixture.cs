﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Present;
using SocialToolBox.Core.Present.RenderingStrategy;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Response
{
    /// <summary>
    /// A helper base class for test fixtures that involve response visitors.
    /// </summary>
    public class ResponseVisitorFixture
    {
        public ResponseVisitorHelper Visitor { get { return new ResponseVisitorHelper(); } }

        private class RequestHandler : WebRequestHandler<NoArgs>
        {
            private readonly Func<WebRequestHandler<NoArgs>, WebResponse> _action;

            public RequestHandler(Func<WebRequestHandler<NoArgs>, WebResponse> action)
            {
                _action = action;
            }

            protected override Task<WebResponse> Process()
            {
                return Task.FromResult(_action(this));
            }
        }

        private class Request : IWebRequest
        {
            public HttpVerb Verb { get; private set; }
            public string Domain { get; private set; }
            public bool IsSecure { get; private set; }
            public int Port { get; private set; }
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

        public IWebDriver Driver;

        [SetUp]
        public void SetUp()
        {
            Driver = new WebDriver(
                new DatabaseDriver(), 
                new NaiveRenderingStrategy<IWebRequest>(new NodeRenderer()));
        }

        public IWebRequest Req;

        public void WithVisitor(ResponseVisitorHelper visitor)
        {
            Req = new Request { ResponseSender = visitor };   
        }

        public void Do(Func<WebRequestHandler<NoArgs>, WebResponse> action)
        {
            var handler = new RequestHandler(action);
            using (var response = handler.Process(Driver, Req, null).Result)
                response.Send();            
        }
    }
}
