using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Core.Tests
{
    /// <summary>
    /// Provides base functionality for testing event readers.
    /// </summary>
    public class EventReaderFixture<TEv,TReader> 
        where TReader : class, IEventReader<TEv>, new()
        where TEv : class
    {
        public TReader Value;
        private UntypedSerializer _serializer;

        private T ThroughSerializer<T>(T input) where T : class
        {
            return _serializer.Unserialize<T>(_serializer.Serialize(input));
        }

        [SetUp]
        public void SetUp()
        {
            var td = new TypeDictionary();
            _serializer = new UntypedSerializer(td);
            Value = ThroughSerializer(new TReader());
        }

        public void After(params TEv[] events)
        {
            foreach (var ev in events)
            {
                Value.Read(ThroughSerializer(ev));
                Value = ThroughSerializer(Value);
            }
        }
    }
}
