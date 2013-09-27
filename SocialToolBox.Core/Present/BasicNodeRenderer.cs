using System.Threading.Tasks;

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
        public Task<HtmlString> Render(HtmlString html)
        {
            return Task.FromResult(html);
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
        protected virtual HtmlString DefaultLayout(HtmlString body)
        {
            return body;
        }

        public abstract Task<HtmlString> Render(NotFound notFound);
        public abstract Task<HtmlString> Render(ColumnPage page);
    }
}
