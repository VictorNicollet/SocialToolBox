using System.Collections.Generic;
using System.Linq;
using SocialToolBox.Core.Present.Builders;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A full web page. Build using a columns-as-stacks-of-nodes approach. 
    /// </summary>
    public sealed class ColumnPage : IPage
    {
        /// <summary>
        /// The title of this page.
        /// </summary>
        public readonly string Title;

        /// <summary>
        /// All the columns, by order of importance.
        /// </summary>
        public readonly IEnumerable<IPageNode>[] Columns;

        /// <summary>
        /// Local navigation, returned by <see cref="LocalNavigation"/>.
        /// </summary>
        private readonly Navigation _localNavigation;

        public ColumnPage(ColumnPageBuilder source)
        {
            Title = source.Title;
            Columns = new[] {source.Primary, source.Secondary, source.Tertiary}
                .Where(column => column.Count > 0)
                .Select(column => (IEnumerable<IPageNode>) column)
                .ToArray();
            _localNavigation = source.LocalNavigation;
        }

        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }

        /// <summary>
        /// Start building a new page.
        /// </summary>
        public static ColumnPageBuilder WithTitle(string title)
        {
            return new ColumnPageBuilder(title);
        }

        public Navigation LocalNavigation { get { return _localNavigation;  } }
    }
}
