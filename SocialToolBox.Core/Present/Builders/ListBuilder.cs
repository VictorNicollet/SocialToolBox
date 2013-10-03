using System;
using System.Collections.Generic;

namespace SocialToolBox.Core.Present.Builders
{
    /// <summary>
    /// Builds a <see cref="ListVertical"/>.
    /// </summary>
    public sealed class ListBuilder
    {
        /// <summary>
        /// Nodes in the list, in the correct order.
        /// </summary>
        public IReadOnlyList<IPageNode> Items { get { return _items; } }

        /// <summary>
        /// The actual list of nodes.
        /// </summary>
        private readonly List<IPageNode> _items = new List<IPageNode>();

        /// <summary>
        /// Append a node to the list.
        /// </summary>
        public ListBuilder Add(IPageNode node)
        {
            _items.Add(node);
            return this;
        }

        /// <summary>
        /// The pagination of the list, none by default.
        /// </summary>
        public Pagination Pagination { get; private set; }

        /// <summary>
        /// Add a pagination (or remove it, if null) to the list.
        /// </summary>
        public ListBuilder WithPagination(Pagination pagination)
        {
            Pagination = pagination;
            return this;
        }

        /// <summary>
        /// Creates a list builder from a list of arbitrary objects and a rendering 
        /// function applied to each of them.
        /// </summary>
        public static ListBuilder From<T>(IEnumerable<T> items, Func<T, IPageNode> render)
        {
            var b = new ListBuilder();
            foreach (var item in items) b.Add(render(item));
            return b;
        }

        /// <summary>
        /// Turn this builder into a vertical list.
        /// </summary>
        public ListVertical BuildVertical()
        {
            return new ListVertical(this);
        }
    }
}
