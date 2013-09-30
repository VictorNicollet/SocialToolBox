using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Web.Args;

namespace SocialToolBox.Core.Database.Projection
{
    /// <summary>
    /// Extensions to <see cref="IIndex{TSet,TSort}"/> and
    /// <see cref="IWritableIndex{TSet,TSort}"/>.
    /// </summary>
    public static class IndexExtensions
    {
        /// <summary>
        /// Run a query on an index with no set key. A simplification of <see cref="IIndex{TSet,TSort}.Query"/>
        /// </summary>
        public static Task<IEnumerable<KeyValuePair<TSort, Id>>> Query<TSort>(
            this IIndex<NoKey, TSort> index, IReadCursor cursor, int count,
            int offset = 0,
            TSort minValue = null,
            TSort maxValue = null)
        
            where TSort : class
        {
            return index.Query(new NoKey(), cursor, count, offset, minValue, maxValue);
        }

        /// <summary>
        /// Counts the items in an index with no set key. A simplification of 
        /// <see cref="IIndex{TSet,TSort}.Count"/>.
        /// </summary>
        public static Task<int> Count<TSort>(this IIndex<NoKey, TSort> index, IReadCursor cursor)
            where TSort : class
        {
            return index.Count(new NoKey(), cursor);
        }

        /// <summary>
        /// Adds a binding to an index with no set key. A simplification of 
        /// <see cref="IWritableIndex{TSet,TSort}.Add(SocialToolBox.Core.Database.Id,TSet,TSort,SocialToolBox.Core.Database.IProjectCursor)"/>
        /// </summary>
        public static Task Add<TSort>(
            this IWritableIndex<NoKey, TSort> index, Id id, TSort sort, IProjectCursor cursor)
            
            where TSort : class
        {
            return index.Add(id, new NoKey(), sort, cursor);
        }

        /// <summary>
        /// Sets a binding in an index with no set key. A simplification of 
        /// <see cref="IWritableIndex{TSet,TSort}.Set"/>
        /// </summary>
        public static Task Set<TSort>(
            this IWritableIndex<NoKey, TSort> index, Id id, TSort sort, IProjectCursor cursor)

            where TSort : class
        {
            return index.Set(id, new NoKey(), sort, cursor);
        }
    }
}
