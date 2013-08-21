using System.Collections.Generic;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Mocks.Database
{
    public class DatabaseDriver : IDatabaseDriver
    {
        /// <summary>
        /// All event streams currently known to this database driver.
        /// </summary>
        public readonly Dictionary<string, EventStream> EventStreams;

        /// <summary>
        /// The mock type dictionary used internally.
        /// </summary>
        public readonly TypeDictionary InnerTypeDictionary;

        public DatabaseDriver()
        {
            EventStreams = new Dictionary<string, EventStream>();
        }

        public IEventStream GetEventStream(string name, bool createIfMissing)
        {
            EventStream stream;
            if (!EventStreams.TryGetValue(name, out stream) && createIfMissing)
            {
                stream = new EventStream(name, this);
                EventStreams.Add(name, stream);
            }

            return stream;
        }

        public ITypeDictionary TypeDictionary { get { return InnerTypeDictionary; } }
    }
}
