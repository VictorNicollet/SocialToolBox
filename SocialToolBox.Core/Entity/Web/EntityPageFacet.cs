using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Present.Builders;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;
using SocialToolBox.Core.Web.Facets;
using SocialToolBox.Core.Web.Response;

// TODO: I18N

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

            protected override async Task<WebResponse> Process()
            {
                var pageT = Facet.Module.Pages.Get(Arguments.Ident, Cursor);

                var page = await pageT;
                if (page == null) return Page(new NotFoundPage());

                var nav = Navigation.Horizontal()
                    .AddPrimary("Page", Facet.View.Url(Request, Arguments), true)
                    .Build();

                var output = ColumnPage
                    .WithTitle(page.Title)
                    .WithLocalNavigation(nav)
                    .AddPrimary(new Heading(page.Title, 1));

                var details = Facet.Module.PageDetailsExtractor.Visit(page);
                if (details != null) output.AddPrimary(details);

                return Page(output.Build());
            }
        }

        /// <summary>
        /// Handler for displaying all pages, in name order.
        /// </summary>
        private class AllPagesHandler : FacetHandler<EntityPageFacet, PageArgs>
        {
            public AllPagesHandler(EntityPageFacet facet) : base(facet) {}

            protected override async Task<WebResponse> Process()
            {
                var countT = Facet.Module.PageByTitle.Count(Cursor);
                var pagesT = Facet.Module.PageByTitle.Query(Cursor, PageSize, PageSize * Arguments.Page);

                var prevLink = Arguments.Page == 0 ? null :
                    Facet.All.Url(Request, new PageArgs(Arguments.Page - 1));

                var count = await countT;

                var nextLink = (Arguments.Page + 1) * PageSize >= count ? null :
                    Facet.All.Url(Request, new PageArgs(Arguments.Page + 1));

                var pages = await pagesT;

                var list = ListBuilder.From(pages, RenderPage)
                    .WithPagination(Pagination.PrevNext(prevLink, nextLink, Pagination.Position.Below))
                    .BuildVertical();

                return Page(ColumnPage
                    .WithTitle("All Entity Pages")
                    .AddPrimary(list)
                    .Build());
            }

            /// <summary>
            /// Render a page.
            /// </summary>
            private IPageNode RenderPage(KeyValuePair<StringKey, Id> arg)
            {
                return ItemSummary.Build(arg.Key.ToString())
                    .WithUrl(Facet.View.Url(Request, new IdArgs(arg.Value)))
                    .Build();
            }
        }
    }
}
