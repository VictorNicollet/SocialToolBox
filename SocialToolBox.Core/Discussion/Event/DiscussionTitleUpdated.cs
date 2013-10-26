using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Event;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Core.Discussion.Event
{
    /// <summary>
    /// The title of a discussion was set to a new value.
    /// </summary>
    [Persist("SocialToolBox.Core.Discussion.Event.DiscussionTitleUpdated")]
    public sealed class DiscussionTitleUpdated : IDiscussionEvent, IEventByUser, IEventWithTime
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        [PersistMember(2)]
        public Id AuthorId { get; private set; }
        
        [PersistMember(3)]
        public string Title { get; private set; }

        public DiscussionTitleUpdated() {}

        public DiscussionTitleUpdated(Id id, DateTime time, Id author, string title)
        {
            Id = id;
            Time = time;
            AuthorId = author;
            Title = title;
        }
    }
}
