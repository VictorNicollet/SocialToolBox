using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Core.Session.Event
{
    /// <summary>
    /// A session is created. Includes session identifier, token and user.
    /// </summary>
    [Persist("SocialToolBox.Core.Session.Event.SessionCreated")]
    public class SessionCreated : ISessionEvent, IUserEvent
    {
        [PersistMember(0)]
        public Id Session { get; private set; }
        
        [PersistMember(1)]
        public DateTime Time { get; private set; }

        [PersistMember(2)]
        public Id Id { get; private set; }

        [PersistMember(3)]
        public string Token { get; private set; }

        public SessionCreated() {}

        public SessionCreated(Id session, DateTime time, Id user, string token)
        {
            Session = session;
            Time = time;
            Id = user;
            Token = token;
        }
    }
}
