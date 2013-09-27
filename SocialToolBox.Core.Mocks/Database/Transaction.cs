using System;
using System.Collections.Generic;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Mocks.Database
{
    /// <summary>
    /// A read-write transaction that handles committing changes.
    /// </summary>
    public class Transaction : ITransaction
    {
        /// <summary>
        /// All the commit functions that must be called when committing.
        /// </summary>
        private readonly Dictionary<object, Action> _committers =
            new Dictionary<object, Action>(); 

        public void Commit()
        {
            foreach (var v in _committers.Values) v();
            _committers.Clear();
        }

        /// <summary>
        /// Register a commit function for the specified object on the
        /// transaction.
        /// </summary>
        public static void RegisterCommit(ITransaction it, object o, Action a)
        {
            var t = it as Transaction;
            if (t == null) return;
            if (t._committers.ContainsKey(o)) return;
            t._committers.Add(o,a);
        }
    }
}
