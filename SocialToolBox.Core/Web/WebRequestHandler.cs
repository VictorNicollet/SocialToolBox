using System.IO;

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
        protected abstract IWebResponse Process();

        public IWebResponse Process(IWebRequest request, T args)
        {
            Request = request;
            Arguments = args;
            return Process();
        }

        /// <summary>
        /// A web response that redirects to the specified URL.
        /// </summary>
        protected IWebResponse Redirect(string url, int code = 303)
        {
            return Request.Redirect(url, code);
        }

        /// <summary>
        /// A web response that returns an ASCII JSON payload.
        /// </summary>
        protected IWebResponse Json(string json, int code = 200)
        {
            return Request.Json(json, code);
        }

        /// <summary>
        /// A web response that returns an UTF-8 HTML payload.
        /// </summary>
        protected IWebResponse Html(string html, int code = 200)
        {
            return Request.Html(html, code);
        }

        /// <summary>
        /// A web response with an arbitrary data payload. Will close the stream
        /// after sending.
        /// </summary>
        protected IWebResponse Data(Stream data, string mime, int code = 200)
        {
            return Request.Data(data, mime, code);
        }

        /// <summary>
        /// A web response with an arbitrary attached data payload. Will close
        /// the stream after sending.
        /// </summary>
        protected IWebResponse File(Stream data, string filename, string mime, int code = 200)
        {
            return Request.File(data, filename, mime, code);
        }
    }
}
