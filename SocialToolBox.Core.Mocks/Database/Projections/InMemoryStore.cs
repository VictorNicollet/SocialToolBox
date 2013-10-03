using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Async;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Core.Mocks.Database.Projections
{
    /// <summary>
    /// An in-memory implementation of <see cref="IWritableStore{T}"/>
    /// </summary>
    public class InMemoryStore<T> : IWritableStore<T> where T : class
    {
        public readonly Dictionary<Id, byte[]> Contents = new Dictionary<Id,byte[]>();

        public IDatabaseDriver Driver { get; private set; }

        public readonly UntypedSerializer Serializer;

        public InMemoryStore(IDatabaseDriver driver)
        {
            Driver = driver;
            Serializer = new UntypedSerializer(driver.TypeDictionary);
        } 

        /// <summary>
        /// A lock for avoiding multi-thread collisions.
        /// </summary>
        private readonly AsyncLock _lock = new AsyncLock();

        public async Task<T> Get(Id id, IReadCursor cursor)
        {
            byte[] bytes;
            
            using (await _lock.Lock())
            {
                if (!Contents.TryGetValue(id, out bytes)) return null;
            }
            
            return Serializer.Unserialize<T>(bytes);
        }

        public event ValueChangedEvent<T> ValueChanged;

        public async Task Set(Id id, T newValue, IProjectCursor cursor)
        {
            byte[] oldBytes;
            var bytes = newValue == null ? null : Serializer.Serialize(newValue);
            
            using (await _lock.Lock())
            {
                Contents.TryGetValue(id, out oldBytes);
                Contents.Remove(id);
                if (newValue != null) Contents.Add(id, bytes);
            }

            if (ValueChanged != null)
            {
                var oldValue = oldBytes == null ? null : Serializer.Unserialize<T>(oldBytes);

                // Run the event after setting the new value AND releasing the lock
                ValueChanged(new ValueChangedEventArgs<T>(id, oldValue, newValue, cursor));
            }
        }
    }
}
