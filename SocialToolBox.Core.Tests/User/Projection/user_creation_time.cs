using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.User.Event;
using SocialToolBox.Core.User.Projection;

namespace SocialToolBox.Core.Tests.User.Projection
{
    [TestFixture]
    public class user_creation_time : EventReaderFixture<IUserEvent,UserCreationTime>
    {
        [Test]
        public void initial_is_empty()
        {
            Assert.AreEqual(Value, new UserCreationTime(null));
        }

        [Test]
        public void set_once()
        {
            var date = new DateTime(2010, 2, 3, 0, 0, 0, DateTimeKind.Utc);
            After(new UserSignedUp(Id.Parse("aaaaaaaaaaa"), date));
            Assert.AreEqual(new UserCreationTime(date), Value);
        }
    }
}
