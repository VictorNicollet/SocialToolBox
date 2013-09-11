using System;
using System.IO;
using NUnit.Framework;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Database.Serialization
{
    [TestFixture]
    public class untyped_serializer
    {
        public byte[] SerializeWithType(Type t, object value)
        {
            var stream = new MemoryStream();
            _serializer.SerializeWithType(t, value, stream);
            return stream.ToArray();
        }

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

        [Test]
        public void serialized_size()
        {
            var original = Mock;
            var bytes = _serializer.Serialize(original);

            Assert.AreEqual(44, bytes.Length);
        }

        [Test]
        public void serialized_content()
        {
            var original = Mock;
            var bytes = _serializer.Serialize(original);
            CollectionAssert.AreEqual(
                new[] {
                    // Id of type MockAccount
                    0,
                    // Bob
                    1,
                    // Bob.Name
                    4, 66, 111, 98,
                    // Bob.Password
                    1,
                    // Bob.Password.BcryptIterationCount
                    10, 0, 0, 0, 
                    // Bob.Password.Hash
                    33, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97,
                        97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97
                }, bytes);           
        }

        [Test]
        public void serialize_with_type_string()
        {
            var bytes = SerializeWithType(typeof (string), "H€llo");
            CollectionAssert.AreEqual(
                new[] {8, 72, 226, 130, 172, 108, 108, 111},
                bytes);        
        }

        [Test]
        public void serialize_with_type_string_null()
        {
            var bytes = SerializeWithType(typeof(string), null);
            CollectionAssert.AreEqual(
                new[] { 0 },
                bytes);
        }

        [Test]
        public void serialize_with_type_string_array()
        {
            var bytes = SerializeWithType(typeof(string[]), new[]{"a","b"});
            CollectionAssert.AreEqual(
                new[] { 3, 2, 97, 2, 98 },
                bytes);
        }

        [Test]
        public void serialize_with_type_string_array_null()
        {
            var bytes = SerializeWithType(typeof(string[]), null);
            CollectionAssert.AreEqual(
                new[] { 0 },
                bytes);
        }

        [Test]
        public void serialize_with_type_string_multi_array()
        {
            var bytes = SerializeWithType(typeof(string[][]), new[] { new[]{ "a" }, new[]{ "a", "b" } });
            CollectionAssert.AreEqual(
                new[] { 3, 2, 2, 97, 3, 2, 97, 2, 98 },
                bytes);
        }

        [Test]
        public void serialize_with_type_mock_account()
        {
            var bytes = SerializeWithType(typeof (MockAccount), MockAccount.Bob);
            CollectionAssert.AreEqual(
                new[] {
                    1,
                    // .Name
                    4, 66, 111, 98,
                    // .Password
                    1,
                    // .Password.BcryptIterationCount
                    10, 0, 0, 0, 
                    // .Password.Hash
                    33, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97,
                        97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97, 97
                }, bytes);
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
