using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Mocks.Web
{
    /// <summary>
    /// A mock web request, which can be created from scratch using helper functions.
    /// </summary>
    public class WebRequest : IWebRequest
    {
        public IWebDriver Driver { get; private set; }
        public HttpVerb Verb { get; set; }
        public string Domain { get; set; }
        public bool IsSecure { get; set; }
        public int Port { get; set; }

        public string Path
        {
            get
            {
                return (MatchedPath == "") 
                    ? string.Join("/", UnmatchedPath) 
                    : string.Join("/", new[] {MatchedPath}.Concat(UnmatchedPath));
            }
        }

        public string MatchedPath { get; set; }
        public string[] UnmatchedPath { get; set; }

        private WebRequest(WebRequest request)
        {
            Verb = request.Verb;
            Cookies = request.Cookies;
            Payload = request.Payload;
            GetArgs = request.GetArgs;
            PostArgs = request.PostArgs;
            Domain = request.Domain;
        }

        private WebRequest(WebUrl url, HttpVerb verb)
        {
            Verb = verb;
            Cookies = new Dictionary<string, string>();
            GetArgs = new Dictionary<string, string>();
            PostArgs = new Dictionary<string, string>();
            Domain = url.Domain;
            UnmatchedPath = new string[0];
            MatchedPath = string.Join("/", url.Path);
            foreach (var kv in url.Get)
                GetArgs.Add(kv.Key,kv.Value);
        }

        /// <summary>
        /// Creates a GET request to the specified URL.
        /// </summary>
        public static WebRequest Get(WebUrl url)
        {
            return new WebRequest(url, HttpVerb.Get);
        }

        /// <summary>
        /// Creates a POST request to the specified URL.
        /// </summary>
        public static WebRequest Post(WebUrl url, Dictionary<string, string> post)
        {
            var request = new WebRequest(url, HttpVerb.Post);
            foreach (var kv in post) request.PostArgs.Add(kv.Key,kv.Value);
            return request;
        }

        /// <summary>
        /// Creates a POST request to the specified URL.
        /// </summary>
        public static WebRequest Post(WebUrl url, string payload)
        {
            return new WebRequest(url, HttpVerb.Post) {Payload = payload};
        }

        /// <summary>
        /// Creates a PUT request to the specified URL.
        /// </summary>
        public static WebRequest Put(WebUrl url, string payload)
        {
            return new WebRequest(url, HttpVerb.Put) {Payload = payload};
        }
        
        /// <summary>
        /// Create a DELETE request to the specified URL.
        /// </summary>
        public static WebRequest Delete(WebUrl url)
        {
            return new WebRequest(url, HttpVerb.Delete);
        }

        public IWebRequest UnmatchOne()
        {
            if (MatchedPath == "") return null;
            
            var last = MatchedPath.LastIndexOf('/');

            string newMatchedPath;
            string[] newUnmatchedPath;

            if (last == -1)
            {
                newMatchedPath = "";
                newUnmatchedPath = new[] {MatchedPath}.Concat(UnmatchedPath).ToArray();
            }
            else
            {
                newMatchedPath = MatchedPath.Substring(0, last);
                newUnmatchedPath = new[] {MatchedPath.Substring(last + 1)}.Concat(UnmatchedPath).ToArray();
            }

            return new WebRequest(this)
            {
                MatchedPath = newMatchedPath,
                UnmatchedPath = newUnmatchedPath
            };
        }

        /// <summary>
        /// All the cookies.
        /// </summary>
        public readonly Dictionary<string, string> Cookies;

        public string Cookie(string name)
        {
            string cookie;
            Cookies.TryGetValue(name, out cookie);
            return cookie;
        }

        /// <summary>
        /// All the POST arguments.
        /// </summary>
        public readonly Dictionary<string, string> PostArgs; 

        public string Post(string name)
        {
            string post;
            PostArgs.TryGetValue(name, out post);
            return post;
        }

        /// <summary>
        /// All the GET arguments.
        /// </summary>
        public readonly Dictionary<string, string> GetArgs;

        public string Get(string name)
        {
            string get;
            GetArgs.TryGetValue(name, out get);
            return get;
        }

        public string Payload { get; set; }

        public IWebResponseVisitor ResponseSender { get { return new NullResponseSender(); } }

        public void SetDriver(IWebDriver webDriver)
        {
            Driver = webDriver;
        }

        /// <summary>
        /// A web response visitor that does nothing with the response.
        /// </summary>
        private class NullResponseSender : IWebResponseVisitor
        {
            // ReSharper disable CSharpWarnings::CS1998
            public async Task Visit(WebResponseRedirect redirect) {}
            public async Task Visit(WebResponseJson json) { }
            public async Task Visit(WebResponseHtml html) { }
            public async Task Visit(WebResponseData data) { }
            public async Task Visit(WebResponsePage page) { }
            // ReSharper restore CSharpWarnings::CS1998
        }
    }
}
