namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// A web URL argument is an object that can be written to a Web URL.
    /// </summary>
    public interface IWebUrlArgument
    {
        /// <summary>
        /// Serialize an argument to an URL.
        /// </summary>
        WebUrl AddTo(WebUrl url);

        /// <summary>
        /// Attempt to parse a request. Return false if failed.
        /// </summary>
        bool TryParse(IWebRequest request);
    }
}
