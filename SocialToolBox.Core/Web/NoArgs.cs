namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// Represents no arguments.
    /// </summary>
    public class NoArgs : IWebUrlArgument
    {
        public WebUrl AddTo(WebUrl url) { return url; }

        /// <summary>
        /// Parsing no-args expects that no unmatched path segments are found.
        /// </summary>
        public bool TryParse(IWebRequest request)
        {
            return (request.UnmatchedPath.Length == 0);
        }
    }
}
