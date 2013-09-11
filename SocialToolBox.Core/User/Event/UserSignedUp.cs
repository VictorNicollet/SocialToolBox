using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;

namespace SocialToolBox.Core.User.Event
{
    /// <summary>
    /// A new user signed up for the service. 
    /// </summary>
    /// <remarks>
    /// This event represents the user signing up for the service on his own.
    /// </remarks>
    [Persist("SocialToolBox.Core.User.Event.UserSignedUp")]
    public class UserSignedUp : IUserEvent
    {
        [PersistMember(0)]
        public Id User { get; private set; }

        [PersistMember(1)]
        public DateTime Time { get; private set; }

        public UserSignedUp()
        {
        }

        public UserSignedUp(Id user, DateTime time)
        {
            User = user;
            Time = time;
        }
    }
}
