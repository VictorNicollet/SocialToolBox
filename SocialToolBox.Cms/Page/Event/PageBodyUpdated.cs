using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Cms.Page.Event
{
    /// <summary>
    /// The body of a page was updated.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Event.PageBodyUpdated")]
    public class PageBodyUpdated : IPageEvent
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
    }
}
