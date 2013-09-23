using System.Threading.Tasks;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// An arbitrary piece of HTML. Can also appear as a page node.
    /// </summary>
    public class HtmlString : IPageNode
    {
        public Task<HtmlString> Visit(IPageNodeVisitor visitor)
        {
            return visitor.Render(this);
        }
    }
}
