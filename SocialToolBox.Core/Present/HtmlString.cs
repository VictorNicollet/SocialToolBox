using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// An arbitrary piece of HTML. Can also appear as a page node.
    /// </summary>
    public class HtmlString : IPageNode
    {
        /// <summary>
        /// The actual HTML string.
        /// </summary>
        private readonly string _value;

        public override string ToString()
        {
            return _value;
        }

        private HtmlString(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Escape a piece of string, turning it into an HTML string.
        /// </summary>
        public static HtmlString Escape(string str)
        {
            return new HtmlString(HttpUtility.HtmlEncode(str));
        }

        /// <summary>
        /// Keep a piece of string as-is.
        /// </summary>
        public static HtmlString Verbatim(string str)
        {
            return new HtmlString(str);
        }

        /// <summary>
        /// Concatenate a sequence of HTML strings.
        /// </summary>
        public static HtmlString Concat(IEnumerable<HtmlString> strings)
        {
            return new HtmlString(string.Concat(strings.Select(h => h.ToString())));
        }

        /// <summary>
        /// Concatenate a sequence of HTML strings.
        /// </summary>
        public static HtmlString Concat(params HtmlString[] strings)
        {
            return Concat((IEnumerable<HtmlString>)strings);
        }

        /// <summary>
        /// A simple formatting tool. Does not handle anything beyond simple
        /// ToString.
        /// </summary>
        public static HtmlString Format(string format, params object[] args)
        {
            var escaped = args.Select(s =>
            {
                if (s is HtmlString) return (object)s.ToString();
                return (object)HttpUtility.HtmlEncode(s.ToString());
            }).ToArray();

            return new HtmlString(string.Format(format, escaped));
        }

        public Task<HtmlString> RenderWith(INodeRenderer visitor)
        {
            return visitor.Render(this);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as HtmlString;
            if (other == null) return false;
            return string.Equals(_value, other._value, StringComparison.InvariantCulture);
        }
    }
}
