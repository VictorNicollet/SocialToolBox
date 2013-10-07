using SocialToolBox.Cms.Page.Event;
using SocialToolBox.Core;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Projection;

namespace SocialToolBox.Cms.Page.Projection
{
    /// <summary>
    /// A page, seen as an entity page.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Projection.PageAsEntityPage")]
    public sealed class PageAsEntityPage : IEntityPage
    {
        public PageAsEntityPage() {}

        public string Title { get; private set; }

        /// <summary>
        /// Extends an entity page visitor to react to contacte events.
        /// </summary>
        public static void ExtendVisitor(Visitor<IEntityPage, IEntityPage> visitor)
        {
            visitor.On<PageCreated>((e,i) => new PageAsEntityPage());
            visitor.On<PageDeleted>((e,i) => null);

            visitor.On<PageTitleUpdated>((e, i) =>
            {
                var old = i == null ? null : i as PageAsEntityPage;
                if (old != null) old.Title = e.Title;
                return i;
            });
        }
    }
}
