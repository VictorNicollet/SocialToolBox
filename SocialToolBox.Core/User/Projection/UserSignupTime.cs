using System;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.User.Event;

namespace SocialToolBox.Core.User.Projection
{
    /// <summary>
    /// Computes and stores the time when an user signed up.
    /// </summary>
    [Persist("SocialToolBox.Core.User.Projection.UserSignupTime")]
    public class UserSignupTime : IEventReader<IUserEvent>, IEquatable<UserSignupTime>
    {
        [PersistMember(0)]
        public DateTime? Value { get; private set; }

        public UserSignupTime()
        {
            Value = null;
        }

        public UserSignupTime(DateTime? value)
        {
            Value = value;
            if (Value != null) Value = ((DateTime)Value).ToUniversalTime();
        }

        public void Read(IUserEvent ev)
        {
            var userSignedUp = ev as UserSignedUp;
            if (null == userSignedUp) return;

            var newTime = ev.Time.ToUniversalTime();

            if (Value == null || (DateTime)Value > newTime)
            {
                Value = newTime;
            }
        }

        public bool Equals(UserSignupTime other)
        {
            if (Value == null) return (other.Value == null);
            if (other.Value == null) return false;

            return ((DateTime)Value == (DateTime)other.Value);
        }

        public override string ToString()
        {
            if (Value == null) return "<UNDEFINED>";
            var date = (DateTime) Value;
            return date.ToUniversalTime().ToString("O");
        }
    }
}
