using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    public interface IWebRequestHandler<T> where T : class
    {
        /// <summary>
        /// Attempts to extract the parameter from the request.
        /// Return <code>null</code> on failure.
        /// </summary>
        T Parse(IWebRequest request);

        /// <summary>
        /// Process a request, return the response to be sent back.
        /// </summary>
        WebResponse Process(IWebRequest request, T args);
    }
}
