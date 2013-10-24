namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Visits nodes inside a page that is being rendered, and
    /// hopefully turns them into clean HTML.
    /// </summary>
    public interface INodeRenderer
    {
        void Render(HtmlString html, HtmlOutput o);
        void Render(NotFoundPage notFound, HtmlOutput o);
        void Render(ColumnPage page, HtmlOutput o);
        void Render(ListVertical list, HtmlOutput o);
        void Render(Pagination pagination, HtmlOutput o);
        void Render(ItemSummary item, HtmlOutput o);
        void Render(Heading title, HtmlOutput o);
        void Render(RichUserContent content, HtmlOutput o);
        void Render(Navigation navigation, HtmlOutput o);
    }
}
