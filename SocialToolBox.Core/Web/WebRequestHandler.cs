using System.IO;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// Implementation of see <see cref="IWebRequestHandler{T}"/> that provides
    /// useful base functions.
    /// </summary>
    public abstract class WebRequestHandler<T> : IWebRequestHandler<T> where T : class
    {
        /// <summary>
        /// The web request being handled right now.
        /// </summary>
        public IWebRequest Request { get; private set; }

        /// <summary>
        /// The arguments from the current web request.
        /// </summary>
        public T Arguments { get; private set; }

        /// <summary>
        /// Parse the current request, return the arguments (or null if not
        /// handled).
        /// </summary>
        protected abstract T Parse();

        public T Parse(IWebRequest request)
        {
            Request = request;
            return Parse();
        }

        public abstract WebUrl Serialize(T t, WebUrl baseUrl);

        /// <summary>
        /// Process the current response and arguments.
        /// </summary>
        /// <returns></returns>
        protected abstract WebResponse Process();

        public WebResponse Process(IWebRequest request, T args)
        {
            Request = request;
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
    }
}
