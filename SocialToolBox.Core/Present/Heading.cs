using System;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A heading: a bold, title-like block for presenting a piece of 
    /// primary information.
    /// </summary>
    public sealed class Heading : IPageNode
    {
        /// <summary>
        /// The label to be presented.
        /// </summary>
        public readonly string Label;

        /// <summary>
        /// The importance level, between 1 (most important) and 6.
        /// </summary>
        public readonly int Level;

        public Heading(string label, int level)
        {
            if (level < 1 || level > 6)
                throw new ArgumentOutOfRangeException("level", level, 
                    "Only levels 1-6 are allowed for headings");

            Label = label;
            Level = level;
        }

        public void RenderWith(INodeRenderer visitor, HtmlOutput output)
        {
            visitor.Render(this, output);
        }
    }
}
