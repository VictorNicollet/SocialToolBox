using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Datatypes.RichContent
{
    /// <summary>
    /// Rich content typed in as raw HTML. May require sanitization
    /// before being output.
    /// </summary>
    [Persist("SocialToolBox.Core.Datatypes.RichContent.RawHtmlRichContent")]
    public class RawHtmlRichContent : IRichContent
    {
        /// <summary>
        /// The raw HTML content.
        /// </summary>
        [PersistMember(0)]
        public string Html { get; private set; }

        public RawHtmlRichContent() {}

        public RawHtmlRichContent(string html)
        {
            Html = html;
        }

        public HtmlString ToHtml()
        {
            return HtmlString.Verbatim(Html);
        }
    }
}
