using System;
using System.Collections.Generic;
using System.Diagnostics;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// When mocking queries to endpoints in unit tests, builds queries
    /// from scratch.
    /// </summary>
    public class WebEndpointQuery
    {
        /// <summary>
        /// The function that will be called when the query is complete.
        /// </summary>
        private readonly Func<IWebRequest, WebResponse> _run;

        /// <summary>
        /// The request that will be passed to the query when it is complete.
        /// </summary>
        private readonly WebRequest _request;

        /// <summary>
        /// A currently empty query.
        /// </summary>
        public WebEndpointQuery(Func<IWebRequest, WebResponse> run)
        {
            _run = run;
            _request = new WebRequest();
        }

        /// <summary>
        /// Constructs a copy of the specified query, but with a new request.
        /// </summary>
        private WebEndpointQuery(WebEndpointQuery query, WebRequest request)
        {
            _run = query._run;
            _request = request;
        }

        /// <summary>
        /// Runs the query, returns the response.
        /// </summary>
        /// <returns></returns>
        public WebResponse Run()
        {
            return _run(_request);
        }

        /// <summary>
        /// This private class carries all the request configuration data.
        /// </summary>
        private class WebRequest : IWebRequest
        {
            /// <summary>
            /// Performs a deep copy of a string-string dictionary.
            /// </summary>
            private static Dictionary<string, string> Copy(Dictionary<string, string> copied)
            {
                var result = new Dictionary<string, string>();
                foreach (var kv in copied) result.Add(kv.Key, kv.Value);
                return result;
            }

            /// <summary>
            /// Creates an empty web request.
            /// </summary>
            public WebRequest() {}

            /// <summary>
            /// Creates an independent copy of a web request.
            /// </summary>
            public WebRequest(WebRequest other)
            {
                Verb = other.Verb;
                Domain = other.Domain;
                MatchedPath = other.MatchedPath;
                UnmatchedPath = other.UnmatchedPath;
                _cookieData = Copy(other._cookieData);
                _postData = Copy(other._postData);
                _getData = Copy(other._getData);
                Payload = other.Payload;
                IsSecure = other.IsSecure;
                Port = other.Port;
            }

            public HttpVerb Verb { get; private set; }
            public string Domain { get; private set; }
            public bool IsSecure { get; private set; }
            public int Port { get; private set; }
            public string Path { get { return MatchedPath + "/" + string.Join("/", UnmatchedPath); } }
            public string MatchedPath { get; private set; }
            public string[] UnmatchedPath { get; private set; }
            public IWebRequest UnmatchOne() { return null; }

            public string Cookie(string name)
            {
                string result;
                if (_cookieData.TryGetValue(name, out result)) return result;
                return null;
            }

            public string Post(string name)
            {
                string result;
                if (_postData.TryGetValue(name, out result)) return result;
                return null;
            }

            public string Get(string name)
            {
                string result;
                if (_getData.TryGetValue(name, out result)) return result;
                return null;
            }

            private readonly Dictionary<string,string> _cookieData = new Dictionary<string, string>();
            private readonly Dictionary<string,string> _postData = new Dictionary<string,string>();
            private readonly Dictionary<string, string> _getData = new Dictionary<string, string>();
            public string Payload { get; private set; }

            public IWebResponseVisitor ResponseSender { get { return null; } }            
        }
    }
}
