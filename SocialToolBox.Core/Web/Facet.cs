namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// Serves as a base class for facets, which install web actions
    /// on a web driver.
    /// </summary>
    public abstract class Facet
    {
        /// <summary>
        /// The driver on which the install is performed.
        /// </summary>
        public readonly IWebDriver Driver;

        /// <summary>
        /// The string prefix prepended to all actions, such as
        /// '/entity/'
        /// </summary>
        public readonly string Namespace;

        /// <summary>
        /// Performs an install with the specified prefix namespace on 
        /// the specified driver.
        /// </summary>
        protected Facet(IWebDriver driver, string prefix)
        {
            Driver = driver;

            prefix = "/" + prefix.Trim('/') + '/';
            if (prefix == "//") prefix = "/";
            Namespace = prefix;
        }

        /// <summary>
        /// Adds a request that responds to the GET verb.
        /// </summary>
        protected WebEndpointBuilder<TArgs> OnGet<TArgs>(string url)            
            where TArgs : class, IWebUrlArgument, new()
        {
            return new WebEndpointBuilder<TArgs>(
                Driver, HttpVerb.Get, Namespace + "/" + url.TrimStart('/'));
        }

        /// <summary>
        /// Adds a request that responds to the POST verb.
        /// </summary>
        protected WebEndpointBuilder<TArgs> OnPost<TArgs>(string url)
            where TArgs : class, IWebUrlArgument, new()
        {
            return new WebEndpointBuilder<TArgs>(
                Driver, HttpVerb.Post, Namespace + "/" + url.TrimStart('/'));
        }
    }
}
