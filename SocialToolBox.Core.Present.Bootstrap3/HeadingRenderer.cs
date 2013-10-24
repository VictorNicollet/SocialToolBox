namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// Renders headings.
    /// </summary>
    public static class HeadingRenderer
    {
        private static readonly HtmlString[] OpenTags = 
        {
            HtmlString.Verbatim("<h1>"),
            HtmlString.Verbatim("<h2>"),
            HtmlString.Verbatim("<h3>"),
            HtmlString.Verbatim("<h4>"),
            HtmlString.Verbatim("<h5>"),
            HtmlString.Verbatim("<h6>")
        };

        private static readonly HtmlString[] CloseTags =
        {
            HtmlString.Verbatim("</h1>"),
            HtmlString.Verbatim("</h2>"),
            HtmlString.Verbatim("</h3>"),
            HtmlString.Verbatim("</h4>"),
            HtmlString.Verbatim("</h5>"),
            HtmlString.Verbatim("</h6>")
        };

        public static void Render(Heading heading, HtmlOutput output)
        {
            output.Add(OpenTags[heading.Level - 1]);
            output.Add(heading.Label);
            output.Add(CloseTags[heading.Level - 1]);
        }
    }
}
