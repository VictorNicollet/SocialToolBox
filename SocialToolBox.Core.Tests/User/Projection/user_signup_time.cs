using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.User.Event;
using SocialToolBox.Core.User.Projection;

namespace SocialToolBox.Core.Tests.User.Projection
{
    [TestFixture]
    public class user_signup_time : EventReaderFixture<IUserEvent,UserSignupTime>
    {
        [Test]
        public void initial_is_empty()
        {
            Assert.AreEqual(Value, new UserSignupTime(null));
        }

        [Test]
        public void set_once()
        {
            var date = new DateTime(2010, 2, 3, 0, 0, 0, DateTimeKind.Utc);
            After(new UserSignedUp(Id.Parse("aaaaaaaaaaa"), date));
            Assert.AreEqual(new UserSignupTime(date), Value);
        }

        [Test]
        public void set_twice_after()
        {
            var date1 = new DateTime(2010, 2, 3, 0, 0, 0, DateTimeKind.Utc);
            var date2 = new DateTime(2010, 2, 3, 0, 0, 1, DateTimeKind.Utc);
            After(new UserSignedUp(Id.Parse("aaaaaaaaaaa"), date1));
            After(new UserSignedUp(Id.Parse("aaaaaaaaaaa"), date2));
            Assert.AreEqual(new UserSignupTime(date1), Value);
        }

        [Test]
        public void set_twice_before()
        {
            var date1 = new DateTime(2010, 2, 3, 0, 0, 0, DateTimeKind.Utc);
            var date2 = new DateTime(2010, 2, 3, 0, 0, 1, DateTimeKind.Utc);
            After(new UserSignedUp(Id.Parse("aaaaaaaaaaa"), date2));
            After(new UserSignedUp(Id.Parse("aaaaaaaaaaa"), date1));
            Assert.AreEqual(new UserSignupTime(date1), Value);
        }
    }
}
