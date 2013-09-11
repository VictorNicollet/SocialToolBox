using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// Custom serializers automatically registered with <see cref="UntypedSerializer"/>
    /// </summary>
    public static class StandardCustomSerializers
    {
        public static void RegisterAll()
        {
            UntypedSerializer.RegisterCustomSerializer(ForDateTime.T, new ForDateTime());
            UntypedSerializer.RegisterCustomSerializer(ForId.T, new ForId());
            UntypedSerializer.RegisterCustomSerializer(ForNullable.T, new ForNullable());
        }

        /// <summary>
        /// Custom serializatoin for <see cref="DateTime"/>
        /// </summary>
        public class ForDateTime : ICustomSerializer
        {
            public static readonly Type T = typeof (DateTime);

            public const string Format = "yyyyMMddHHmmss";

            public void Serialize(UntypedSerializer serializer, object serialized, Stream output, Type[] typeargs = null)
            {
                var date = (DateTime) serialized;
                var utc = date.ToUniversalTime();
                var asString = utc.ToString(Format);
                var asBytes = Encoding.ASCII.GetBytes(asString);
                output.Write(asBytes, 0, Format.Length);
            }

            public object Unserialize(UntypedSerializer serializer, Stream input, Type[] typeargs = null)
            {
                var bytes = new byte[Format.Length];
                input.Read(bytes, 0, bytes.Length);
                var asString = Encoding.ASCII.GetString(bytes);
                DateTime result;
                
                if (!DateTime.TryParseExact(asString, Format,
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result))
                {
                    throw new SerializationException(
                        string.Format("Could not parse date '{0}'", asString));    
                }

                return result.ToUniversalTime();
            }
        }

        /// <summary>
        /// Custom serialization for <see cref="Id"/>
        /// </summary>
        public class ForId : ICustomSerializer
        {
            public static readonly Type T = typeof (Id);

            public void Serialize(UntypedSerializer serializer, object serialized, Stream output, Type[] typeargs = null)
            {
                var id = (Id) serialized;
                output.Write(id.Bytes, 0, Id.Length);
            }

            public object Unserialize(UntypedSerializer serializer, Stream input, Type[] typeargs = null)
            {
                var bytes = new byte[Id.Length];
                input.Read(bytes, 0, Id.Length);
                return Id.Parse(bytes);
            }
        }

        /// <summary>
        /// Custom serialization for <see cref="Nullable{T}"/>
        /// </summary>
        public class ForNullable : ICustomSerializer
        {
            public static readonly Type T = typeof (Nullable<>);
            
            public void Serialize(UntypedSerializer serializer, object serialized, Stream output, Type[] typeargs = null)
            {
                Debug.Assert(typeargs != null && typeargs.Length > 0);

                if (serialized == null)
                {
                    output.WriteByte(0);
                    return;
                }

                output.WriteByte(1);
                serializer.SerializeWithType(typeargs[0], serialized, output);
            }

            public object Unserialize(UntypedSerializer serializer, Stream input, Type[] typeargs = null)
            {
                Debug.Assert(typeargs != null && typeargs.Length > 0);

                var first = input.ReadByte();
                if (first == 0) return null;

                return serializer.UnserializeWithType(typeargs[0], input);
            }
        }
    }
}
