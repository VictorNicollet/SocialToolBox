using SocialToolBox.Cms.Page.Projection;
using SocialToolBox.Core.Entity;

namespace SocialToolBox.Cms.Page
{
    /// <summary>
    /// Extensions involved in making the page module connect to the
    /// entity module.
    /// </summary>
    public static class PageModuleEntityExtensions
    {        
        /// <summary>
        /// Register all pages as entities : these will appear as entity
        /// pages, in entity searches, and so on.
        /// </summary>
        public static void RegisterPagesAsEntities(this PageModule pmodule, EntityModule emodule)
        {
            emodule.AddEventStream(pmodule.Stream);
            PageAsEntityPage.ExtendEventVisitor(emodule.PageEventVisitor);
            PageAsEntityPage.ExtendDetailsVisitor(emodule.PageDetailsExtractor);
        }
    }
}
