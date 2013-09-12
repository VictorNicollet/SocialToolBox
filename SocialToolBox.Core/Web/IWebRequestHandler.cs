using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    public interface IWebRequestHandler<in T> where T : class
    {
        /// <summary>
        /// Process a request, return the response to be sent back.
        /// </summary>
        WebResponse Process(IWebRequest request, T args);
    }
}
