namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A navigation block. 
    /// </summary>
    public sealed class Navigation : IPageNode
    {
        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
