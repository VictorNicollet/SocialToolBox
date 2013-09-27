using System;
using System.Net;
using System.Web.UI.WebControls;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// Builds an endpoint on a driver. The point of this class is to avoid
    /// having to specify the type of the handler.
    /// </summary>
    public class WebEndpointBuilder<TArgs> where TArgs : class, IWebUrlArgument, new()
    {
        /// <summary>
        /// The driver on which this builder will register the handler.
        /// </summary>
        public readonly IWebDriver Driver;

        /// <summary>
        /// The verb for which the handler will be registered.
        /// </summary>
        public readonly HttpVerb Verb;

        /// <summary>
        /// The url for which the handler will be registered.
        /// </summary>
        public readonly string Url;

        public WebEndpointBuilder(IWebDriver driver, HttpVerb verb, string url)
        {
            Driver = driver;
            Verb = verb;
            Url = url;
        }

        /// <summary>
        /// Registers the provided handler.
        /// </summary>
        public WebEndpoint<TArgs, THandler> Use<THandler>(Func<THandler> handler)
            where THandler : WebRequestHandler<TArgs>
        {
            return Driver.Register<TArgs, THandler>(Verb, Url, handler);
        }
    }
}
