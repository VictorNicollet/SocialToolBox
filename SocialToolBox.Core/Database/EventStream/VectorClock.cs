﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SocialToolBox.Core.Database.EventStream
{
    /// <summary>
    /// Tracks the last processed event in one or more event streams.
    /// </summary>
    public class VectorClock : IEquatable<VectorClock>
    {
        /// <summary>
        /// Maps event stream names to next position in those streams.
        /// </summary>
        private readonly Dictionary<string, long> _positions;

        public VectorClock()
        {
            _positions = new Dictionary<string, long>();
        }

        /// <summary>
        /// The next position in the stream with the specified name.
        /// </summary>
        public long GetNextInStream(string stream)
        {
            long position;
            _positions.TryGetValue(stream, out position);
            return position;
        }

        /// <summary>
        /// The next position in the specified stream.
        /// </summary>
        public long GetNextInStream(IEventStream stream)
        {
            return GetNextInStream(stream.Name);
        }

        /// <summary>
        /// Advance the named stream to a new (greater) next position.
        /// </summary>
        public void Advance(string stream, long newNextPosition)
        {
            long current;
            _positions.TryGetValue(stream, out current);
            
            if (current > newNextPosition)
            {
                throw new ArgumentOutOfRangeException(
                    "newNextPosition", newNextPosition,
                    String.Format("Stream '{0}' is already at position {1}", stream, current));
            }

            _positions.Remove(stream);
            _positions.Add(stream, newNextPosition);
        }

        /// <summary>
        /// Advances the specified stream to a new (greater) position.
        /// </summary>
        public void Advance(IEventStream stream, long newNextPosition)
        {
            Advance(stream.Name, newNextPosition);
        }

        /// <summary>
        /// Advances the clock to right after the provided event.
        /// </summary>
        public void Advance<T>(EventInStream<T> inStream) where T : class
        {
            Advance(inStream.Stream.Name, inStream.Position + 1);        
        }

        /// <summary>
        /// Serialize the vector clock to a human-readable format.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            
            foreach (var kv in _positions)
            {
                sb.AppendFormat("{0}:{1}\n",
                    kv.Key.Replace(@"\", @"\\").Replace("\n", @"\n"),
                    kv.Value);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Unserializes a vector clock from the format generated by
        /// <see cref="VectorClock.ToString"/>
        /// </summary>
        public static VectorClock Unserialize(string format)
        {
            var vectorClock = new VectorClock();
            
            foreach (var pair in format.Split('\n'))
            {
                var lastColon = pair.LastIndexOf(':');
                if (lastColon == -1) continue;

                long value;
                if (!long.TryParse(pair.Substring(lastColon + 1), out value)) continue;

                var key = pair.Substring(0, lastColon).Replace(@"\n", "\n").Replace(@"\\", @"\");
                if (key.Length == 0) continue;

                vectorClock.Advance(key, value);
            }

            return vectorClock;
        }

        /// <summary>
        /// Two vector clocks are equal if their <see cref="GetNextInStream(string)"/> function
        /// returns the same values for all parameters.
        /// </summary>
        public bool Equals(VectorClock other)
        {
            foreach (var kv in _positions)
            {
                long otherValue;
                if (kv.Value == 0) continue;
                if (!other._positions.TryGetValue(kv.Key, out otherValue)) return false;
                if (kv.Value != otherValue) return false;            
            }

            foreach (var kv in other._positions)
            {
                long myValue;
                if (kv.Value == 0) continue;
                if (!_positions.TryGetValue(kv.Key, out myValue)) return false;
                if (kv.Value != myValue) return false;
            }

            return true;
        }

        /// <summary>
        /// Create a clone of this clock. Initially equal, but can be modified
        /// separately.
        /// </summary>
        public VectorClock Clone()
        {
            var clock = new VectorClock();
            foreach (var kv in _positions)
                clock._positions.Add(kv.Key, kv.Value);
            return clock;
        }
    }
}
