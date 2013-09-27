using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Mocks.Database
{
    /// <summary>
    /// A read-write transaction that handles committing changes.
    /// </summary>
    public class ProjectTransaction : IProjectTransaction
    {
        /// <summary>
        /// All the commit functions that must be called when committing.
        /// </summary>
        private readonly Dictionary<object, Action> _committers =
            new Dictionary<object, Action>(); 

// ReSharper disable CSharpWarnings::CS1998
        public async Task Commit()
// ReSharper restore CSharpWarnings::CS1998
        {
            foreach (var v in _committers.Values) v();
            _committers.Clear();
            Load = 0;
        }

        public int Load { get; private set; }

        /// <summary>
        /// Register a commit function for the specified object on the
        /// transaction. Increases load by the specified number.
        /// </summary>
        public static void RegisterCommit(IProjectTransaction it, object o, Action a, int load = 1)
        {
            var t = it as ProjectTransaction;
            if (t == null) return;
            t.Load += load;
            if (t._committers.ContainsKey(o)) return;
            t._committers.Add(o,a);
        }
    }
}
