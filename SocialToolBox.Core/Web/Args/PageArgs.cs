using System;
using System.Globalization;

namespace SocialToolBox.Core.Web.Args
{
    /// <summary>
    /// Serializes a page number. On the C# side, page numbers are zero-based. On the web side, 
    /// the first page is page 1, and "1" is never serialized as a segment (though it is accepted). 
    /// </summary>
    public class PageArgs : IWebUrlArgument
    {
        /// <summary>
        /// Zero-based page number. 
        /// </summary>
        public int Page { get; private set; }

        public PageArgs() {}

        public PageArgs(int page)
        {
            if (page < 0) 
                throw new ArgumentOutOfRangeException("page", page, "Should be greater than zero");

            Page = page;
        }

        public virtual WebUrl AddTo(WebUrl url)
        {
            if (Page > 0) 
                url.AddPathSegment((Page + 1).ToString(CultureInfo.InvariantCulture));

            return url;
        }

        /// <summary>
        /// Tries to parse the request, ignoring the specified number of 
        /// arguments first. Useful when overloading 
        /// <see cref="TryParse(SocialToolBox.Core.Web.IWebRequest)"/>
        /// </summary>
        protected bool TryParse(IWebRequest request, int ignoreFirst)
        {
            var unmatched = request.UnmatchedPath;
            
            if (ignoreFirst == unmatched.Length)
            {
                Page = 0; 
                return true;
            }

            if (ignoreFirst + 1 != unmatched.Length) return false;

            var value = unmatched[ignoreFirst];
            int page;
            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out page))
                return false;

            Page = page - 1;
            return true;
        }

        public virtual bool TryParse(IWebRequest request)
        {
            return TryParse(request, 0);
        }
    }
}
