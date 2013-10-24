namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A page. Described the page root, but not how it should be 
    /// rendered.
    /// </summary>
    public interface IPage : IPageNode
    {
        /// <summary>
        /// The local navigation for this page. Will usually be different
        /// depending on the page.
        /// </summary>
        Navigation LocalNavigation { get; }
    }
}
