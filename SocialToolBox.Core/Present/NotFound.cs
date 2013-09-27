using System.Threading.Tasks;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A node that explains something could not be found. Often 
    /// used as a root node.
    /// </summary>
    public class NotFound : IPage
    {
        public Task<HtmlString> RenderWith(INodeRenderer visitor)
        {
            return visitor.Render(this);
        }
    }
}
