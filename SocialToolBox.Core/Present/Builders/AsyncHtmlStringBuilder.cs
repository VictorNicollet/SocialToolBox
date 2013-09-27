using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialToolBox.Core.Present.Builders
{
    /// <summary>
    /// Asynchronously builds an HTML string.
    /// </summary>
    public class AsyncHtmlStringBuilder
    {
        /// <summary>
        /// All the nodes currently in this builder that are being blocked by an 
        /// asynchronous task.
        /// </summary>
        private readonly Queue<Task<HtmlString>> _pending = new Queue<Task<HtmlString>>();

        /// <summary>
        /// The string builder used for concatenating everything.
        /// </summary>
        private readonly StringBuilder _builder = new StringBuilder();

        /// <summary>
        /// Returns whether the builder is currently synchronous (that is, there are no
        /// uncompleted tasks waiting to be appended).
        /// </summary>
        public bool IsSync
        {
            get
            {
                while (_pending.Count > 0 && _pending.Peek().IsCompleted)                
                    _builder.Append(_pending.Dequeue().Result);

                return _pending.Count == 0;
            }
        }

        /// <summary>
        /// Adds a string, with escaping.
        /// </summary>
        public void Add(string str)
        {
            if (IsSync) _builder.Append(HttpUtility.HtmlEncode(str));
            else _pending.Enqueue(Task.FromResult(HtmlString.Escape(str)));
        }

        /// <summary>
        /// Adds a string, without escaping.
        /// </summary>
        public void AddVerbatim(string str)
        {
            if (IsSync) _builder.Append(str);
            else _pending.Enqueue(Task.FromResult(HtmlString.Verbatim(str)));
        }

        /// <summary>
        /// Add an HTML string. 
        /// </summary>
        public void Add(HtmlString hstr)
        {
            if (IsSync) _builder.Append(hstr);
            else _pending.Enqueue(Task.FromResult(hstr));
        }

        /// <summary>
        /// Add a range of HTML strings.
        /// </summary>
        public void AddRange(IEnumerable<HtmlString> hstrs)
        {
            if (IsSync) foreach (var hstr in hstrs) _builder.Append(hstr);
            else _pending.Enqueue(Task.FromResult(HtmlString.Concat(hstrs)));
        }

        /// <summary>
        /// Add an asynchronous HTML string.
        /// </summary>
        public void Add(Task<HtmlString> hstrT)
        {
            if (!hstrT.IsCompleted)
            {
                hstrT.Start();
                _pending.Enqueue(hstrT);
            }
            else if (IsSync)
            {
                _builder.Append(hstrT.Result);
            }
            else
            {
                _pending.Enqueue(hstrT);
            }
            
        }

        /// <summary>
        /// Add a range of asynchronous HTML strings.
        /// </summary>
        public void AddRange(IEnumerable<Task<HtmlString>> hstrTs)
        {
            foreach (var hstrT in hstrTs) Add(hstrT);
        }

        /// <summary>
        /// Build the concatenation HTML string.
        /// </summary>
        /// <returns></returns>
        public async Task<HtmlString> Build()
        {
            while (_pending.Count > 0)
                _builder.Append(await _pending.Dequeue());

            return HtmlString.Verbatim(_builder.ToString());
        }
    }
}
