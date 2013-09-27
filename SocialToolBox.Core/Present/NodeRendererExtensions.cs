using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Extends <see cref="HtmlOutput"/> to make rendering of <see cref="IPageNode"/> objects
    /// easier.
    /// </summary>
    public static class NodeRendererExtensions
    {
        /// <summary>
        /// Render a page node using a renderer, once the page node is 
        /// available.
        /// </summary>
        public static void InsertNode(this HtmlOutput output, Task<IPageNode> node, INodeRenderer renderer)
        {
            output.Insert(o => node.ContinueWith(t => t.Result.RenderWith(renderer, o)));
        }

        /// <summary>
        /// Add a range of asynchronous HTML strings.
        /// </summary>
        public static void InsertNodes(this HtmlOutput output, IEnumerable<Task<IPageNode>> nodes, INodeRenderer renderer)
        {
            foreach (var node in nodes) output.InsertNode(node, renderer);
        }

        /// <summary>
        /// Add single rendered HTML strings.
        /// </summary>
        public static void InsertNode(this HtmlOutput output, IPageNode node, INodeRenderer renderer)
        {
            node.RenderWith(renderer, output);
        }

        /// <summary>
        /// Add a range of rendered HTML strings.
        /// </summary>
        public static void InsertNodes(this HtmlOutput output, IEnumerable<IPageNode> nodes, INodeRenderer renderer)
        {
            foreach (var node in nodes) node.RenderWith(renderer, output);
        }
    }
}
