using System.Collections.Generic;

namespace SocialToolBox.Core.Present.Builders
{
    /// <summary>
    /// Builds a <see cref="ColumnPage"/> from scratch.
    /// </summary>
    public class ColumnPageBuilder
    {
        /// <summary>
        /// The title of the page.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nodes in the primary column. If empty, secondary is
        /// promoted to primary.
        /// </summary>
        public IReadOnlyList<IPageNode> Primary { get { return _primary; } }

        /// <summary>
        /// Actual list of nodes in the primary column.
        /// </summary>
        private readonly List<IPageNode> _primary = new List<IPageNode>(); 

        /// <summary>
        /// Nodes in the secondary column. If empty, tertiary column
        /// is promoted to secondary.
        /// </summary>
        public IReadOnlyList<IPageNode> Secondary { get { return _secondary; } }

        /// <summary>
        /// Actual list of nodes in the secondary column.
        /// </summary>
        private readonly List<IPageNode> _secondary = new List<IPageNode>(); 

        /// <summary>
        /// Nodes in the tertiary column. 
        /// </summary>
        public IReadOnlyList<IPageNode> Tertiary { get { return _tertiary; } }

        /// <summary>
        /// Actual list of nodes in the tertiary column.
        /// </summary>
        private readonly List<IPageNode> _tertiary = new List<IPageNode>(); 

        /// <summary>
        /// Add a node at the bottom of the primary column.
        /// </summary>
        public ColumnPageBuilder AddPrimary(IPageNode node) 
        { 
            _primary.Add(node);
            return this;
        }

        /// <summary>
        /// Add a node at the bottom of the secondary column.
        /// </summary>
        public ColumnPageBuilder AddSecondary(IPageNode node)
        {
            _secondary.Add(node);
            return this;
        }

        /// <summary>
        /// Add a node at the bottom of the tertiary column.
        /// </summary>
        public ColumnPageBuilder AddTertiary(IPageNode node)
        {
            _tertiary.Add(node);
            return this;
        }

        public ColumnPageBuilder(string title)
        {
            Title = title;
        }

        public ColumnPage Build()
        {
            return new ColumnPage(this);
        }
    }
}
