using System.Threading.Tasks;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Web.Response
{
    /// <summary>
    /// Respond with a page. The page itself may not be fully built yet (as pages
    /// can contain task nodes).
    /// </summary>
    public class WebResponsePage : WebResponse
    {
        /// <summary>
        /// The page to be rendered as HTML and sent back to the client.
        /// </summary>
        public readonly IPage Page;

        /// <summary>
        /// The renderer recommended for rendering to HTML.
        /// </summary>
        public readonly INodeRenderer Renderer;

        public WebResponsePage(IPage page, INodeRenderer renderer, int code, IWebResponseVisitor sender) : base(code, sender)
        {
            Page = page;
            Renderer = renderer;
        }

        protected override Task Visit(IWebResponseVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override void Dispose() {}
    }
}
