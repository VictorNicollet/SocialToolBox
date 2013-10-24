using System.Collections.Generic;
using System.Linq;
using SocialToolBox.Core.Present.Builders;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A navigation block. 
    /// </summary>
    public sealed class Navigation : IPageNode
    {
        /// <summary>
        /// An individual link inside the navigation block.
        /// </summary>
        public sealed class Link
        {
            /// <summary>
            /// The text to be displayed for this link.
            /// </summary>
            public readonly string Label;

            /// <summary>
            /// The URL activated when clicking this link.
            /// </summary>
            public readonly WebUrl Url;

            /// <summary>
            /// Is this the currently selected link ? 
            /// </summary>
            public readonly bool IsCurrent;

            public Link(NavigationBuilder.Link link)
            {
                Label = link.Label;
                Url = link.Url;
                IsCurrent = link.IsCurrent;
            }
        }

        /// <summary>
        /// The primary links of this navigation block. Recommended
        /// display: top or left.
        /// </summary>
        public readonly IReadOnlyList<Link> PrimaryLinks;

        /// <summary>
        /// The secondary links of this navigation block. Recommended 
        /// display: bottom or right.
        /// </summary>
        public readonly IReadOnlyList<Link> SecondaryLinks;

        /// <summary>
        /// Start building a new item.
        /// </summary>
        public static NavigationBuilder Horizontal()
        {
            return new NavigationBuilder();
        }

        public Navigation(NavigationBuilder builder)
        {
            PrimaryLinks = builder.Primary.Select(link => new Link(link)).ToList();
            SecondaryLinks = builder.Secondary.Select(link => new Link(link)).ToList();
        }

        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
