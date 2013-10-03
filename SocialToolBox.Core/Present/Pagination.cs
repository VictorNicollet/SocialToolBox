namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Represents a pagination block.
    /// </summary>
    public class Pagination : IPageNode
    {
        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
