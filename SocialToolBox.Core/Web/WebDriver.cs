using System;
using System.Linq;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
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

        public WebDriver(IDatabaseDriver db, IRenderingStrategy<IWebRequest> rendering, 
            string domain = "localhost", bool isSecure = true, int port = 443)
        {
            Domain = domain;
            IsSecure = isSecure;
            Port = port;
            Rendering = rendering;
            Database = db;
        }

        public WebEndpoint<TArgs, THandler> Register<TArgs, THandler>(HttpVerb verb, string url, Func<THandler> handler) 
            where TArgs : class, IWebUrlArgument, new() 
            where THandler : WebRequestHandler<TArgs>
        {
            var segs = url.Split('/').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            _dispatcher.Register(this, string.Join("/", segs), verb, handler);
            return new WebEndpoint<TArgs, THandler>(this, handler, verb, Domain, segs, IsSecure, Port);
        }

        public IRenderingStrategy<IWebRequest> Rendering { get; private set; }

        public IDatabaseDriver Database { get; private set; }

        public Task<WebResponse> Dispatch(IWebRequest request)
        {
            return _dispatcher.Dispatch(request);
        }
    }
}
