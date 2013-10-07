using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Cms.Page.Event
{
    /// <summary>
    /// A new page is created.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Events.PageCreated")]
    public class PageCreated : IPageEvent
    {
        public Id Id { get; private set; }
        public DateTime Time { get; private set; }
        public Id AuthorId { get; private set; }

        public PageCreated() {}

        public PageCreated(Id id, DateTime time, Id author)
        {
            AuthorId = author;
            Id = id;
            Time = time;
        }
    }
}
