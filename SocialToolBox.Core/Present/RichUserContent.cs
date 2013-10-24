﻿namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Rich (HTML-like) content generated by the user.
    /// </summary>
    public sealed class RichUserContent : IPageNode
    {
        /// <summary>
        /// The HTML generated by the user (or from the user's input). 
        /// This should already be sanitized.
        /// </summary>
        public readonly HtmlString Html;

        public RichUserContent(HtmlString html)
        {
            Html = html;
        }

        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
