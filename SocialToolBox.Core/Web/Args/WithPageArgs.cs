using System;
using System.Globalization;

namespace SocialToolBox.Core.Web.Args
{
    /// <summary>
    /// Serializes a page number. On the C# side, page numbers are zero-based. On the web side, 
    /// the first page is page 1, and "1" is not serialized as a segment (though it is accepted). 
    /// </summary>
    public abstract class WithPageArgs : IWebUrlArgument
    {
        /// <summary>
        /// Zero-based page number. 
        /// </summary>
        public int Page { get; private set; }

        protected WithPageArgs() {}

        protected WithPageArgs(int page)
        {
            if (page < 0) 
                throw new ArgumentOutOfRangeException("page", page, "Should be greater than zero");

            Page = page;
        }

        protected WithPageArgs(WithPageArgs other)
        {
            Page = other.Page;
        }

        /// <summary>
        /// Append the page number to the URL. By default, 1 is never appended
        /// unless forced.
        /// </summary>        
        protected void AppendTo(WebUrl url, bool forced = false)
        {
            if (Page > 0 || forced) 
                url.AddPathSegment((Page + 1).ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Tries to parse the request, starting at the specified index.
        /// <see cref="TryParse(SocialToolBox.Core.Web.IWebRequest)"/>
        /// </summary>
        protected bool TryParsePage(IWebRequest request, ref int startAt)
        {
            var unmatched = request.UnmatchedPath;
            
            if (startAt == unmatched.Length)
            {
                Page = 0; 
                return true;
            }

            if (startAt + 1 != unmatched.Length) return false;

            var value = unmatched[startAt];
            int page;
            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out page))
                return false;

            if (page < 1) return false;

            Page = page - 1;
            startAt++;
            return true;
        }

        public abstract WebUrl AddTo(WebUrl url);
        public abstract bool TryParse(IWebRequest request);        
    }
}
