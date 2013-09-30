using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialToolBox.Core.Async;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Mocks.Database.Projections
{
    /// <summary>
    /// An in-memory implementation of <see cref="IWritableIndex{TSet,TSort}"/>.
    /// </summary>
    public class InMemoryIndex<TSet, TSort> : IWritableIndex<TSet, TSort>
        where TSet : class
        where TSort : class
    {
        /// <summary>
        /// A line in the sorted index array.
        /// </summary>
        private class IndexLine
        {
            public readonly TSet Set;
            public readonly TSort Sort;
            public readonly Id Id;

            public IndexLine(TSet set, TSort sort, Id id)
            {
                Set = set;
                Sort = sort;
                Id = id;
            }
        }

        /// <summary>
        /// The index itself.
        /// </summary>
        private readonly List<IndexLine> _index = new List<IndexLine>();

        /// <summary>
        /// An index line comparison class, compares based on set,
        /// then based on sort.
        /// </summary>
        private class Comparison : IComparer<IndexLine>
        {
            public readonly IndexKeyComparer<TSet> SetComparer =
                new IndexKeyComparer<TSet>();

            public readonly IndexKeyComparer<TSort> SortComparer = 
                new IndexKeyComparer<TSort>();

            public int Compare(IndexLine x, IndexLine y)
            {
                var set = SetComparer.Compare(x.Set, y.Set);
                if (set != 0) return set;

                return SortComparer.Compare(x.Sort, y.Sort);
            }
        }

        /// <summary>
        /// An instance of the comparer.
        /// </summary>
        private readonly Comparison _comparer = new Comparison();

        /// <summary>
        /// An asynchronous lock, to avoid writing and reading at the same time.
        /// </summary>
        private readonly AsyncLock _lock = new AsyncLock();

        public async Task<int> Count(TSet set, IReadCursor cursor)
        {
            using (await _lock.Lock())
            {
                return _index.Count(l => _comparer.SetComparer.Compare(l.Set, set) == 0);
            }
        }

        public async Task<IEnumerable<KeyValuePair<TSort, Id>>> Query(TSet set, IReadCursor cursor, int count, int offset = 0, TSort minValue = null, TSort maxValue = null)
        {
            using (await _lock.Lock())
            {
                var inSet = _index.Where(l => _comparer.SetComparer.Compare(l.Set, set) == 0);

                if (minValue != null)
                    inSet = inSet.Where(l => _comparer.SortComparer.Compare(l.Sort, minValue) >= 0);

                if (maxValue != null)
                    inSet = inSet.Where(l => _comparer.SortComparer.Compare(l.Sort, maxValue) <= 0);

                var filtered = inSet.ToArray();
                Array.Sort(filtered, _comparer);

                return filtered                    
                    .Skip(offset).Take(count)
                    .Select(l => new KeyValuePair<TSort, Id>(l.Sort, l.Id));
            }
        }

        public async Task Delete(Id id, IProjectCursor c)
        {
            using (await _lock.Lock())
            {
                _index.RemoveAll(l => l.Id.Equals(id));
            }
        }

        public async Task DeleteSet(Id id, TSet set, IProjectCursor c)
        {
            using (await _lock.Lock())
            {
                _index.RemoveAll(l => l.Id.Equals(id)
                                      && _comparer.SetComparer.Compare(set, l.Set) == 0);
            }
        }

        public async Task Add(Id id, TSet set, TSort sort, IProjectCursor c)
        {
            using (await _lock.Lock())
            {
                _index.Add(new IndexLine(set, sort, id));
            }
        }

        public async Task Add(Id id, IEnumerable<IPair<TSet, TSort>> bindings, IProjectCursor c)
        {
            using (await _lock.Lock())
            {
                _index.AddRange(bindings.Select(p => new IndexLine(p.First, p.Second, id)));
            }
        }

        public async Task Set(Id id, TSet set, TSort sort, IProjectCursor c)
        {
            using (await _lock.Lock())
            {
                var pos = _index.FindIndex(l => l.Id.Equals(id));
                if (pos == -1) _index.Add(new IndexLine(set, sort, id));
                else _index[pos] = new IndexLine(set, sort, id);
            }
        }
    }
}
