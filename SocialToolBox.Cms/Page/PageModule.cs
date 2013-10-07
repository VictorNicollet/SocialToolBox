using SocialToolBox.Core.Database;

namespace SocialToolBox.Cms.Page
{
    /// <summary>
    /// The plug-in module for CMS pages.
    /// </summary>
    public sealed class PageModule
    {
        /// <summary>
        /// The name of the <see cref="Stream"/>
        /// </summary>
        public const string StreamName = "SocialToolBox.Cms.Page.PageModule.Stream";

        /// <summary>
        /// The database driver used by this module.
        /// </summary>
        public readonly IDatabaseDriver Driver;

        /// <summary>
        /// The stream to which all contact-related events should be appended.
        /// </summary>
        public readonly IEventStream Stream;

        public PageModule(IDatabaseDriver driver)
        {
            Driver = driver;
            Stream = driver.GetEventStream(StreamName, true);
        }

        /// <summary>
        /// Has this module been compiled yet ?
        /// </summary>
        public bool Compiled { get; private set; }

        /// <summary>
        /// Create and compile all projections in this module.
        /// </summary>
        public void Compile()
        {
            Compiled = true;
        }
    }
}
