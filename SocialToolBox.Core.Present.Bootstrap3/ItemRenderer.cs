namespace SocialToolBox.Core.Present.Bootstrap3
{
    /// <summary>
    /// Rendering <see cref="ItemSummary"/> nodes.
    /// </summary>
    public static class ItemRenderer
    {
        /// <summary>
        /// Render the name (and link) part of the item summary.
        /// </summary>
        public static void RenderName(ItemSummary item, HtmlOutput output)
        {
            if (item.Url == null)
            {
                output.Add(item.Name);
                return;
            }

            output.AddVerbatim("<a href='");
            output.Add(item.Url.ToString());
            output.AddVerbatim("'>");
            output.Add(item.Name);
            output.AddVerbatim("</a>");
        }
    }
}
