using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Core.Discussion.Event
{
    /// <summary>
    /// A discussion was created by an author through a typical, 
    /// manual "create discussion" process.
    /// </summary>
    [Persist("SocialToolBox.Core.Discussion.Event.DiscussionCreated")]
    public sealed class DiscussionCreated : IDiscussionEvent, IEventWithTime, IEventByUser
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        [PersistMember(2)]
        public Id AuthorId { get; private set; }

        public DiscussionCreated() {}

        public DiscussionCreated(Id id, DateTime time, Id author)
        {
            Id = id;
            Time = time;
            AuthorId = author;
        }
    }
}
