namespace SocialToolBox.Core.Web.Args
{
    public class PageArgs : WithPageArgs
    {
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
