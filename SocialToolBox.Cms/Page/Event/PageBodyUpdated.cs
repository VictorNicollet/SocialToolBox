using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Cms.Page.Event
{
    /// <summary>
    /// The body of a page was updated.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Event.PageBodyUpdated")]
    public sealed class PageBodyUpdated : IPageEvent, IEntityPageEvent
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        [PersistMember(2)]
        public Id AuthorId { get; private set; }
    
        [PersistMember(3)]
        public string Body { get; private set; }

        public PageBodyUpdated() {}

        public PageBodyUpdated(Id id, DateTime time, Id author, string body)
        {
            Id = id;
            Time = time;
            AuthorId = author;
            Body = body;
        }

        public override string ToString()
        {
            return string.Format("[{2:s}] Page {0} body updated by {1}",
                Id, AuthorId, Time);
        }

        public Id EntityId { get { return Id; } }
    }
}
