using System;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Base abstract class, provides elementary functionality for node renderers.
    /// </summary>
    public abstract class BasicNodeRenderer : INodeRenderer
    {
        /// <summary>
        /// Rendering an <see cref="HtmlString"/> should always return the argument
        /// as-is.
        /// </summary>
        public void Render(HtmlString html, HtmlOutput o)
        {
            o.Add(html);
        }

        /// <summary>
        /// Applies formatting to a title, such as prefixing the site name.
        /// The default implementation does nothing.
        /// </summary>
        protected virtual string FormatTitle(string title)
        {
            return title;
        }

        /// <summary>
        /// Wraps the actual body with the default layout. 
        /// </summary>
        protected virtual Action<HtmlOutput> DefaultLayout(Action<HtmlOutput> body)
        {
            return body;
        }

        public abstract void Render(NotFoundPage notFound, HtmlOutput o);
        public abstract void Render(ColumnPage page, HtmlOutput o);
        public abstract void Render(ListVertical list, HtmlOutput o);
        public abstract void Render(Pagination pagination, HtmlOutput o);
        public abstract void Render(ItemSummary item, HtmlOutput o);
        public abstract void Render(Heading heading, HtmlOutput o);
    }
}
