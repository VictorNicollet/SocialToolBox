using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Mocks.Database.Events
{
    /// <summary>
    /// The password of a mock account was updated.
    /// </summary>
    [Persist("SocialToolBox.Core.Mocks.Database.Events.MockAccountPasswordUpdated")]
    public class MockAccountPasswordUpdated : IMockEvent
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }

        [PersistMember(2)]
        public MockAccount.HashedPassword Password { get; private set; }

        public MockAccountPasswordUpdated(Id id, DateTime time, MockAccount.HashedPassword pass)
        {
            Id = id;
            Time = time;
            Password = pass;
        }

        public MockAccountPasswordUpdated() {}
    }
}
