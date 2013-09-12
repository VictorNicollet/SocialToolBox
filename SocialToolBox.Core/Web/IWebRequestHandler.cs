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
        /// Process a request, return the response to be sent back.C:\Users\admin\Documents\GitHub\SocialToolBox\SocialToolBox.Core\Web\IWebRequestHandler.cs
        /// </summary>
        IWebResponse Process(IWebRequest request, T args);
    }
}
