using System.Collections.Generic;
using System.Threading.Tasks;
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

// ReSharper disable CSharpWarnings::CS1998
        public async Task<T> Get(Id id, IReadCursor cursor)
// ReSharper restore CSharpWarnings::CS1998
        {
            byte[] value;
            if (!Contents.TryGetValue(id, out value)) return null;

            return Serializer.Unserialize<T>(value);
        }

// ReSharper disable CSharpWarnings::CS1998
        public async Task Set(Id id, T item, IProjectCursor cursor)
// ReSharper restore CSharpWarnings::CS1998
        {
            Contents.Remove(id);
            if (item == null) return;
            Contents.Add(id,Serializer.Serialize(item));         
        }
    }
}
