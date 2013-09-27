using System.Collections.Generic;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;

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

        /// <summary>
        /// The mock clock registry used internally.
        /// </summary>
        public readonly ClockRegistry InnerClockRegistry;

        /// <summary>
        /// The actual projection engine.
        /// </summary>
        public readonly ProjectionEngine InnerProjectionEngine;

        public DatabaseDriver()
        {
            EventStreams = new Dictionary<string, EventStream>();
            InnerTypeDictionary = new TypeDictionary();
            InnerClockRegistry = new ClockRegistry();
            InnerProjectionEngine = new ProjectionEngine(this);
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

        public IClockRegistry ClockRegistry { get { return InnerClockRegistry; } }        

        public ProjectionEngine Projections { get { return InnerProjectionEngine;  } }

        public IProjection<TEv> CreateProjection<TEv>(string name) where TEv : class
        {
            return new Projection<TEv>(name, this);            
        }

        public IReadTransaction StartReadTransaction()
        {
            return new ReadTransaction();
        }

        public IProjectTransaction StartProjectorTransaction()
        {
            return new ProjectTransaction();
        }

        public ITransaction StartReadWriteTransaction()
        {
            return new Transaction();
        }
    }
}
