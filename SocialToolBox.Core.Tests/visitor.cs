using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Mocks.Database.Events;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests
{
    [TestFixture]
    public class visitor
    {
        [Test]
        public void find_event()
        {
            var visitor = new Visitor<MockAccount, MockAccount>();
            visitor.On<MockAccountCreated>((e,i) => new MockAccount{Name=e.Name});
            visitor.On<MockAccountDeleted>((e,i) => null);

            var ev = new MockAccountCreated(Id.Parse("aaaaaaaaaaa"), "Test", DateTime.Parse("2012/05/21"));

            var m = visitor.Visit(ev,null);

            Assert.AreEqual("Test", m.Name);
            Assert.IsNull(m.Password);
        }

        [Test]
        public void find_event_by_interface()
        {
            var visitor = new Visitor<Unit, DateTime>();
            visitor.On<IMockEvent>((e,unit) => e.Time);

            var date = DateTime.Parse("2012/05/21");
            var ev = new MockAccountDeleted(Id.Parse("aaaaaaaaaaa"), date);

            var date2 = visitor.Visit(ev, Unit.Instance);

            Assert.AreEqual(date,date2);
        }
    }
}
