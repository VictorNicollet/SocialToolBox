using System.Collections.Generic;
using SocialToolBox.Core.Present.Builders;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A list of items, stacked vertically.
    /// </summary>
    public class ListVertical : IPageNode
    {
        /// <summary>
        /// Items present in the list, in the correct order.
        /// </summary>
        public readonly IReadOnlyList<IPageNode> Items;

        /// <summary>
        /// How is this list paginated, if applicable ? 
        /// </summary>
        public readonly Pagination Pagination;

        public ListVertical(ListBuilder source)
        {
            Items = source.Items;
            Pagination = source.Pagination;
        }

        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
