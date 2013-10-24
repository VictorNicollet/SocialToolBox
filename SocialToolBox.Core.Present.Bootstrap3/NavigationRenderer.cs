using System.Linq;

namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// Render horizontal navigation blocks.
    /// </summary>
    public static class NavigationRenderer
    {
        /// <summary>
        /// The opening tag of the navigation block.
        /// </summary>
        public static readonly HtmlString OpenTag =
            HtmlString.Verbatim("<ul class=\"nav nav-tabs\">");

        /// <summary>
        /// The closing tag of the navigation block.
        /// </summary>
        public static readonly HtmlString CloseTag =
            HtmlString.Verbatim("</ul>");

        /// <summary>
        /// Open a primary, non-current item tag.
        /// </summary>
        public static readonly HtmlString OpenItemTag =
            HtmlString.Verbatim("<li><a href=\"");

        /// <summary>
        /// Open a primary current item tag.
        /// </summary>
        public static readonly HtmlString OpenCurrentItemTag =
            HtmlString.Verbatim("<li class=active><a href=\"");

        /// <summary>
        /// Open a secondary, non-current item tag.
        /// </summary>
        public static readonly HtmlString OpenSecondaryItemTag =
            HtmlString.Verbatim("<li class=pull-right><a href=\"");

        /// <summary>
        /// Open a secondary current item tag.
        /// </summary>
        public static readonly HtmlString OpenCurrentSecondaryItemTag =
            HtmlString.Verbatim("<li class=\"pull-right active\"><a href=\"");

        /// <summary>
        /// The middle part of an item tag: after the URL, before the label.
        /// </summary>
        public static readonly HtmlString MidItemTag =
            HtmlString.Verbatim("\">");

        /// <summary>
        /// Close an item tag.
        /// </summary>
        public static readonly HtmlString CloseItemTag =
            HtmlString.Verbatim("</a></li>");

        public static void Render(Navigation nav, HtmlOutput output)
        {
            if (nav.PrimaryLinks.Count == 0 && nav.SecondaryLinks.Count == 0) return;

            output.Add(OpenTag);

            foreach (var link in nav.PrimaryLinks)
            {
                output.Add(link.IsCurrent ? OpenCurrentItemTag : OpenItemTag);
                output.Add(link.Url.ToString());
                output.Add(MidItemTag);
                output.Add(link.Label);
                output.Add(CloseItemTag);
            }

            foreach (var link in nav.SecondaryLinks.Reverse())
            {
                output.Add(link.IsCurrent ? OpenCurrentSecondaryItemTag : OpenSecondaryItemTag);
                output.Add(link.Url.ToString());
                output.Add(MidItemTag);
                output.Add(link.Label);
                output.Add(CloseItemTag);
            }

            output.Add(CloseTag);
        }
    }
}
