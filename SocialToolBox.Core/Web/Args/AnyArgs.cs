using System.Collections.Generic;
using System.Linq;

namespace SocialToolBox.Core.Web.Args
{
    /// <summary>
    /// An Url argument object that reads all unmatched values.
    /// </summary>
    public class AnyArgs : IWebUrlArgument
    {
        public string[] Values { get; private set; }

        public AnyArgs() { }

        public AnyArgs(IEnumerable<string> values)
        {
            Values = values.ToArray();
        }

        public AnyArgs(params string[] values)
        {
            Values = values;
        }

        public bool TryParse(IWebRequest request)
        {
            Values = request.UnmatchedPath;
            return true;
        }

        public WebUrl AddTo(WebUrl url)
        {
            foreach (var value in Values) url.AddPathSegment(value);
            return url;
        }
    }
}
