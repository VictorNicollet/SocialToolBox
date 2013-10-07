using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Cms.Page.Event
{
    /// <summary>
    /// A page was deleted.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Event.PageDeleted")]
    public sealed class PageDeleted : IPageEvent, IEntityPageEvent
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        [PersistMember(2)]
        public Id AuthorId { get; private set; }
    
        public PageDeleted() {}

        public PageDeleted(Id id, DateTime time, Id author)
        {
            Id = id;
            Time = time;
            AuthorId = author;
        }

        public override string ToString()
        {
            return string.Format("[{2:s}] Page {0} deleted by {1}",
                Id, AuthorId, Time);
        }

        public Id EntityId { get { return Id; } }
    }
}
