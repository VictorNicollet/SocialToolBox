using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// An endpoint builds URLs from parameters.
    /// </summary>
    public class WebEndpoint<TArgs,THandler> : IWebEndpoint<TArgs> 
        where TArgs : class, IWebUrlArgument
        where THandler : WebRequestHandler<TArgs>
    {
        /// <summary>
        /// The web driver to which this endpoint is bound.
        /// </summary>
        public readonly IWebDriver Driver;

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

        public readonly HttpVerb Verbs;

        /// <summary>
        /// The handler for requests on this endpoint.
        /// </summary>
        public readonly Func<THandler> RequestHandler; 

        public WebEndpoint(IWebDriver driver, Func<THandler> handler, HttpVerb verbs, string domain, IEnumerable<string> path, bool secure, int port)
        {
            Driver = driver;
            Domain = domain;
            BasePath = path.ToArray();
            IsSecure = secure;
            Port = port;
            RequestHandler = handler;
            Verbs = verbs;
        }

        public WebUrl Url(TArgs args)
        {
            return args.AddTo(new WebUrl(Domain, BasePath, IsSecure, Port));
        }

        public WebUrl Url(IWebRequest request, TArgs args)
        {
            return args.AddTo(new WebUrl(request.Domain, BasePath, request.IsSecure, request.Port));
        }

        /// <summary>
        /// Builds a query for unit testing the endpoint.
        /// </summary>
        public WebEndpointQuery Query(TArgs args)
        {
            return new WebEndpointQuery(req => RequestHandler().Process(Driver, req, args));
        }
    }
}
