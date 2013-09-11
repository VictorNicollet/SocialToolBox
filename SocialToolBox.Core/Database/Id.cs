using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// An unique identifier. Uses characters [a-zA-Z0-9] and 
    /// is 11 characters long.
    /// </summary>
    [Serializable]
    public struct Id : IEquatable<Id>, IComparable<Id>, ISerializable
    {
        /// <summary>
        /// The number of characters in an identifier.
        /// </summary>
        public const int Length = 11;       

        /// <summary>
        /// The string representation of the identifier.
        /// </summary>
        public readonly string Value;

        private Id(string value)
        {
            Value = value;
        }

        public Id(SerializationInfo info, StreamingContext context)
        {
            Value = info.GetString("id");
        }

        /// <summary>
        /// The byte representation (in ASCII).
        /// </summary>
        public byte[] Bytes { get { return Encoding.ASCII.GetBytes(Value); } }

        /// <summary>
        /// Parse a string as an identifier, throw an exception if parsing
        /// fails.
        /// </summary>
        public static Id Parse(string source)
        {
            if (source.Length != Length) 
                throw new ArgumentException(
                    string.Format("Identifier must be a {0}-character string", Length),
                    "source");

            if (source.Any(c => (c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9')))
                throw new ArgumentException(
                    "Identifier must contain only characters [a-zA-Z0-9]",
                    "source");

            return new Id(source);
        }

        /// <summary>
        /// Parse a byte sequence as an identifier, throw an exception if parsing
        /// fails.
        /// </summary>
        public static Id Parse(byte[] bytes)
        {
            return Parse(Encoding.ASCII.GetString(bytes));
        }

        #region Generation 

        /// <summary>
        /// The initial portion of an unique identifier is the number of one-second
        /// ticks since this date.
        /// </summary>
        private static readonly DateTime EpochStart = new DateTime(2013, 8, 9);

        /// <summary>
        /// The 62 bytes used to construct new identifiers.
        /// </summary>
        private static readonly byte[] Alphabet =
            Encoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

        /// <summary>
        /// The current generator sequence. Incremented every time a
        /// new id is generated.
        /// </summary>
        private static int _generatorSequence;

        /// <summary>
        /// A reasonably unique integer seed.
        /// </summary>
        private static readonly int UidSeed = ((Guid.NewGuid().GetHashCode() % 62) + 62) % 62;

        /// <summary>
        /// Generates a new unique task identifier.
        /// </summary>
        /// <remarks>
        /// Generated as <code>AAAAAABBBBC</code> where A is the number of
        /// seconds since the epoch start, B is a number incremented every 
        /// time an ID is generated, and C is a reasonably unique number for
        /// each process.
        /// </remarks>
        public static Id Generate()
        {
            var output = new byte[Length];
            
            // Locking the alphabet instead of the genseq, because we need an object 
            int genseq;
            lock (Alphabet) genseq = _generatorSequence++;

            genseq = genseq * 62 + UidSeed;

            var ticks = (int)(DateTime.Now - EpochStart).TotalSeconds;

            for (var i = 5; i >= 0; --i)
            {
                output[i] = Alphabet[ticks%62];
                ticks = ticks/62;
            }

            for (var i = 10; i >= 6; --i)
            {
                output[i] = Alphabet[genseq%62];
                genseq = genseq/62;
            }

            return new Id(Encoding.ASCII.GetString(output));
        }

        #endregion

        public bool Equals(Id other)
        {
            return other.Value == Value;
        }

        public int CompareTo(Id other)
        {
            return string.Compare(Value, other.Value, StringComparison.InvariantCulture);
        }

        public override string ToString()
        {
            return Value;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id",Value);
        }
    }
}
