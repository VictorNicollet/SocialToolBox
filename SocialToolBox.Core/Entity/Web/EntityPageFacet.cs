using System.Linq;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;
using SocialToolBox.Core.Web.Facets;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Entity.Web
{
    /// <summary>
    /// Registers web actions related to displaying individual entity 
    /// pages.
    /// </summary>
    public class EntityPageFacet : Facet
    {
        /// <summary>
        /// The module backing this facet.
        /// </summary>
        public readonly EntityModule Module;

        /// <summary>
        /// View a single entity page.
        /// </summary>
        public readonly IWebEndpoint<IdArgs> View;

        /// <summary>
        /// View all entity pages.
        /// </summary>
        public readonly IWebEndpoint<PageArgs> All;

        /// <summary>
        /// The number of pages on each page of <see cref="All"/>
        /// </summary>
        public const int PageSize = 20;

        public EntityPageFacet(IWebDriver driver, EntityModule module) : base(driver, "entity")
        {
            Module = module;
            View = OnGet<IdArgs>("").Use(() => new ViewPageHandler(this));
            All = OnGet<PageArgs>("all").Use(() => new AllPagesHandler(this));
        }

        /// <summary>
        /// Handler for viewing an individual page.
        /// </summary>
        private class ViewPageHandler : FacetHandler<EntityPageFacet,IdArgs>
        {
            public ViewPageHandler(EntityPageFacet facet) : base(facet) {}

            protected override WebResponse Process()
            {
                var pageT = Facet.Module.Pages.Get(Arguments.Ident, Cursor);

                var page = pageT.Result;
                if (page == null) return Page(new NotFound());

                return Page(ColumnPage
                    .WithTitle(page.Title)
                    .AddPrimary(HtmlString.Escape(page.Title))
                    .Build());
            }
        }

        private class AllPagesHandler : FacetHandler<EntityPageFacet, PageArgs>
        {
            public AllPagesHandler(EntityPageFacet facet) : base(facet) {}

            protected override WebResponse Process()
            {
                var pagesT = Facet.Module.PageByTitle.Query(Cursor, PageSize, PageSize * Arguments.Page);

                var pages = pagesT.Result;

                return Page(ColumnPage
                    .WithTitle("All Entity Pages")
                    .AddPrimary(HtmlString.Concat(pages.Select(p => HtmlString.Escape(p.Key.Key))))
                    .Build());
            }
        }
    }
}
