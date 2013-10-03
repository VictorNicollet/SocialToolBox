namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// A generic web endpoint, generates URLs from arguments.
    /// </summary>
    public interface IWebEndpoint<in T> where T : class
    {
        /// <summary>
        /// Using the arguments, generate an URL.
        /// </summary>
        WebUrl Url(T args);

        /// <summary>
        /// Using the arguments, generate an URL on the same domain, protocol and 
        /// port as the provided request.
        /// </summary>
        WebUrl Url(IWebRequest request, T args);

        /// <summary>
        /// Perform a simulated query. Run as when testing.
        /// </summary>
        WebEndpointQuery Query(T args);
    }
}
