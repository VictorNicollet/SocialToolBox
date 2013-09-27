using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialToolBox.Core.Present
{
    /// <summary>
    /// Asynchronously builds an HTML string.
    /// </summary>
    public class HtmlOutput
    {
        /// <summary>
        /// All the nodes currently in this builder that are being blocked by an 
        /// asynchronous task.
        /// </summary>
        private readonly Queue<Task<Action<StringBuilder>>> _pending = 
            new Queue<Task<Action<StringBuilder>>>();

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
                    _pending.Dequeue().Result(_builder);

                return _pending.Count == 0;
            }
        }

        /// <summary>
        /// Adds a string, with escaping.
        /// </summary>
        public void Add(string str)
        {
            if (IsSync) DoAdd(str)(_builder);
            else _pending.Enqueue(Task.FromResult(DoAdd(str)));
        }

        /// <summary>
        /// Performs the action behind <see cref="Add(string)"/>.
        /// </summary>
        private static Action<StringBuilder> DoAdd(string str)
        {
            return b => b.Append(HttpUtility.HtmlEncode(str));
        }

        /// <summary>
        /// Adds a string, without escaping.
        /// </summary>
        public void AddVerbatim(string str)
        {
            if (IsSync) _builder.Append(str);
            else _pending.Enqueue(Task.FromResult(DoAddVerbatim(str)));
        }

        /// <summary>
        /// Performs the action behind <see cref="AddVerbatim"/>.
        /// </summary>
        private static Action<StringBuilder> DoAddVerbatim(string str)
        {
            return b => b.Append(str);
        }

        /// <summary>
        /// Add an HTML string. 
        /// </summary>
        public void Add(HtmlString hstr)
        {
            if (IsSync) _builder.Append(hstr);
            else _pending.Enqueue(Task.FromResult(DoAdd(hstr)));
        }

        /// <summary>
        /// Performs the action behind <see cref="Add(HtmlString)"/>.
        /// </summary>
        private static Action<StringBuilder> DoAdd(HtmlString hstr)
        {
            return b => b.Append(hstr);
        }

        /// <summary>
        /// Add a range of HTML strings.
        /// </summary>
        public void AddRange(IEnumerable<HtmlString> hstrs)
        {
            if (IsSync) foreach (var hstr in hstrs) _builder.Append(hstr);
            else _pending.Enqueue(Task.FromResult(DoAdd(HtmlString.Concat(hstrs))));
        }

        /// <summary>
        /// Used by <see cref="Insert"/> to delegate rendering to a distinct builder, 
        /// which is then inserted when the task finishes. This starts running the inner task
        /// immediately.
        /// </summary>
        private async Task<Action<StringBuilder>> RunInsertLater(Func<HtmlOutput, Task> renderer)
        {
            var newOutput = new HtmlOutput();
            await StartTask(renderer(newOutput));
            // ReSharper disable RedundantLambdaParameterType
            return (StringBuilder b) => b.Append(newOutput._builder);
            // ReSharper restore RedundantLambdaParameterType
        }

        /// <summary>
        /// Add an asynchronous HTML string.
        /// </summary>
        public void Insert(Func<HtmlOutput,Task> renderer)
        {
            _pending.Enqueue(RunInsertLater(renderer));
        }

        /// <summary>
        /// Build the concatenation HTML string.
        /// </summary>
        /// <returns></returns>
        public HtmlString Build()
        {
            while (_pending.Count > 0)
                _pending.Dequeue().Result(_builder);

            return HtmlString.Verbatim(_builder.ToString());
        }

        /// <summary>
        /// Makes sure a task is either running or completed, and returns it.
        /// </summary>
        private static Task StartTask(Task t)
        {
            if (t.Status == TaskStatus.Created) t.Start();
            return t;
        }
    }
}
