﻿using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Entity.Event;

namespace SocialToolBox.Cms.Page.Event
{
    /// <summary>
    /// A new page is created.
    /// </summary>
    [Persist("SocialToolBox.Cms.Page.Events.PageCreated")]
    public sealed class PageCreated : IPageEvent, IEntityPageEvent
    {
        [PersistMember(0)]
        public Id Id { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }
        
        [PersistMember(2)]
        public Id AuthorId { get; private set; }

        public PageCreated() {}

        public PageCreated(Id id, DateTime time, Id author)
        {
            AuthorId = author;
            Id = id;
            Time = time;
        }

        public override string ToString()
        {
            return string.Format("[{2:s}] Page {0} created by {1}",
                Id, AuthorId, Time);
        }

        public Id EntityId { get { return Id; } }
    }
}
