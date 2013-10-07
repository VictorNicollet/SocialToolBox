using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Cms.Page.Event
{
    /// <summary>
    /// A page was deleted.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Event.PageDeleted")]
    public class PageDeleted : IPageEvent
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
    }
}
