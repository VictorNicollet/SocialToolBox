using System;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Mocks.Database.Events;

namespace SocialToolBox.Core.Tests
{
    [TestFixture]
    public class visiting_extractor
    {
        [Test]
        public void find_event()
        {
            var visitor = new VisitingExtractor<IMockEvent,string>();
            visitor.On<MockAccountCreated>(e => e.Name);

            var ev = new MockAccountCreated(Id.Parse("aaaaaaaaaaa"), "Test", DateTime.Parse("2012/05/21"));

            var m = visitor.Visit(ev);

            Assert.AreEqual("Test", m);
        }

        [Test]
        public void find_event_by_interface()
        {
            var visitor = new VisitingExtractor<IMockEvent,DateTime>();
            visitor.On<IMockEvent>(e => e.Time);

            var date = DateTime.Parse("2012/05/21");
            var ev = new MockAccountDeleted(Id.Parse("aaaaaaaaaaa"), date);

            var date2 = visitor.Visit(ev);

            Assert.AreEqual(date, date2);
        }
    }
}
