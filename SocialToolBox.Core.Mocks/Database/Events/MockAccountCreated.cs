using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Core.Mocks.Database.Events
{
    /// <summary>
    /// A mock account is created. It has the specified name, 
    /// but no password.
    /// </summary>
    [Persist("SocialToolBox.Core.Mocks.Database.Events.MockAccountCreated")]
    public class MockAccountCreated : IMockEvent, IEntityPageEvent
    {
        [PersistMember(0)]
        public string Name { get; private set; }

        [PersistMember(1)]
        public Id Id { get; private set; }

        [PersistMember(2)]
        public DateTime Time { get; private set; }

        public MockAccountCreated(Id id, string name, DateTime time)
        {
            Id = id;
            Name = name;
            Time = time;
        }        

        public MockAccountCreated() {}
        
        public Id EntityId { get { return Id; } }

        public bool EntityTitleChanged { get { return true; } }
    }
}
