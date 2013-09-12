using System.Collections.Generic;
using System.Linq;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// An endpoint builds URLs from parameters.
    /// </summary>
    public class WebEndpoint<TArgs,THandler> : IWebEndpoint<TArgs> 
        where TArgs : class, IWebUrlArgument
        where THandler : class, IWebRequestHandler<TArgs>
    {
        /// <summary>
        /// The domain on which this endpoint runs.
        /// </summary>
        public readonly string Domain;

        /// <summary>
        /// Whether this endpoint is secure.
        /// </summary>
        public readonly bool IsSecure;

        /// <summary>
        /// The base path for this endpoint (to which more segments can be added).
        /// </summary>
        public readonly IReadOnlyCollection<string> BasePath;

        /// <summary>
        /// The port on which this endpoint runs.
        /// </summary>
        public readonly int Port;

        /// <summary>
        /// The handler for requests on this endpoint.
        /// </summary>
        public readonly THandler RequestHandler; 

        public WebEndpoint(THandler handler, string domain, IEnumerable<string> path, bool secure, int port)
        {
            Domain = domain;
            BasePath = path.ToArray();
            IsSecure = secure;
            Port = port;
            RequestHandler = handler;
        }

        public WebUrl Url(TArgs args)
        {
            return args.AddTo(new WebUrl(Domain, BasePath, IsSecure, Port));
        }
    }
}
