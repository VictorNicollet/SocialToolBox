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
        WebEndpoint<TArgs, THandler> Register<TArgs, THandler>(HttpVerb verb, string url, THandler handler)
            where TArgs : class, IWebUrlArgument, new()
            where THandler : class, IWebRequestHandler<TArgs>;

        /// <summary>
        /// Dispatch and handle a web request.
        /// </summary>
        WebResponse Dispatch(IWebRequest request);
    }
}
