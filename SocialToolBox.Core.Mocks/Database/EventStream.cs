﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.EventStream;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Core.Mocks.Database
{
    public class EventStream : IEventStream
    {
        /// <summary>
        /// The actual inner database driver, with its known type.
        /// </summary>
        public readonly DatabaseDriver InnerDriver;

        /// <summary>
        /// The name of this stream.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// The serialized events.
        /// </summary>
        public readonly List<byte[]> SerializedEventList;

        /// <summary>
        /// The serializer used to read/write events.
        /// </summary>
        public readonly UntypedSerializer Serializer;

        public EventStream(string name, DatabaseDriver driver)
        {
            InnerDriver = driver;
            SerializedEventList = new List<byte[]>();
            Serializer = new UntypedSerializer(InnerDriver.TypeDictionary);
            _name = name;
        }

        public IDatabaseDriver Driver
        {
            get { return InnerDriver; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Task AddEvent(object e, ICursor t)
        {
            var serialized = Serializer.Serialize(e);
            SerializedEventList.Add(serialized);
            return new Task(() => {});
        }

// ReSharper disable CSharpWarnings::CS1998
        public async Task<EventInStream<T>> GetEvent<T>(long position, IProjectCursor t) where T : class
// ReSharper restore CSharpWarnings::CS1998
        {
            // It is acceptable to cast the position to an int, since this happens in-memory
            if (position >= SerializedEventList.Count) return null;
            
            var bytes = SerializedEventList[(int) position];
            var ev = Serializer.Unserialize<T>(bytes);

            return new EventInStream<T>(this, ev, position);
        }

// ReSharper disable CSharpWarnings::CS1998
        public async Task<EventListInStream<T>> GetEvents<T>(long startPosition, int count, IProjectCursor t) where T : class
// ReSharper restore CSharpWarnings::CS1998
        {
            // It is acceptable to cast the position to an int, since this happens in-memory
            var start = (int) startPosition;
            var list = new List<EventInStream<T>>();
            var pos = start;

            for (; pos - start < count && pos < SerializedEventList.Count; ++pos)
            {
                var bytes = SerializedEventList[pos];
                var ev = Serializer.Unserialize<T>(bytes);
                list.Add(new EventInStream<T>(this, ev, pos));
            }

            return new EventListInStream<T>(list, pos, pos - start);
        }

        // ReSharper disable CSharpWarnings::CS1998
        public async Task<EventListInStream<T>> GetEventsOfType<T>(long startPosition, int count, IProjectCursor t) where T : class
        // ReSharper restore CSharpWarnings::CS1998
        {
            // It is acceptable to cast the position to an int, since this happens in-memory
            var start = (int)startPosition;
            var list = new List<EventInStream<T>>();
            var pos = start;

            for (; pos - start < count && pos < SerializedEventList.Count; ++pos)
            {
                var bytes = SerializedEventList[pos];
                var ev = Serializer.UnserializeOfType<T>(bytes);
                if (ev != null) list.Add(new EventInStream<T>(this, ev, pos));
            }

            return new EventListInStream<T>(list, pos, pos - start);
        }

        public Task<long> NextPosition(IReadCursor t)
        {
            return Task.FromResult((long)SerializedEventList.Count); 
        }
    }
}
