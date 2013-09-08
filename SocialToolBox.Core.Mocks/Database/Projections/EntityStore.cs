using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;

namespace SocialToolBox.Core.Mocks.Database.Projections
{
    public class EntityStore<T> : IEntityStore<T> where T : class
    {
        /// <summary>
        /// The entities currently stored in this entity store.
        /// </summary>
        public readonly Dictionary<Id, T> Entities;

        public EntityStore()
        {
            Entities = new Dictionary<Id, T>();
        }

        public async Task<T> Get(Id id)
        {
            await Task.Yield();

            T result;
            Entities.TryGetValue(id, out result);
            return result;
        }

        public async Task<IDictionary<Id, T>> GetAll(IEnumerable<Id> ids)
        {
            var result = new Dictionary<Id, T>();

            foreach (var id in ids)
            {
                if (result.ContainsKey(id)) continue;
                var entity = await Get(id);
                if (entity != null) result.Add(id, entity);
            }

            return result;
        }

        public async Task<bool> Exists(Id id)
        {
            await Task.Yield();
            return Entities.ContainsKey(id);
        }
    }
}
