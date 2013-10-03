using System.Collections.Generic;

namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// Rendering lists.
    /// </summary>
    public static class ListRenderer
    {
        /// <summary>
        /// Opening tag for a stack of items.
        /// </summary>
        public static readonly HtmlString StartStack =
            HtmlString.Verbatim("<table class='table'>");

        /// <summary>
        /// Closing tag for a stack of items.
        /// </summary>
        public static readonly HtmlString EndStack =
            HtmlString.Verbatim("</table>");

        /// <summary>
        /// Opening tag for an item in a stack.
        /// </summary>
        public static readonly HtmlString StartStackItem =
            HtmlString.Verbatim("<tr><td>");

        /// <summary>
        /// Closing tag for an item in a stack.
        /// </summary>
        public static readonly HtmlString EndStackItem =
            HtmlString.Verbatim("</td></tr>");

        /// <summary>
        /// Renders a list of items with no wrappers.
        /// </summary>
        public static void RenderStacked(
            IEnumerable<IPageNode> items, Pagination pagination, INodeRenderer renderer, HtmlOutput output)
        {
            output.Add(StartStack);

            if ((pagination.Where & Pagination.Position.Above) != 0)
            {
                output.Add(StartStackItem);
                output.InsertNode(pagination, renderer);
                output.Add(EndStackItem);
            }

            foreach (var item in items)
            {
                output.Add(StartStackItem);
                output.InsertNode(item, renderer);
                output.Add(EndStackItem);
            }

            if ((pagination.Where & Pagination.Position.Below) != 0)
            {
                output.Add(StartStackItem);
                output.InsertNode(pagination, renderer);
                output.Add(EndStackItem);
            }
            
            output.Add(EndStack);
        }
    }
}
