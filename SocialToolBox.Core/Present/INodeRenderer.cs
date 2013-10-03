namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Visits nodes inside a page that is being rendered, and
    /// hopefully turns them into clean HTML.
    /// </summary>
    public interface INodeRenderer
    {
        void Render(HtmlString html, HtmlOutput o);
        void Render(NotFound notFound, HtmlOutput o);
        void Render(ColumnPage page, HtmlOutput o);
        void Render(ListVertical list, HtmlOutput o);
        void Render(Pagination pagination, HtmlOutput o);
    }
}
