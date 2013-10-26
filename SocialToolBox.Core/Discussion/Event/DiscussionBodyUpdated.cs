using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Core.Discussion.Event
{
    [Persist("SocialToolBox.Core.Discussion.Event.DiscussionBodyUpdated")]
    public sealed class DiscussionBodyUpdated : IDiscussionEvent, IEventWithTime, IEventByUser
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        [PersistMember(2)]
        public Id AuthorId { get; private set; }

        [PersistMember(3)]
        public string Body { get; private set; }

        public DiscussionBodyUpdated() {}

        public DiscussionBodyUpdated(Id id, DateTime time, Id author, string body)
        {
            Id = id;
            Time = time;
            AuthorId = author;
            Body = body;
        }
    }
}
