using System.Threading.Tasks;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A page node, part of a page tree. Specifies the contents, but not
    /// how they should be rendered. Instead, a node renderer should be
    /// used for rendering.
    /// </summary>
    public interface IPageNode
    {
        Task<HtmlString> RenderWith(INodeRenderer visitor);
    }
}
