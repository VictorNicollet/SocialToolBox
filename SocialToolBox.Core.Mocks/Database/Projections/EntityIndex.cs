using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Mocks.Database.Projections
{
    /// <summary>
    /// An in-memory entity index implementation.
    /// </summary>
    public class EntityIndex<TSet,TSort,TEntity> : IEntityIndex<TSet,TSort,TEntity>
        where TSet : class
        where TSort : class 
        where TEntity : class
    {
        /// <summary>
        /// The entities currently stored in this entity store.
        /// </summary>
        public readonly Dictionary<Id, TEntity> Entities =
            new Dictionary<Id, TEntity>();

        /// <summary>
        /// The index emitter, received upon creation.
        /// </summary>
        public readonly Func<Id, TEntity, IEnumerable<IPair<TSet, TSort>>> Emitter; 

        /// <summary>
        /// A line in the index array. Sortable for easier searching.
        /// </summary>
        public class IndexLine
        {
            /// <summary>
            /// The identifier of the entity which generated this line.
            /// </summary>
            public readonly Id Id;

            /// <summary>
            /// The set key for this line.
            /// </summary>
            public readonly TSet SetKey;

            /// <summary>
            /// The sort key for this line.
            /// </summary>
            public readonly TSort SortKey;

            public IndexLine(Id id, TSet setKey, TSort sortKey)
            {
                Id = id;
                SetKey = setKey;
                SortKey = sortKey;
            }
        }

        /// <summary>
        /// All index lines currently in this index.
        /// </summary>
        public readonly List<IndexLine> Index = new List<IndexLine>();

        /// <summary>
        /// Is the index sorted ?
        /// </summary>
        private bool _isIndexSorted;

        /// <summary>
        /// Comparer for the set key type.
        /// </summary>
        public readonly IComparer<TSet> SetComparer = new IndexKeyComparer<TSet>();
 
        /// <summary>
        /// Comparer for the sort key type.
        /// </summary>
        public readonly IComparer<TSort> SortComparer = new IndexKeyComparer<TSort>();

        /// <summary>
        /// Compare two index lines.
        /// </summary>
        public int Compare(IndexLine a, IndexLine b)
        {
            var set = SetComparer.Compare(a.SetKey, b.SetKey);
            if (set != 0) return set;

            var sort = SortComparer.Compare(a.SortKey, b.SortKey);
            if (sort != 0) return sort;

            return 0;
        }

        /// <summary>
        /// Updates the entity with the provided identifier.
        /// </summary>
        public void Update(Id id, TEntity entity)
        {
            if (Entities.ContainsKey(id))
                // The entity already exists, so remove any items from the index
                Index.RemoveAll(line => line.Id.Equals(id));

            Entities.Remove(id);

            if (entity != null)
            {
                Entities.Add(id, entity);

                foreach (var pair in Sets(id, entity))
                {
                    _isIndexSorted = false;
                    Index.Add(new IndexLine(id, pair.First, pair.Second));
                }
            }
        }

        /// <summary>
        /// Sprts the index, if it is not already sorted, and returns it.
        /// </summary>
        public IList<IndexLine> SortedIndex
        {
            get
            {
                if (!_isIndexSorted)
                    Index.Sort(Compare);
                return Index;
            }
        }

        public EntityIndex(Func<Id, TEntity, IEnumerable<IPair<TSet, TSort>>> emitter)
        {
            Emitter = emitter;
        }

        public IEnumerable<IPair<TSet, TSort>> Sets(Id id, TEntity entity)
        {
            return Emitter(id, entity);
        }

        public async Task<TEntity> Get(Id id)
        {
            await Task.Yield();

            TEntity result;
            Entities.TryGetValue(id, out result);
            return result;
        }

        public async Task<IDictionary<Id, TEntity>> GetAll(IEnumerable<Id> ids)
        {
            var result = new Dictionary<Id, TEntity>();

            foreach (var id in ids)
            {
                if (result.ContainsKey(id)) continue;
                var entity = await Get(id);
                if (entity != null) result.Add(id, entity);
            }

            return result;
        }

        public async Task<IPair<Id, TEntity>[]> Query(TSet set, int count, int offset = 0, 
            TSort greaterThan = null, TSort lessThan = null, bool @descending = false)
        {
            await Task.Yield();

            var inSet = Index.Where(line => SetComparer.Compare(line.SetKey,set) == 0);

            if (greaterThan != null)
                inSet = inSet.Where(line => SortComparer.Compare(line.SortKey,greaterThan) >= 0);

            if (lessThan != null)
                inSet = inSet.Where(line => SortComparer.Compare(line.SortKey,lessThan) <= 0);

            if (descending)
                inSet = inSet.Reverse();

            return inSet.Skip(offset).Take(count).Select(line => line.Id)
                .Select(id =>
                {
                    TEntity result;
                    Entities.TryGetValue(id, out result);
                    return Pair.Make(id, result);
                })
                .Where(id => id.Second != null)
                .ToArray();
        }
    }
}
