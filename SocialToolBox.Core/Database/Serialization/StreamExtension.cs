using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace SocialToolBox.Core.Database.Serialization
{
    /// <summary>
    /// Extends the stream class with encoding/decoding functions.
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// Encodes an unsigned integer in as few bytes as possible using
        /// a 7-bit encoding.
        /// </summary>
        public static void EncodeUInt7Bit(this Stream stream, uint value)
        {
            while (value > 127)
            {
                stream.WriteByte((byte)(value % 128 + 128));
                value /= 128;
            }          
  
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Decodes an unsigned integer encoded with <see cref="EncodeUInt7Bit"/>
        /// </summary>
        public static uint DecodeUInt7Bit(this Stream stream)
        {
            uint result = 0;
            byte read;
            int shift = 0;

            do
            {
                var nextByte = stream.ReadByte();
                if (nextByte == -1)
                    throw new SerializationException("Premature end of stream.");

                read = (byte) nextByte;
                result += (uint)((read % 128) << shift);

                shift += 7;

            } while (read >= 128);

            return result;
        }

        /// <summary>
        /// Decodes a sequence as encoded by <see cref="EncodeSequence{T}"/>
        /// </summary>
        public static IEnumerable<T> DecodeSequence<T>(this Stream stream, Func<Stream, T> parse)
        {
            var count = stream.DecodeUInt7Bit();
            if (count == 0) return null;

            return stream.DecodeFiniteSequence<T>(parse, count - 1);
        }

        /// <summary>
        /// Decode a sequence of known length.
        /// </summary>
        public static IEnumerable<T> DecodeFiniteSequence<T>(this Stream stream, 
            Func<Stream, T> parse, uint count)
        {            
            for (var i = 0; i < count; ++i)
                yield return parse(stream);
        }    

        /// <summary>
        /// Encodes a sequence by writing the sequence length plus one, then all 
        /// elements in order.
        /// </summary>
        public static void EncodeSequence<T>(this Stream stream, Action<Stream, T> encode, IList<T> sequence)
        {
            if (sequence == null)
            {
                stream.EncodeUInt7Bit(0);
                return;
            }

            stream.EncodeUInt7Bit((uint)sequence.Count+1);
            foreach (var t in sequence) encode(stream, t);
        }

        /// <summary>
        /// Encodes a sequence by writing the sequence length, then all 
        /// elements in order.
        /// </summary>
        public static void EncodeSequence(this Stream stream, Action<Stream, object> encode, Array sequence)
        {
            if (sequence == null)
            {
                stream.EncodeUInt7Bit(0);
                return;
            }

            stream.EncodeUInt7Bit((uint)sequence.Length+1);
            foreach (var t in sequence) encode(stream, t);
        }

        /// <summary>
        /// Encodes a dictionary by writing the value count, then all
        /// key-value pairs in order.
        /// </summary>
        public static void EncodeDictionary<TK, TV>(this Stream stream,
            Action<Stream, TK> encodekey, Action<Stream, TV> encodevalue, IDictionary<TK, TV> dict)
        {
            stream.EncodeUInt7Bit((uint)dict.Count);
            foreach (var kv in dict)
            {
                encodekey(stream, kv.Key);
                encodevalue(stream, kv.Value);
            }
        }

        /// <summary>
        /// Decodes a dictionary as encoded by <see cref="EncodeDictionary{TK,TV}"/>
        /// </summary>
        public static IDictionary<TK, TV> DecodeDictionary<TK, TV>(this Stream stream,
            Func<Stream, TK> decodekey, Func<Stream, TV> decodevalue)
        {
            var dict = new Dictionary<TK, TV>();
            var count = stream.DecodeUInt7Bit();
            for (var i = 0; i < count; ++i)
            {
                var key = decodekey(stream);
                var value = decodevalue(stream);
                dict.Add(key, value);
            }
            return dict;
        }

        /// <summary>
        /// Encodes a string using UTF8, prefixed by its length in bytes plus one.
        /// </summary>
        /// <remarks>
        /// Writing the length in bytes plus one is done so that a value of zero
        /// marks a null string (as opposed to an empty one).
        /// </remarks>
        public static void EncodeString(this Stream stream, string s)
        {
            if (s == null)
            {
                stream.EncodeUInt7Bit(0);
                return;
            }

            var bytes = Encoding.UTF8.GetBytes(s);
            stream.EncodeUInt7Bit((uint)bytes.Length+1);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Decodes a string encoded with <see cref="EncodeString"/>
        /// </summary>
        public static string DecodeString(this Stream stream)
        {
            var count = stream.DecodeUInt7Bit();
            if (count == 0) return null;

            count -= 1;
            var bytes = new byte[count];
            stream.Read(bytes, 0, (int)count);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Encode a boolean as zero or one.
        /// </summary>
        public static void EncodeBool(this Stream stream, bool b)
        {
            stream.WriteByte(b ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// Decodes a boolean, non-zero is true.
        /// </summary>
        public static bool DecodeBool(this Stream stream)
        {
            var b = stream.ReadByte();
            return (b != 0);
        }
    }
}
