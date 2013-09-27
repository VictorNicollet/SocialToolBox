namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A node that explains something could not be found. Often 
    /// used as a root node.
    /// </summary>
    public class NotFound : IPage
    {
        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
