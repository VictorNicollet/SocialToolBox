using System.Linq;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web.Dispatch;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// Implementation of <see cref="IWebDriver"/>.
    /// </summary>
    public class WebDriver : IWebDriver
    {
        /// <summary>
        /// The dispatcher used for incoming requests.
        /// </summary>
        private readonly WebRequestDispatcher _dispatcher = 
            new WebRequestDispatcher();

        /// <summary>
        /// The domain for the generated URLs.
        /// </summary>
        public readonly string Domain;

        /// <summary>
        /// Should generated URLs be secured ? 
        /// </summary>
        public readonly bool IsSecure;

        /// <summary>
        /// The port for the generated URLs.
        /// </summary>
        public readonly int Port;

        public WebDriver(IRenderingStrategy<IWebRequest> rendering, 
            string domain = "localhost", bool isSecure = true, int port = 443)
        {
            Domain = domain;
            IsSecure = isSecure;
            Port = port;
            Rendering = rendering;
        }

        public WebEndpoint<TArgs, THandler> Register<TArgs, THandler>(HttpVerb verb, string url, THandler handler) 
            where TArgs : class, IWebUrlArgument, new() 
            where THandler : class, IWebRequestHandler<TArgs>
        {
            var segs = url.Split('/').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            _dispatcher.Register(string.Join("/", segs), verb, handler);
            return new WebEndpoint<TArgs, THandler>(this, handler, verb, Domain, segs, IsSecure, Port);
        }

        public IRenderingStrategy<IWebRequest> Rendering { get; private set; }

        public WebResponse Dispatch(IWebRequest request)
        {
            request.SetDriver(this);
            return _dispatcher.Dispatch(request);
        }
    }
}
