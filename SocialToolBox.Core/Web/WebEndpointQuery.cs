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
        public WebEndpointQuery(IWebDriver driver, Func<IWebRequest, WebResponse> run)
        {
            _run = run;
            _request = new WebRequest(driver);
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
            public WebRequest(IWebDriver driver)
            {
                Driver = driver;
            }

            /// <summary>
            /// Creates an independent copy of a web request.
            /// </summary>
            public WebRequest(WebRequest other)
            {
                Verb = other.Verb;
                Domain = other.Domain;
                MatchedPath = other.MatchedPath;
                UnmatchedPath = other.UnmatchedPath;
                CookieData = Copy(other.CookieData);
                PostData = Copy(other.PostData);
                GetData = Copy(other.GetData);
                Payload = other.Payload;
                Driver = other.Driver;
            }

            public IWebDriver Driver { get; set; }
            public HttpVerb Verb { get; set; }
            public string Domain { get; set; }
            public string Path { get { return MatchedPath + "/" + string.Join("/", UnmatchedPath); } }
            public string MatchedPath { get; set; }
            public string[] UnmatchedPath { get; set; }
            public IWebRequest UnmatchOne() { return null; }

            public string Cookie(string name)
            {
                string result;
                if (CookieData.TryGetValue(name, out result)) return result;
                return null;
            }

            public string Post(string name)
            {
                string result;
                if (PostData.TryGetValue(name, out result)) return result;
                return null;
            }

            public string Get(string name)
            {
                string result;
                if (GetData.TryGetValue(name, out result)) return result;
                return null;
            }

            public readonly Dictionary<string,string> CookieData = new Dictionary<string, string>();
            public readonly Dictionary<string,string> PostData = new Dictionary<string,string>();
            public readonly Dictionary<string, string> GetData = new Dictionary<string, string>();
            public string Payload { get; set; }

            public IWebResponseVisitor ResponseSender { get { return null; } }
            public void SetDriver(IWebDriver webDriver)
            {
                Debug.Assert(false, "This request should never be passed to a driver.");
            }
        }
    }
}
