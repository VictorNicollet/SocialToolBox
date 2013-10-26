using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Datatypes.RichContent
{
    /// <summary>
    /// Rich content is content entered by an end user that can be turned
    /// into HTML. This could be actual HTML, or Markdown, or another format
    /// that can be compiled to HTML.
    /// </summary>
    public interface IRichContent
    {
        /// <summary>
        /// Compile the rich content to an HTML string.
        /// </summary>
        HtmlString ToHtml();
    }
}
