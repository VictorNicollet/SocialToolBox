using SocialToolBox.Cms.Page.Event;
using SocialToolBox.Core;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;
using SocialToolBox.Core.Entity.Projection;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Cms.Page.Projection
{
    /// <summary>
    /// A page, seen as an entity page.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Projection.PageAsEntityPage")]
    public sealed class PageAsEntityPage : IEntityPage
    {
        public PageAsEntityPage() {}

        [PersistMember(0)]
        public string Title { get; private set; }

        /// <summary>
        /// The content of the page.
        /// </summary>
        [PersistMember(1)]
        public string Body { get; private set; }

        /// <summary>
        /// Extends an entity page visitor to react to page events.
        /// </summary>
        public static void ExtendEventVisitor(Visitor<IEntityPageEvent, IEntityPage, IEntityPage> visitor)
        {
            visitor.On<PageCreated>((e,i) => new PageAsEntityPage());
            visitor.On<PageDeleted>((e,i) => null);

            visitor.On<PageTitleUpdated>((e, i) =>
            {
                var old = i == null ? null : i as PageAsEntityPage;
                if (old != null) old.Title = e.Title;
                return i;
            });

            visitor.On<PageBodyUpdated>((e, i) =>
            {
                var old = i == null ? null : i as PageAsEntityPage;
                if (old != null) old.Body = e.Body;
                return i;
            });
        }

        /// <summary>
        /// Extends the details visitor to generate a body from a page.
        /// </summary>
        public static void ExtendDetailsVisitor(VisitingExtractor<IEntityPage, IPageNode> visitor)
        {
            visitor.On<PageAsEntityPage>(page => new RichUserContent(HtmlString.Verbatim(page.Body)));
        }
    }
}
