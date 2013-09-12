using System;

namespace SocialToolBox.Core.Web.Response
{
    /// <summary>
    /// A cookie, as part of a web response.
    /// </summary>
    public class WebResponseCookie : IEquatable<WebResponseCookie>
    {
        public readonly string Name;
        public readonly string Domain;
        public readonly TimeSpan? Expires;

        public WebResponseCookie(string name, string domain, TimeSpan? expires)
        {
            Name = name;
            Domain = domain;
            Expires = expires;
        }

        public bool Equals(WebResponseCookie other)
        {
            return Name == other.Name
                   && Domain == other.Domain
                   && Expires == other.Expires;
        }
    }
}
