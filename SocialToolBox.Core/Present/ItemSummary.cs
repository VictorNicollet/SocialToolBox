using SocialToolBox.Core.Present.Builders;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Present
{
    public class ItemSummary : IPageNode
    {
        /// <summary>
        /// The url for viewing the actual item.
        /// </summary>
        public readonly WebUrl Url;

        /// <summary>
        /// The name or title of the item.
        /// </summary>
        public readonly string Name;

        public ItemSummary(ItemBuilder builder)
        {
            Url = builder.Url;
            Name = builder.Name;
        }

        /// <summary>
        /// Start building a new item with the specified name.
        /// </summary>
        public static ItemBuilder Build(string name)
        {
            return new ItemBuilder(name);
        }

        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
