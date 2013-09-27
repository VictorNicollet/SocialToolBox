using System;
using System.Web;

namespace SocialToolBox.Core.Web.IIS
{
    /// <summary>
    /// Serves as the base class for an HTTP handler. Performs
    /// all the dispatching automatically, using a dispatcher
    /// obtained on the first request from the application.
    /// </summary>
    public class HttpHandler : IHttpHandler
    {
        /// <summary>
        /// The dispatcher used by this HTTP handler to dispatch 
        /// requests.
        /// </summary>
        private IWebDriver _dispatcher;

        public void ProcessRequest(HttpContext context)
        {
            if (_dispatcher == null)
            {
                var appWithDispatcher = context.ApplicationInstance as IApplicationWithDispatcher;
                if (appWithDispatcher == null)
                    throw new MissingMemberException("Application does not implement IApplicationWithDispatcher");
                _dispatcher = appWithDispatcher.Dispatcher;
            }

            var response = _dispatcher.Dispatch(driver => new WebRequest(driver,context));
            if (response != null) response.Send();
        }

        public bool IsReusable { get { return true; } }
    }
}
