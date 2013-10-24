using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Present.Builders
{
    /// <summary>
    /// Builds a <see cref="ItemSummary"/>.
    /// </summary>
    public class ItemBuilder
    {
        /// <summary>
        /// The Url at which the item can be viewed.
        /// </summary>
        public WebUrl Url { get; private set; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Add an Url to the item.
        /// </summary>
        public ItemBuilder WithUrl(WebUrl url)
        {
            Url = url;
            return this;
        }

        public ItemBuilder(string name)
        {
            Name = name;
        }

        public ItemSummary Build()
        {
            return new ItemSummary(this);
        }
    }
}
