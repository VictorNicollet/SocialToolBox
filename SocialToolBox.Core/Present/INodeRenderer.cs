using System.Threading.Tasks;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Visits nodes inside a page that is being rendered, and
    /// hopefully turns them into clean HTML.
    /// </summary>
    public interface INodeRenderer
    {
        Task<HtmlString> Render(HtmlString html);
    }
}
