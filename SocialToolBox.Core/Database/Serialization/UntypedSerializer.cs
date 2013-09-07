using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// Serializes and unserializes objects to binary blobs, but replaces
    /// the heavy type information with a simple integer. Internally uses
    /// a type dictionary for type-integer mapping.
    /// </summary>
    public class UntypedSerializer
    {
        /// <summary>
        /// The inner formatter used for serialization.
        /// </summary>
        private readonly BinaryFormatter _formatter;

        /// <summary>
        /// The type dictionary used by this serializer to perform 
        /// mappings.
        /// </summary>
        public readonly ITypeDictionary TypeDictionary;

        public UntypedSerializer(ITypeDictionary dictionary)
        {
            _formatter = new BinaryFormatter();
            TypeDictionary = dictionary;
        }

        /// <summary>
        /// Unserializes an object from a stream.
        /// </summary>
        public object Unserialize(Stream input)
        {
            return _formatter.Deserialize(input);            
        }

        /// <summary>
        /// Unserializes an object of a specific type from a byte sequence.
        /// Throws if it cannot return a non-null object.
        /// </summary>
        public T Unserialize<T>(byte[] input) where T : class
        {
            using (var stream = new MemoryStream(input))
            {
                var obj = Unserialize(stream);
                return (T) obj;
            }
        }

        /// <summary>
        /// Unserializes an object of a specific type from a byte sequence.
        /// Throws if unserialization fails, returns <code>null</code>
        /// if object was not of expected type.
        /// </summary>
        public T UnserializeOfType<T>(byte[] input) where T : class
        {
            using (var stream = new MemoryStream(input))
            {
                var obj = Unserialize(stream);
                return obj as T;
            }
        }

        /// <summary>
        /// Serializes an object to a stream.
        /// </summary>
        public void Serialize(object input, Stream output)
        {
            _formatter.Serialize(output, input);
        }

        /// <summary>
        /// Serialize an object to a byte sequence.
        /// </summary>
        public byte[] Serialize(object input)
        {
            using (var stream = new MemoryStream())
            {
                Serialize(input, stream);
                return stream.ToArray();
            }
        }
    }
}
