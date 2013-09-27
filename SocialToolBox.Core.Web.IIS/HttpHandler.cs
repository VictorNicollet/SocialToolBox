using System;
using System.Web;

namespace SocialToolBox.Core.Web.IIS
{
    /// <summary>
    /// Serves as the base class for an HTTP handler. Performs
    /// all the dispatching automatically, using a dispatcher
    /// obtained on the first request from the application.
    /// </summary>
    public class HttpHandler : IHttpAsyncHandler
    {
        /// <summary>
        /// The dispatcher used by this HTTP handler to dispatch 
        /// requests.
        /// </summary>
        private IWebDriver _dispatcher;

        public void ProcessRequest(HttpContext context)
        {
        }

        public bool IsReusable { get { return true; } }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var action = new AsyncHttpAction(_dispatcher, context, cb, extraData);
            action.Start();
            return action;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
        }
    }
}
