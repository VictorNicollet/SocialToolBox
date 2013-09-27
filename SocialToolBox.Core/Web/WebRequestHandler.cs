using System.IO;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// All request handlers must extend this class.
    /// </summary>
    public abstract class WebRequestHandler<T> where T : class
    {
        /// <summary>
        /// The web request being handled right now.
        /// </summary>
        public IWebRequest Request { get; private set; }

        /// <summary>
        /// The web driver to be used to handle this request.
        /// </summary>
        public IWebDriver Web { get; private set; }

        /// <summary>
        /// The database driver to be used to handle this request.
        /// </summary>
        public IDatabaseDriver Database { get { return Web.Database; } }

        /// <summary>
        /// The arguments from the current web request.
        /// </summary>
        public T Arguments { get; private set; }

        /// <summary>
        /// Process the current response and arguments.
        /// </summary>
        protected abstract WebResponse Process();

        /// <summary>
        /// Processes the provided request. Not re-entrant.
        /// </summary>
        public WebResponse Process(IWebDriver webDriver, IWebRequest request, T args)
        {
            Request = request;
            Web = webDriver;
            Arguments = args;
            return Process();
        }

        /// <summary>
        /// A web response that redirects to the specified URL.
        /// </summary>
        public WebResponse Redirect(string url, int code = 303)
        {
            return new WebResponseRedirect(url, code, Request.ResponseSender);
        }

        /// <summary>
        /// A web response that returns an ASCII JSON payload.
        /// </summary>
        public WebResponse Json(string json, int code = 200)
        {
            return new WebResponseJson(json, code, Request.ResponseSender);
        }

        /// <summary>
        /// A web response that returns an UTF-8 HTML payload.
        /// </summary>
        public WebResponse Html(string html, int code = 200)
        {
            return new WebResponseHtml(html, code, Request.ResponseSender);
        }

        /// <summary>
        /// A web response with an arbitrary data payload. Will close the stream
        /// after sending.
        /// </summary>
        public WebResponse Data(Stream data, string mime, int code = 200)
        {
            return new WebResponseData(data, null, mime, code, Request.ResponseSender);
        }

        /// <summary>
        /// A web response with an arbitrary attached data payload. Will close
        /// the stream after sending.
        /// </summary>
        public WebResponse File(Stream data, string filename, string mime, int code = 200)
        {
            return new WebResponseData(data, filename, mime, code, Request.ResponseSender);
        }

        /// <summary>
        /// A web response with a page and its associated renderer. If no renderer is 
        /// provided, uses the driver's rendering strategy for obtaining one.
        /// </summary>
        public WebResponse Page(IPage node, INodeRenderer renderer = null, int code = 200)
        {
            if (renderer == null) renderer = Web.Rendering.PickRenderer(Request);
            return new WebResponsePage(node, renderer, code, Request.ResponseSender);
        }
    }
}
