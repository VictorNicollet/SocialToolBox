using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Datatypes.RichContent
{
    /// <summary>
    /// Rich content typed in as raw HTML. May require sanitization
    /// before being output.
    /// </summary>
    public class RawHtmlRichContent : IRichContent
    {
        public HtmlString ToHtml()
        {
            throw new System.NotImplementedException();
        }
    }
}
