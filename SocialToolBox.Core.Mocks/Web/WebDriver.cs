using System.Linq;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Dispatch;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Mocks.Web
{
    public class WebDriver : IWebDriver
    {
        /// <summary>
        /// The dispatcher used for incoming requests.
        /// </summary>
        private readonly WebRequestDispatcher _dispatcher = 
            new WebRequestDispatcher();

        public WebEndpoint<TArgs, THandler> Register<TArgs, THandler>(HttpVerb verb, string url, THandler handler) 
            where TArgs : class, IWebUrlArgument 
            where THandler : class, IWebRequestHandler<TArgs>
        {
            var segs = url.Split('/').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            _dispatcher.Register(string.Join("/", segs), WebRequestHandlerWrapper.Wrap(verb, handler));
            return new WebEndpoint<TArgs, THandler>(handler, "localhost", segs, true, 443);
        }

        /// <summary>
        /// Dispatches a web request, returns the corresponding response.
        /// </summary>
        public WebResponse Dispatch(IWebRequest request)
        {
            return _dispatcher.Dispatch(request);
        }
    }
}
