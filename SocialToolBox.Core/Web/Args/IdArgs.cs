using System;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Web.Args
{
    /// <summary>
    /// An Url argument object that reads a single Id.
    /// </summary>
    public sealed class IdArgs : IWebUrlArgument
    {
        /// <summary>
        /// The identifier read or added.
        /// </summary>
        public Id Ident { get; private set; }

        public IdArgs() {}

        public IdArgs(Id ident)
        {
            Ident = ident;
        }

        public WebUrl AddTo(WebUrl url)
        {
            url.AddPathSegment(Ident.ToString());
            return url;
        }

        public bool TryParse(IWebRequest request)
        {
            if (request.UnmatchedPath.Length != 1) return false;
            try
            {
                Ident = Id.Parse(request.UnmatchedPath[0]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
