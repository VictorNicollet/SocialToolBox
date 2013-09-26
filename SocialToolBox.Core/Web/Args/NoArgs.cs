namespace SocialToolBox.Core.Web.Args
{
    /// <summary>
    /// Represents no arguments. Singleton.
    /// </summary>
    public class NoArgs : IWebUrlArgument, IConvertibleToArgs<AnyArgs>
    {
        public WebUrl AddTo(WebUrl url) { return url; }

        /// <summary>
        /// Parsing no-args expects that no unmatched path segments are found.
        /// </summary>
        public bool TryParse(IWebRequest request)
        {
            return (request.UnmatchedPath.Length == 0);
        }

        /// <summary>
        /// Static implementation.
        /// </summary>
        public static readonly NoArgs It = new NoArgs();
        
        public AnyArgs ToArgs()
        {
            return new AnyArgs();
        }
    }
}
