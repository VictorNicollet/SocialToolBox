﻿using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace SocialToolBox.Core.Database
{
    /// <summary>
    /// An event inside an <see cref="IEventStream"/>. Includes the actual event, 
    /// and the event's position inside the original stream.
    /// </summary>
    public class EventInStream<T> where T : class
    {
        public readonly T Event;
        public readonly long Position;

        public EventInStream(T e, long pos)
        {
            Debug.Assert(e != null);
            Debug.Assert(pos >= 0);

            Event = e;
            Position = pos;
        }
    }
}
