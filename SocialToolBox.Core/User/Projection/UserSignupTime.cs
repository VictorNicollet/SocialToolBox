using System;
using SocialToolBox.Core.Database.Reader;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Core.User.Projection
{
    /// <summary>
    /// Computes and stores the time when an user signed up.
    /// </summary>
    [Persist("SocialToolBox.Core.User.Projection.UserSignupTime")]
    public class UserSignupTimeEx : EarliestTime<IUserEvent>
    {
        public UserSignupTimeEx() {}

        public UserSignupTimeEx(DateTime? value) : base(value) {}

        public override bool AcceptEvent(IUserEvent ev)
        {
            return ev is UserSignedUp;
        }
    }
}
