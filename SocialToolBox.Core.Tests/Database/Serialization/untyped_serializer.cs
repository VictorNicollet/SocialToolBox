using NUnit.Framework;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Database.Serialization
{
    [TestFixture]
    public class untyped_serializer
    {
        [SetUp]
        public void SetUp()
        {
            var dictionary = new TypeDictionary();
            _serializer = new UntypedSerializer(dictionary);
        }

        [Test]
        public void serialize_and_unserialize()
        {
            var original = Mock;
            var bytes = _serializer.Serialize(original);
            var copy = _serializer.Unserialize<MockAccount>(bytes);

            Assert.AreEqual(original, copy);
        }

        private UntypedSerializer _serializer;

        private MockAccount Mock 
        {
            get
            {
                return MockAccount.Bob;
            }
        }
    }
}
