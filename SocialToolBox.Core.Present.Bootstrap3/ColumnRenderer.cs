using System.Collections.Generic;

namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// Rendering columns in grid mode.
    /// </summary>
    public static class ColumnRenderer
    {
        /// <summary>
        /// Starting column div tags depending on size.
        /// </summary>
        public static readonly HtmlString[] StartColumn = 
        {
            HtmlString.Verbatim("<div style='display:none'>"),
            HtmlString.Verbatim("<div class='col-md-1'>"),
            HtmlString.Verbatim("<div class='col-md-2'>"),
            HtmlString.Verbatim("<div class='col-md-3'>"),
            HtmlString.Verbatim("<div class='col-md-4'>"),
            HtmlString.Verbatim("<div class='col-md-5'>"),
            HtmlString.Verbatim("<div class='col-md-6'>"),
            HtmlString.Verbatim("<div class='col-md-7'>"),
            HtmlString.Verbatim("<div class='col-md-8'>"),
            HtmlString.Verbatim("<div class='col-md-9'>"),
            HtmlString.Verbatim("<div class='col-md-10'>"),
            HtmlString.Verbatim("<div class='col-md-11'>"),
            HtmlString.Verbatim("<div class='col-md-12'>")
        };

        /// <summary>
        /// Ending column div tags.
        /// </summary>
        public static readonly HtmlString EndColumn =
            HtmlString.Verbatim("</div>");

        /// <summary>
        /// Ending column div tags.
        /// </summary>
        public static readonly HtmlString StartRow =
            HtmlString.Verbatim("<div class='row'>");

        /// <summary>
        /// Endinc row div tags.
        /// </summary>
        public static readonly HtmlString EndRow = EndColumn;

        /// <summary>
        /// Render a list of columns
        /// </summary>
        public static void Render(
            IEnumerable<IEnumerable<IPageNode>> columns, 
            int[] sizes, 
            INodeRenderer renderer, 
            HtmlOutput output)
        {
            output.Add(StartRow);
            
            var i = 0;
            foreach (var column in columns)
            {
                if (i >= sizes.Length) break;
                output.Add(StartColumn[sizes[i++]]);
                output.InsertNodes(column, renderer);
                output.Add(EndColumn);                
            }

            output.Add(EndRow);
        }
    }
}
