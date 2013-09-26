﻿using System.Threading.Tasks;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// A page node. Describes what appears on the page, but
    /// not what HTML should be used to render it.
    /// </summary>
    public interface IPageNode
    {
        Task<HtmlString> Visit(IPageNodeVisitor visitor);
    }
}