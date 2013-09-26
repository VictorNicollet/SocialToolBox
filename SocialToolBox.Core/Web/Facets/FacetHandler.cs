namespace SocialToolBox.Core.Web.Facets
{
    /// <summary>
    /// A web request handler within a facet.
    /// </summary>
    public abstract class FacetHandler<TFacet, TArgs> : WebRequestHandler<TArgs>
        where TArgs : class, IWebUrlArgument
        where TFacet : Facet
    {
        /// <summary>
        /// The facet by which this handler is registered.
        /// </summary>
        public readonly TFacet Facet;

        /// <summary>
        /// Creates a handler on a facet.
        /// </summary>
        protected FacetHandler(TFacet facet)
        {
            Facet = facet;
        }
    }
}
