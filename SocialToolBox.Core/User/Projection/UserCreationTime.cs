using System;
using SocialToolBox.Core.Database.Reader;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Core.User.Projection
{
    /// <summary>
    /// The date when an user was created (the date of the earliest user event).
    /// </summary>
    [Persist("SocialToolBox.Core.User.Projection.UserCreationDate")]
    public class UserCreationTime : EarliestTime<IUserEvent>
    {
        public UserCreationTime() {}

        public UserCreationTime(DateTime? time) : base(time) {}

        public override bool AcceptEvent(IUserEvent ev)
        {
            return true;
        }
    }
}
