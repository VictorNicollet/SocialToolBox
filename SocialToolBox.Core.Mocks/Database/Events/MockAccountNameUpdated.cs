using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Core.Mocks.Database.Events
{
    /// <summary>
    /// The name of a mock account was changed.
    /// </summary>
    [Persist("SocialToolBox.Core.Mocks.Database.Events.MockAccountNameUpdated")]
    public class MockAccountNameUpdated : IMockEvent, IEntityPageEvent
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
    
        [PersistMember(2)]
        public string Name { get; private set; }

        public MockAccountNameUpdated(Id id, DateTime time, string name)
        {
            Id = id;
            Time = time;
            Name = name;
        }

        public MockAccountNameUpdated() {}

        public Id EntityId { get { return Id; } }

        public bool EntityTitleChanged { get { return true; } }
    }
}
