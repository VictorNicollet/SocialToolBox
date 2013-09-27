using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Mocks.Present
{
    /// <summary>
    /// A very simple node renderer.
    /// </summary>
    public class NodeRenderer : INodeRenderer
    {
        public void Render(HtmlString html, HtmlOutput output)
        {
            output.Add(html);
        }

        public void Render(NotFound notFound, HtmlOutput output)
        {
            output.Add("Not Found !");
        }

        public void Render(ColumnPage page, HtmlOutput output)
        {
            output.Add("Page !");
        }
    }
}
