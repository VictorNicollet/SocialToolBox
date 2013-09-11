using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

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
        /// The type dictionary used by this serializer to perform 
        /// mappings.
        /// </summary>
        public readonly ITypeDictionary TypeDictionary;

        public UntypedSerializer(ITypeDictionary dictionary)
        {
            TypeDictionary = dictionary;
        }

        #region Typed serialization

        /// <summary>
        /// Serializes an arbitrary object to a stream, assumes the object type
        /// will be determined in another way.
        /// </summary>
        public void SerializeWithType(Type t, object serialized, Stream output)
        {
            if (SerializeCustom(t, serialized, output))
                return;

            // Arrays are handled as normal sequences
            if (t.IsArray)
            {
                var newRank = t.GetArrayRank() - 1;
                var innerType = t.GetElementType();
                var withNewRank = newRank == 0 ? innerType : innerType.MakeArrayType(newRank);
                var array = serialized as Array;

                output.EncodeSequence((s,x) => SerializeWithType(withNewRank,x,s), array);
                return;
            }

            // Strings are a very frequent special case
            var asString = serialized as string;
            if (null != asString || serialized == null && t == typeof (string))
            {
                output.EncodeString(asString);
                return;
            }

            // All other primitive types are delegated to a different function
            if (t.IsPrimitive)
            {
                SerializePrimitive(t, serialized, output);
                return;
            }

            // Persistent classes are written member by member.
            if (t.GetCustomAttribute<PersistAttribute>() != null)
            {
                SerializePersistent(t, serialized, output);
                return;
            }

            throw new SerializationException(
                string.Format("Type {0} cannot be serialized.", t));
        }

        /// <summary>
        /// Attempts to serialize a value with a custom serializer, if any
        /// exists. If none exist, does nothing and returns false.
        /// </summary>
        private bool SerializeCustom(Type t, object serialized, Stream output)
        {
            ICustomSerializer custom;
            if (CustomSerializers.TryGetValue(t, out custom))
            {
                custom.Serialize(this, serialized, output);
                return true;
            }
            
            // Generic classes get special case-by-case treatment.
            if (t.IsGenericType)
            {
                var typedef = t.GetGenericTypeDefinition();
                var typeargs = t.GetGenericArguments();

                if (CustomSerializers.TryGetValue(typedef, out custom))
                {
                    custom.Serialize(this, serialized, output, typeargs);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serialize the members of a persistent object.
        /// </summary>
        private void SerializePersistent(Type t, object serialized, Stream output)
        {
            // Write whether the inner object is null or not, as a single byte.
            if (serialized == null)
            {
                output.WriteByte(0);
                return;
            }

            output.WriteByte(1);

            // Make sure the object is actually of the specified type
            if (t != serialized.GetType())
                throw new SerializationException(
                    string.Format("Cannot serialize object of type {0} as if it were type {1}",
                        serialized.GetType(), t));

            // Process individual members
            var members = GetPersistentTypeMembers(t);

            foreach (var member in members)
            {
                Type memberType;
                object value;

                switch (member.Key.MemberType)
                {
                    case MemberTypes.Property:
                        var asProperty = (PropertyInfo) member.Key;
                        value = asProperty.GetValue(serialized);
                        memberType = asProperty.PropertyType;
                        break;
                    case MemberTypes.Field:
                        var asField = (FieldInfo) member.Key;
                        value = asField.GetValue(serialized);
                        memberType = asField.FieldType;
                        break;
                    default:
                        // Attribute PersistMember should not be placed on
                        // anything else.
                        Debug.Assert(false);
                        return;
                }

                // Polymorphic members will be serialized by including the type.
                if (member.Value.Polymorphic || memberType.IsAbstract || memberType.IsInterface)
                {
                    Serialize(value, output);
                }
                else
                {
                    SerializeWithType(memberType, value, output);
                }
            }
        }

        public object UnserializeWithType(Type t, Stream input)
        {
            object custom;
            if (UnserializeCustom(t, input, out custom))
                return custom;
            
            // Arrays are handled as normal sequences
            if (t.IsArray)
            {
                var newRank = t.GetArrayRank() - 1;
                var innerType = t.GetElementType();
                var withNewRank = newRank == 0 ? innerType : innerType.MakeArrayType(newRank);

                return input.DecodeSequence(s => UnserializeWithType(withNewRank, s)).ToArray();
            }

            // Strings are a very frequent special case
            if (t == typeof (string))
                return input.DecodeString();
            
            // All other primitive types are delegated to a different function
            if (t.IsPrimitive)
                return UnserializePrimitive(t, input);
                
            // Persistent classes are written member by member.
            if (t.GetCustomAttribute<PersistAttribute>() != null)
            {
                var isNotNull = input.ReadByte() != 0;
                
                if (!isNotNull) return null;

                // Create a default instance in order to populate it.
                var constructor = t.GetConstructor(new Type[] {});
                if (null ==  constructor)
                    throw new SerializationException(
                        string.Format("Type {0} has no default constructor.", t));

                var result = constructor.Invoke(new object[]{});

                // Process individual members one by one to populate the object
                var members = GetPersistentTypeMembers(t);

                foreach (var member in members)
                {
                    Type memberType;
                    object value;

                    switch (member.Key.MemberType)
                    {
                        case MemberTypes.Property:
                            var asProperty = (PropertyInfo)member.Key;
                            memberType = asProperty.PropertyType;
                            
                            value =
                                (member.Value.Polymorphic || memberType.IsAbstract || memberType.IsInterface)
                                    ? Unserialize(input)
                                    : UnserializeWithType(memberType, input);

                            asProperty.SetValue(result, value);
                            
                            break;

                        case MemberTypes.Field:
                            var asField = (FieldInfo)member.Key;
                            memberType = asField.FieldType;

                            value =
                                (member.Value.Polymorphic || memberType.IsAbstract || memberType.IsInterface)
                                    ? Unserialize(input)
                                    : UnserializeWithType(memberType, input);
                            
                            asField.SetValue(result, value);

                            break;
                        default:
                            // Attribute PersistMember should not be placed on
                            // anything else.
                            Debug.Assert(false);
                            break;
                    }
                }

                return result;
            }

            throw new SerializationException(
                string.Format("Type {0} cannot be unserialized.", t));
        }

        /// <summary>
        /// Attempts to serialize a value with a custom serializer, if any
        /// exists. If none exist, does nothing and returns false.
        /// </summary>
        private bool UnserializeCustom(Type t, Stream input, out object result)
        {
            ICustomSerializer custom;
            if (CustomSerializers.TryGetValue(t, out custom))
            {
                result = custom.Unserialize(this, input);
                return true;
            }

            // Generic classes get special case-by-case treatment.
            if (t.IsGenericType)
            {
                var typedef = t.GetGenericTypeDefinition();
                var typeargs = t.GetGenericArguments();

                if (CustomSerializers.TryGetValue(typedef, out custom))
                {
                    result = custom.Unserialize(this, input, typeargs);
                    return true;
                }
            }

            result = null;
            return false;
        }

        #endregion 

        #region Primitive serialization

        /// <summary>
        /// Serialize a primitive type, without including type information.
        /// </summary>
        public void SerializePrimitive(Type t, object serialized, Stream output)
        {
            using (var bw = new BinaryWriter(output, Encoding.UTF8, true))
            {
                if (t == typeof (Int32))
                {
                    bw.Write((Int32) serialized);
                    return;
                }

                throw new SerializationException(
                    string.Format("Unsupported serialization of type {0}.", t));
            }
        }

        /// <summary>
        /// Unserialize a primitive type.
        /// </summary>
        public object UnserializePrimitive(Type t, Stream input)
        {
            using (var br = new BinaryReader(input, Encoding.UTF8, true))
            {
                if (t == typeof(Int32))
                {
                    return br.ReadInt32();
                }

                throw new SerializationException(
                    string.Format("Unsupported serialization of type {0}.", t));
            }
        }

        #endregion

        #region Persistent type members

        /// <summary>
        /// Returns the persisted members of a persistent type, in order of
        /// serialization. Uses a static cache.
        /// </summary>
        private static IEnumerable<KeyValuePair<MemberInfo, PersistMemberAttribute>> GetPersistentTypeMembers(Type t)
        {
            List<KeyValuePair<MemberInfo, PersistMemberAttribute>> list;
            if (PersistentTypeMembers.TryGetValue(t, out list)) return list;

            list = t.GetMembers()
                    .Select(m => new KeyValuePair<MemberInfo, PersistMemberAttribute>(m,
                        m.GetCustomAttribute<PersistMemberAttribute>()))
                    .Where(kv => kv.Value != null)
                    .OrderBy(kv => kv.Value.Order)
                    .ToList();

            lock (PersistentTypeMembers)
            {
                if (!PersistentTypeMembers.ContainsKey(t)) 
                    PersistentTypeMembers.Add(t, list);
            }

            return list;
        }

        /// <summary>
        /// A cached list of serialized fields of persistent types
        /// </summary>
        private static readonly Dictionary<Type,List<KeyValuePair<MemberInfo,PersistMemberAttribute>>> PersistentTypeMembers = 
           new Dictionary<Type, List<KeyValuePair<MemberInfo, PersistMemberAttribute>>>();

        #endregion 
        
        /// <summary>
        /// Serializes an object to a stream.
        /// </summary>
        public void Serialize(object serialized, Stream output)
        {
            Type t = serialized.GetType();
            var name = PersistAttribute.GetName(t);

            if (null == name)
                throw new ArgumentException(
                    string.Format("Type {0} does not have Persist attribute.", t.Name),
                    "serialized");

            var typeid = TypeDictionary.FindIdentifier(name);

            output.EncodeUInt7Bit(typeid);

            SerializeWithType(t, serialized, output);
        }

        /// <summary>
        /// Serialize an object to a byte sequence.
        /// </summary>
        public byte[] Serialize(object serialized)
        {
            using (var stream = new MemoryStream())
            {
                Serialize(serialized, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Unserializes an object from a stream.
        /// </summary>
        public object Unserialize(Stream input)
        {
            var typeid = input.DecodeUInt7Bit();
            var name = TypeDictionary.FindType(typeid);

            if (null == name)
                throw new SerializationException(
                    string.Format("Unrecognized type id #{0}", typeid));

            var type = PersistAttribute.GetTypeByName(name);

            if (null == type)
                throw new SerializationException(
                    string.Format("Unknown type name '{0}'", name));

            return UnserializeWithType(type, input);
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
                return (T)obj;
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
        /// Custom serializers are used to perform custom serialization for
        /// specific types.
        /// </summary>
        private static readonly Dictionary<Type, ICustomSerializer> CustomSerializers =
            new Dictionary<Type, ICustomSerializer>();

        public static void RegisterCustomSerializer(Type t, ICustomSerializer ics)
        {
            CustomSerializers.Add(t, ics);
        }

        static UntypedSerializer()
        {
            // Register all standard custom serializers here
            // (because otherwise class StandardCustomSerializers is never
            // actually loaded)
            StandardCustomSerializers.RegisterAll();
        }
    }
}
