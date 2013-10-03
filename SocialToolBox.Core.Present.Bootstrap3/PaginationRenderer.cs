using System.Collections.Generic;
using System.Linq;

namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// Rendering various versions of the pagination component.
    /// </summary>
    public static class PaginationRenderer
    {
        /// <summary>
        /// Starting prev/next paginator tag.
        /// </summary>
        public static readonly HtmlString StartPager =
            HtmlString.Verbatim("<ul class='pager'>");

        /// <summary>
        /// Closing prev/next paginator tag.
        /// </summary>
        public static readonly HtmlString EndPager =
            HtmlString.Verbatim("</ul>");

        /// <summary>
        /// Render a previous/next paginator.
        /// </summary>
        public static void RenderPrevNext(IEnumerable<Pagination.Link> links, HtmlOutput output)
        {
            var prevNext = links.Take(2).ToArray();
        
            output.Add(StartPager);
            if (prevNext.Length > 0) RenderLink(prevNext[0], output, "previous");
            if (prevNext.Length > 1) RenderLink(prevNext[1], output, "next");
            output.Add(EndPager);
        }

        /// <summary>
        /// Render an individual link.
        /// </summary>
        private static void RenderLink(Pagination.Link link, HtmlOutput output, string cssClass)
        {
            if (cssClass == null)
            {
                if (link.IsCurrent) cssClass = "active";
                else if (link.Url == null) cssClass = "disabled";
            }
            else if (link.Url == null) cssClass += " disabled";

            var url = link.Url == null ? "javascript:void(0)" : link.Url.ToString();

            if (cssClass != null)
            {
                output.Add(HtmlString.Format("<li class='{2}'><a href='{0}'>{1}</a></li>",
                    url, link.Label, cssClass));
            }
            else
            {
                output.Add(HtmlString.Format("<li><a href='{0}'>{1}</a></li>",
                    url, link.Label));
            }
        }
    }
}
