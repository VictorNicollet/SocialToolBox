using System.Collections.Generic;

namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// Rendering lists.
    /// </summary>
    public static class ListRenderer
    {
        /// <summary>
        /// Renders a list of items with no wrappers.
        /// </summary>
        public static void RenderStacked(
            IEnumerable<IPageNode> items, INodeRenderer renderer, HtmlOutput output)
        {
            output.InsertNodes(items, renderer);
        }
    }
}
