namespace SocialToolBox.Core.Web.Args
{
    /// <summary>
    /// A single argument representing a page number.
    /// </summary>
    public class PageArgs : WithPageArgs
    {
        public PageArgs() {}
        public PageArgs(int page) : base(page) {}

        public override WebUrl AddTo(WebUrl url)
        {
            AppendTo(url);
            return url;
        }

        public override bool TryParse(IWebRequest request)
        {
            var startAt = 0;
            return TryParsePage(request, ref startAt);
        }
    }
}
