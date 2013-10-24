using System.Collections.Generic;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Present.Builders
{
    // TODO: test

    /// <summary>
    /// Builds a <see cref="Navigation"/> object.
    /// </summary>
    public sealed class NavigationBuilder
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

            public Link(string label, WebUrl url, bool isCurrent = false)
            {
                Label = label;
                Url = url;
                IsCurrent = isCurrent;
            }

            public Link Clone(bool isCurrent = false)
            {
                return new Link(Label, Url, isCurrent);
            }
        }

        /// <summary>
        /// Primary links.
        /// </summary>
        private readonly IList<Link> _primary = new List<Link>();

        /// <summary>
        /// Secondary link.
        /// </summary>
        private readonly IList<Link> _secondary = new List<Link>();

        /// <summary>
        /// Primary links. If no primary links were provided, returns secondary
        /// links instead.
        /// </summary>
        public IEnumerable<Link> Primary
        {
            get
            {
                return _primary.Count == 0 ? _secondary : _primary;
            }
        }

        /// <summary>
        /// Secondary links. If no primary links were provided, secondary links
        /// become primary, and there are no secondary links returned here.
        /// </summary>
        public IEnumerable<Link> Secondary
        {
            get
            {
                return _primary.Count == 0 ? new Link[0] : _secondary;
            }
        }

        /// <summary>
        /// Adds a primary link to the builder, and returns the builder.
        /// </summary>
        public NavigationBuilder AddPrimary(Link link)
        {
            _primary.Add(link);
            return this;
        }

        /// <summary>
        /// Adds a primary link to the builder, and returns the builder.
        /// </summary>
        public NavigationBuilder AddPrimary(string label, WebUrl url, bool isCurrent = false)
        {
            return AddPrimary(new Link(label, url, isCurrent));
        }

        /// <summary>
        /// Adds a secondary link to the builder, and returns the builder.
        /// </summary>
        public NavigationBuilder AddSecondary(Link link)
        {
            _secondary.Add(link);
            return this;
        }

        /// <summary>
        /// Adds a secondary link to the builder, and returns the builder.
        /// </summary>
        public NavigationBuilder AddSecondary(string label, WebUrl url, bool isCurrent = false)
        {
            return AddSecondary(new Link(label, url, isCurrent));
        }
    }
}
