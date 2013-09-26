using System.Threading.Tasks;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Mocks.Present
{
    /// <summary>
    /// A very simple node renderer.
    /// </summary>
    public class NodeRenderer : INodeRenderer
    {
        public Task<HtmlString> Render(HtmlString html)
        {
            return Task.FromResult(html);
        }
    }
}
