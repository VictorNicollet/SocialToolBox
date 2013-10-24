
using System;

namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// A renderer system that uses Bootstrap3 for design.
    /// </summary>
    public class PageNodeRenderer : BasicNodeRenderer
    {
        public string BootstrapCssUrl =
            "//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css";
        public string BootstrapJsUrl =
            "//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.js";

        /// <summary>
        /// Renders a wrapper around the provided body, including bootstrap
        /// code along.
        /// </summary>
        protected virtual Action<HtmlOutput> RenderHtml(string title, Action<HtmlOutput> body)
        {
            return output =>
            {
                output.AddVerbatim("<!DOCTYPE html><html><head><meta charset='utf-8'/><title>");
                output.Add(title);
                output.AddVerbatim("</title><link rel='stylesheet' href='");
                output.Add(BootstrapCssUrl);
                output.AddVerbatim("'/></head><body>");
                body(output);
                output.AddVerbatim("<script src='");
                output.Add(BootstrapJsUrl);
                output.AddVerbatim("'></script></body></html>");                
            };
        }

        /// <summary>
        /// The default layout includes a centered container.
        /// </summary>
        protected override Action<HtmlOutput> DefaultLayout(Action<HtmlOutput> body)
        {
            return output =>
            {
                output.AddVerbatim("<div class='container'>");
                body(output);
                output.AddVerbatim("</div>");
            };
        }

        /// <summary>
        /// The title of the 404 not found page.
        /// </summary>
        public string NotFoundTitle { get; set; }

        /// <summary>
        /// The body of the 404 not found page.
        /// </summary>
        public HtmlString NotFoundBody { get; set; }

        public override void Render(NotFoundPage notFound, HtmlOutput output)
        {
            RenderHtml(
                FormatTitle(NotFoundTitle),
                DefaultLayout(o => o.Add(NotFoundBody)))
                (output);
        }

        /// <summary>
        /// ColumnSizes[n][i] returns the size of column 'i' when there are
        /// 'n' columns total (1 ≤ n ≤ 3). Expressed in fractions of 12.
        /// </summary>
        public int[][] ColumnSizes { get; set; }

        public override void Render(ColumnPage page, HtmlOutput output)
        {
            var nColumns = page.Columns.Length;
            if (nColumns == 0) return;
            if (nColumns >  3) nColumns = 3;

            var sizes = ColumnSizes[nColumns];

            RenderHtml(
                FormatTitle(page.Title), 
                DefaultLayout(o => ColumnRenderer.Render(page.Columns, sizes, this, o)))
                (output);
        }

        public override void Render(ListVertical list, HtmlOutput output)
        {
            ListRenderer.RenderStacked(list.Items, list.Pagination, this, output);
        }

        public override void Render(Pagination pagination, HtmlOutput output)
        {
            if (pagination.IsPrevNext)
                PaginationRenderer.RenderPrevNext(pagination.Links, output);
        }

        public override void Render(ItemSummary item, HtmlOutput output)
        {            
            ItemRenderer.RenderName(item, output);
        }

        public override void Render(Heading heading, HtmlOutput output)
        {
            HeadingRenderer.Render(heading, output);
        }

        public override void Render(RichUserContent content, HtmlOutput o)
        {
            o.AddVerbatim("<div class=\"user-content\">");
            o.Add(content.Html);
            o.AddVerbatim("</div>");
        }

        public override void Render(Navigation navigation, HtmlOutput o)
        {            
        }

        public PageNodeRenderer()
        {
            NotFoundTitle = "Page not found";
            NotFoundBody =
                HtmlString.Format(
                    "<div class='jumbotron'><h1>Page not found</h1><p>We're really very sorry about this.</p></div>");
            ColumnSizes = new[]
            {
                null,
                new[] {12},
                new[] {9, 3},
                new[] {3, 6, 3}
            };
        }
    }
}
