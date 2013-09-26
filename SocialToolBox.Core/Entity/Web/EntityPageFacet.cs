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

        public EntityPageFacet(IWebDriver driver, EntityModule module) : base(driver, "entity")
        {
            Module = module;
            View = OnGet<IdArgs>("").Use(new ViewPageHandler(this));
        }

        private class ViewPageHandler : FacetHandler<EntityPageFacet,IdArgs>
        {
            public ViewPageHandler(EntityPageFacet facet) : base(facet) {}

            protected override WebResponse Process()
            {
                return Html(Arguments.Ident.ToString());
            }
        }
    }
}
