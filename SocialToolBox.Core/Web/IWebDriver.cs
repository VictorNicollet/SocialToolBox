using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// Web drivers describe how request handlers can be registered with
    /// the web server.
    /// </summary>
    public interface IWebDriver
    {
        /// <summary>
        /// Registers an endpoint accepting parameters of the specified type.
        /// </summary>
        WebEndpoint<TArgs, THandler> Register<TArgs, THandler>(HttpVerb verb, string url, Func<THandler> handler)
            where TArgs : class, IWebUrlArgument, new()
            where THandler : WebRequestHandler<TArgs>;

        /// <summary>
        /// The rendering strategy, dependent on the web request (for instance, for 
        /// detecting whether client is mobile or desktop).
        /// </summary>
        IRenderingStrategy<IWebRequest> Rendering { get; }

        /// <summary>
        /// The database driver used to respond to requests handled by 
        /// this driver.
        /// </summary>
        IDatabaseDriver Database { get; }
            
        /// <summary>
        /// Dispatch and handle a web request. Since the web request should carry
        /// the driver which is dispatching it, this expects a request-building
        /// function instead of a request.
        /// </summary>
        WebResponse Dispatch(IWebRequest request);
    }
}
