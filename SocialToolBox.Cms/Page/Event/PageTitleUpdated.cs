using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Cms.Page.Event
{
    /// <summary>
    /// The title of a page is updated.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Event.PageTitleUpdated")]
    public sealed class PageTitleUpdated : IPageEvent
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        [PersistMember(2)]
        public Id AuthorId { get; private set; }

        [PersistMember(3)]
        public string Title { get; private set; }
    
        public PageTitleUpdated() {}

        public PageTitleUpdated(Id id, DateTime time, Id author)
        {
            Id = id;
            Time = time;
            AuthorId = author;
        }

        public override string ToString()
        {
            return string.Format("[{2:s}] Page {0} title updated by {1} : '{3}'",
                Id, AuthorId, Time, Title);
        }
    }
}
