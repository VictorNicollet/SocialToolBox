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
        /// Serialize the parameter to an argument config object.
        /// </summary>
        WebUrl Serialize(T t, WebUrl baseUrl);

        /// <summary>
        /// Process a request, return the response to be sent back.
        /// </summary>
        IWebResponse Process(IWebRequest request, T args);
    }
}
