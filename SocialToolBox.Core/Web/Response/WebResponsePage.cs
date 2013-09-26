using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Web.Response
{
    /// <summary>
    /// Respond with a page.
    /// </summary>
    public class WebResponsePage : WebResponse
    {
        /// <summary>
        /// The page to be rendered as HTML and sent back to the client.
        /// </summary>
        public readonly IPageNode Page;

        /// <summary>
        /// The renderer recommended for rendering to HTML.
        /// </summary>
        public readonly IPageNodeVisitor Renderer;

        public WebResponsePage(IPageNode page, IPageNodeVisitor renderer, int code, IWebResponseVisitor sender) : base(code, sender)
        {
            Page = page;
            Renderer = renderer;
        }

        protected override void Visit(IWebResponseVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Dispose() {}
    }
}
