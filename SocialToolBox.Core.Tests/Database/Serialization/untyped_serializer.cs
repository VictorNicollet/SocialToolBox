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
        public void setUp()
        {
            var dictionary = new TypeDictionary();
            Serializer = new UntypedSerializer(dictionary);
        }

        [Test]
        public void serialize_and_unserialize()
        {
            var original = Mock;
            var bytes = Serializer.Serialize(original);
            var copy = Serializer.Unserialize<MockAccount>(bytes);

            Assert.AreEqual(original, copy);
        }

        private UntypedSerializer Serializer;

        private MockAccount Mock 
        {
            get
            {
                return new MockAccount
                {
                    Name = "Bob",
                    Password = new MockAccount.HashedPassword
                    {
                        BcryptIterationCount = 10,
                        Hash = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
                    }
                };
            }
        }
    }
}
