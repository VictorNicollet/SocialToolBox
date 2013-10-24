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

        public void Render(NotFoundPage notFound, HtmlOutput output)
        {
            output.Add("Not Found !");
        }

        public void Render(ColumnPage page, HtmlOutput output)
        {
            output.Add("Page !");
        }

        public void Render(ListVertical list, HtmlOutput output)
        {
            output.Add("List !");
        }

        public void Render(Pagination pagination, HtmlOutput output)
        {
            output.Add("Pagination !");
        }

        public void Render(ItemSummary item, HtmlOutput output)
        {
            output.Add("Item !");
        }

        public void Render(Heading title, HtmlOutput output)
        {
            output.Add("Title !");
        }

        public void Render(RichUserContent content, HtmlOutput output)
        {
            output.Add("Rich user content !");
        }
    }
}
