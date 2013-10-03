using System;
using System.Collections.Generic;
using System.Linq;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Represents a pagination block.
    /// </summary>
    public class Pagination : IPageNode
    {
        /// <summary>
        /// Represents the position of the pagination block around a 
        /// page.
        /// </summary>
        [Flags]
        public enum Position
        {
            Above = 1,
            Below = 2,
            Both = 3
        }

        /// <summary>
        /// If rendered as part of a page, list or other pagination-aware concept,
        /// where should it be rendered ? 
        /// </summary>
        public readonly Position Where;

        /// <summary>
        /// An individual pagination link.
        /// </summary>
        public class Link
        {
            /// <summary>
            /// The URL at which the link points. If no link is
            /// provided, link is disabled.
            /// </summary>
            public readonly WebUrl Url;

            /// <summary>
            /// The label displayed on this link.
            /// </summary>
            public readonly string Label;

            /// <summary>
            /// Whether this is the current link.
            /// </summary>
            public readonly bool IsCurrent;

            public Link(WebUrl url, string label, bool isCurrent)
            {
                Url = url;
                Label = label;
                IsCurrent = isCurrent;
            }
        }

        /// <summary>
        /// Is this pagination object a simple "previous, next" ?
        /// If so, only the first two elements of the link enumeration
        /// are kept, an used as previous and next respectively.
        /// </summary>
        public readonly bool IsPrevNext;

        /// <summary>
        /// All the available links. 
        /// </summary>
        public readonly IReadOnlyList<Link> Links;

        /// <summary>
        /// Build a previous/next pagination block.
        /// </summary>
        public static Pagination PrevNext(WebUrl prevUrl, WebUrl nextUrl, Position where = Position.Both)
        {
            var links = new[] {
                new Link(prevUrl, "«", false),
                new Link(nextUrl, "»", false)
            };
            
            return new Pagination(links, true, where);
        }

        /// <summary>
        /// Private constructor, use static builders instead.
        /// </summary>
        private Pagination(IEnumerable<Link> links, bool isPrevNext, Position where)
        {
            IsPrevNext = isPrevNext;
            Where = where;
            Links = links.ToList();
        }

        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
