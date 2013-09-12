using System;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// An enumeration representing all supported HTTP verbs.
    /// </summary>
    [Flags]
    public enum HttpVerb
    {
        Get    = 0x01,
        Post   = 0x02,
        Put    = 0x04,
        Delete = 0x08
    }
}
