using System.Linq;
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

        public WebDriver(string domain = "localhost", bool isSecure = true, int port = 443)
        {
            Domain = domain;
            IsSecure = isSecure;
            Port = port;
        }

        public WebEndpoint<TArgs, THandler> Register<TArgs, THandler>(HttpVerb verb, string url, THandler handler) 
            where TArgs : class, IWebUrlArgument, new() 
            where THandler : class, IWebRequestHandler<TArgs>
        {
            var segs = url.Split('/').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            _dispatcher.Register(string.Join("/", segs), verb, handler);
            return new WebEndpoint<TArgs, THandler>(handler, Domain, segs, IsSecure, Port);
        }

        public WebResponse Dispatch(IWebRequest request)
        {
            return _dispatcher.Dispatch(request);
        }
    }
}
