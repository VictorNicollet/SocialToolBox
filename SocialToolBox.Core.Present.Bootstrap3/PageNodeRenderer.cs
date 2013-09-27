using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        protected virtual HtmlString RenderHtml(string title, HtmlString body)
        {
            var head = HtmlString.Format(
                "<!DOCTYPE html><html><head>" +
                "<meta charset='utf-8'/>" +
                "<title>{0}</title>" +
                "<link rel='stylesheet' href='{1}'/></head><body>",
                title, 
                BootstrapCssUrl);

            var foot = HtmlString.Format(
                "<script src='{0}'></script></body></html>",
                BootstrapJsUrl
                );

            return HtmlString.Concat(head, body, foot);
        }

        /// <summary>
        /// The default layout includes a centered container.
        /// </summary>
        protected override HtmlString DefaultLayout(HtmlString body)
        {
            return HtmlString.Concat(
                HtmlString.Verbatim("<div class='container'>"),
                body,
                HtmlString.Verbatim("</div>"));
        }

        /// <summary>
        /// The title of the 404 not found page.
        /// </summary>
        public string NotFoundTitle { get; set; }

        /// <summary>
        /// The body of the 404 not found page.
        /// </summary>
        public HtmlString NotFoundBody { get; set; }

        public override Task<HtmlString> Render(NotFound notFound)
        {
            return Task.FromResult(RenderHtml(
                FormatTitle(NotFoundTitle), DefaultLayout(NotFoundBody)));
        }

        /// <summary>
        /// ColumnSizes[n][i] returns the size of column 'i' when there are
        /// 'n' columns total (1 ≤ n ≤ 3). Expressed in fractions of 12.
        /// </summary>
        public int[][] ColumnSizes { get; set; }

        public override async Task<HtmlString> Render(ColumnPage page)
        {
            var nColumns = page.Columns.Length;
            if (nColumns == 0) return HtmlString.Verbatim("");
            if (nColumns >  3) nColumns = 3;

            var sizes = ColumnSizes[nColumns];

            var body = await ColumnRenderer.Render(page.Columns, sizes, this);

            return RenderHtml(FormatTitle(page.Title), DefaultLayout(body));
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
