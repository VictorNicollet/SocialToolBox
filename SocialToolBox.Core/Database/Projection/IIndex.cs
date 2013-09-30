using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// A read-only index interface maps set and sort keys to identifiers.
    /// </summary>
    public interface IIndex<in TSet, TSort>
        where TSet : class
        where TSort : class
    {
        /// <summary>
        /// The number of elements with a specific set key.
        /// </summary>
        Task<int> Count(TSet set, IReadCursor cursor);

        /// <summary>
        /// Queries a set of values, up to the count, discarding
        /// the specified offset first, and staying between the min
        /// and max values (inclusive).
        /// </summary>
        Task<IEnumerable<IPair<TSort, Id>>> Query(
            TSet set, IReadCursor cursor, int count,
            int offset = 0,
            TSort maxValue = null,
            TSort minValue = null);
    }
}
