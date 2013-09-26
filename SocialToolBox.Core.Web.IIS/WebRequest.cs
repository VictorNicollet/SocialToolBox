using System.Linq;
using System.Text;
using System.Web;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web.IIS
{
    /// <summary>
    /// Adapter pattern : implements <see cref="IWebRequest"/> using 
    /// an IIS request object.
    /// </summary>
    public class WebRequest : IWebRequest
    {
        /// <summary>
        /// The underlying context received from IIS.
        /// </summary>
        public readonly HttpContext Context;

        /// <summary>
        /// All the path segments, excluding empty ones.
        /// </summary>
        private readonly string[] _pathSegments;

        /// <summary>
        /// How many segments in <see cref="_pathSegments"/> are counted
        /// as matched.
        /// </summary>
        private readonly int _matchedSegments;

        public WebRequest(HttpContext context)
        {
            Context = context;
            _pathSegments = context.Request.Path.Split('/').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            _matchedSegments = _pathSegments.Length;
        }

        /// <summary>
        /// Construct a sub-request, used when unmatching segments.
        /// </summary>
        private WebRequest(HttpContext context, string[] segments, int matched)
        {
            Context = context;
            _pathSegments = segments;
            _matchedSegments = matched;
        }

        public HttpVerb Verb
        {
            get
            {
                switch (Context.Request.HttpMethod)
                {
                    case "POST": return HttpVerb.Post;
                    case "DELETE": return HttpVerb.Delete;
                    case "PUT": return HttpVerb.Put;
                    default: return HttpVerb.Get;                        
                }
            }
        }

        public string Domain
        {
            get { return Context.Request.Url.Host; }
        }

        public string Path
        {
            get { return string.Join("/", _pathSegments); }
        }

        public string MatchedPath
        {
            get { return string.Join("/", _pathSegments.Take(_matchedSegments)); }
        }

        public string[] UnmatchedPath
        {
            get { return _pathSegments.Skip(_matchedSegments).ToArray(); }
        }

        public IWebRequest UnmatchOne()
        {
            if (_matchedSegments == 0) return null;
            return new WebRequest(Context, _pathSegments, _matchedSegments - 1);
        }

        public string Cookie(string name)
        {
            var cookie = Context.Request.Cookies[name];
            if (cookie == null) return null;
            return cookie.Value;
        }

        public string Post(string name)
        {
            return Context.Request.Form[name];
        }

        public string Get(string name)
        {
            return Context.Request.Params[name];
        }

        public string Payload
        {
            get
            {
                var length = (int)Context.Request.InputStream.Length;
                var bytes = new byte[length];
                Context.Request.InputStream.Read(bytes, 0, length);

                // TODO: detect encoding based on request
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public IWebResponseVisitor ResponseSender { get { return new WebResponseVisitor(Context); } }
    }
}
