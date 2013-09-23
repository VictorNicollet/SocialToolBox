﻿using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Core.Mocks.Database.Events
{
    /// <summary>
    /// A mock account was deleted.
    /// </summary>
    [Persist("SocialToolBox.Core.Mocks.Database.Events.MockAccountDeleted")]
    public class MockAccountDeleted : IMockEvent
    {
        [PersistMember(0)]
        public Id Id { get; private set; }

        [PersistMember(1)]
        public DateTime Time { get; private set; }

        public MockAccountDeleted(Id id, DateTime time)
        {
            Id = id;
            Time = time;
        }

        public MockAccountDeleted() {}
    }
}
