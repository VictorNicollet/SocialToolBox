using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.EventStream
{
    /// <summary>
    /// An object that iterates through multiple streams at once,
    /// yielding objects of a specified type.
    /// </summary>
    public interface IMultiStreamIterator<T> : IEnumerable<T> where T : class
    {
        /// <summary>
        /// The current vector clock of this iterator. Read-only,
        /// though modified internally by the iterator.
        /// </summary>
        VectorClock VectorClock { get; }

        /// <summary>
        /// Grab the next element in the iterator asynchronously, or
        /// <code>null</code> if no values are left.
        /// </summary>
        Task<T> NextAsync();

        /// <summary>
        /// Grab the next element in the iterator, or <code>null</code>
        /// if no values are left.
        /// </summary>
        T Next();
    }
}
