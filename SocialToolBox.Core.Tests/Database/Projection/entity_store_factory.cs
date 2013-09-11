using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Projection;
using SocialToolBox.Core.Database.Serialization;
using SocialToolBox.Core.Mocks.Database;

namespace SocialToolBox.Core.Tests.Database.Projection
{
    [TestFixture]
    public class entity_store_factory
    {
        public ProjectionEngine Projections;
        public IEntityStore<string> Store; 
        public IEventStream Events;
        public IEntityStoreFactory Factory;

        public Id IdA = Id.Parse("aaaaaaaaaaa");
        public Id IdB = Id.Parse("bbbbbbbbbbb");

        [Persist("SocialToolBox.Core.Tests.Database.Projection.entity_store_factory.ConcatOrRemoveEvent")]
        private class ConcatOrRemoveEvent
        {
            [PersistMember(0)]
            public readonly Id? Id;

            [PersistMember(1)]
            public readonly string Value;

            public ConcatOrRemoveEvent(Id? id, string value)
            {
                Id = id;
                Value = value;
            }

            public ConcatOrRemoveEvent()
            {
            }
        }

        private class ProjectionConfig : IEntityStoreProjection<ConcatOrRemoveEvent, string>
        {
            public Id? EventIdentifier(ConcatOrRemoveEvent ev)
            {
                return ev.Id;
            }

            public string Update(Id id, ConcatOrRemoveEvent ev, string old)
            {
                if (ev.Value == null) return null;
                return (old ?? "!") + "." + ev.Value;
            }
        }

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Projections = driver.Projections;
            Events = driver.GetEventStream("A", true);
            Factory = driver.EntityStore;
            Store = Factory.Create("TEST", new ProjectionConfig(), new[] {Events});
        }

        private void Remove(Id id)
        {
            Events.AddEvent(new ConcatOrRemoveEvent(id, null)); 
        }

        private void Concat(Id id, string what)
        {
            Events.AddEvent(new ConcatOrRemoveEvent(id, what));
        }

        private void Nop()
        {
            Events.AddEvent(new ConcatOrRemoveEvent(null, null));
        }

        [Test]
        public void empty_initially()
        {
            Assert.IsNull(Store.Get(IdA).Result);
            Assert.IsEmpty(Store.GetAll(new[]{ IdA, IdB }).Result);
        }

        [Test]
        public void empty_before_running()
        {
            Concat(IdA, "A");
            Assert.IsNull(Store.Get(IdA).Result);
            Assert.IsEmpty(Store.GetAll(new[] { IdA, IdB }).Result);
        }

        [Test]
        public void initialization()
        {
            Concat(IdA, "A");
            Projections.Run();

            Assert.AreEqual("!.A", Store.Get(IdA).Result);
            
            var dict = Store.GetAll(new[] { IdA, IdB }).Result;
            Assert.AreEqual(1, dict.Count);
            Assert.AreEqual("!.A", dict[IdA]);
        }

        [Test]
        public void update()
        {
            Concat(IdA, "A");
            Concat(IdA, "B");
            Projections.Run();

            Assert.AreEqual("!.A.B", Store.Get(IdA).Result);

            var dict = Store.GetAll(new[] { IdA, IdB }).Result;
            Assert.AreEqual(1, dict.Count);
            Assert.AreEqual("!.A.B", dict[IdA]);
        }

        [Test]
        public void removal()
        {
            Concat(IdA, "A");
            Concat(IdA, "B");
            Remove(IdA);
            Projections.Run();

            Assert.IsNull(Store.Get(IdA).Result);
            Assert.IsEmpty(Store.GetAll(new[] { IdA, IdB }).Result);
        }

        [Test]
        public void nop()
        {
            Concat(IdA, "A");
            Nop();
            Concat(IdA, "B");
            Projections.Run();

            Assert.AreEqual("!.A.B", Store.Get(IdA).Result);

            var dict = Store.GetAll(new[] { IdA, IdB }).Result;
            Assert.AreEqual(1, dict.Count);
            Assert.AreEqual("!.A.B", dict[IdA]);
        }

        [Test]
        public void get_all()
        {
            Concat(IdA, "A");
            Concat(IdB, "B");
            Projections.Run();

            Assert.AreEqual("!.A", Store.Get(IdA).Result);
            Assert.AreEqual("!.B", Store.Get(IdB).Result);

            var dict = Store.GetAll(new[] { IdA, IdB }).Result;
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("!.A", dict[IdA]);
            Assert.AreEqual("!.B", dict[IdB]);
        }
    }
}
