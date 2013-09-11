using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Core.Tests.Database.Serialization
{
    [TestFixture]
    public class stream_extension
    {
        public T EncodeDecode<T>(Action<Stream> encode, Func<Stream,T> decode)
        {
            var s = new MemoryStream();
            encode(s);
            s.Seek(0, SeekOrigin.Begin);
            return decode(s);
        }

        [Test]
        public void encode_uint7bit_0()
        {
            Assert.AreEqual(0, 
                EncodeDecode(s => s.EncodeUInt7Bit(0), s => s.DecodeUInt7Bit()));
        }

        [Test]
        public void encode_uint7bit_63()
        {
            Assert.AreEqual(63,
                EncodeDecode(s => s.EncodeUInt7Bit(63), s => s.DecodeUInt7Bit()));
        }

        [Test]
        public void encode_uint7bit_128()
        {
            Assert.AreEqual(128,
                EncodeDecode(s => s.EncodeUInt7Bit(128), s => s.DecodeUInt7Bit()));
        }
        
        [Test]
        public void encode_uint7bit_19937()
        {
            Assert.AreEqual(19937,
                EncodeDecode(s => s.EncodeUInt7Bit(19937), s => s.DecodeUInt7Bit()));
        }

        [Test]
        public void encode_sequence()
        {
            var list = new uint[] {10, 128, 19937, 15};
            var decoded = EncodeDecode(
                s => s.EncodeSequence(StreamExtension.EncodeUInt7Bit, list),
                s => s.DecodeSequence(StreamExtension.DecodeUInt7Bit));

            CollectionAssert.AreEqual(list, decoded);
        }

        [Test]
        public void encode_dictionary()
        {
            var dict = new Dictionary<string, uint>();
            dict.Add("a", 10);
            dict.Add("b", 19937);

            var decoded = EncodeDecode(
                s => s.EncodeDictionary(StreamExtension.EncodeString, StreamExtension.EncodeUInt7Bit, dict),
                s => s.DecodeDictionary(StreamExtension.DecodeString, StreamExtension.DecodeUInt7Bit));

            CollectionAssert.AreEqual(dict, decoded);
        }

        [Test]
        public void encode_string()
        {
            Assert.AreEqual("H€llo wörld.",
                EncodeDecode(s => s.EncodeString("H€llo wörld."), s => s.DecodeString()));
        }

        [Test]
        public void encode_null_string()
        {
            Assert.AreEqual(null,
                EncodeDecode(s => s.EncodeString(null), s => s.DecodeString()));
        }

        [Test]
        public void encode_empty_string()
        {
            Assert.AreEqual("",
                EncodeDecode(s => s.EncodeString(""), s => s.DecodeString()));
        }


        [Test]
        public void encode_true()
        {
            Assert.AreEqual(true,
                EncodeDecode(s => s.EncodeBool(true), s => s.DecodeBool()));
        }

        [Test]
        public void encode_false()
        {
            Assert.AreEqual(false,
                EncodeDecode(s => s.EncodeBool(false), s => s.DecodeBool()));
        }
    }
}
