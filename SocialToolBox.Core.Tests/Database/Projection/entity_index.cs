using System.Linq;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Database.Index;
using SocialToolBox.Core.Mocks.Database.Projections;

namespace SocialToolBox.Core.Tests.Database.Projection
{
    [TestFixture]
    public class entity_index
    {
        public Id IdA = Id.Parse("aaaaaaaaaaa");
        public Id IdB = Id.Parse("bbbbbbbbbbb");
        public Id IdC = Id.Parse("ccccccccccc");

        public EntityIndex<StringKey, StringKey, IPair<string, string>[]> Index;
            
        [SetUp]
        public void SetUp()
        {
            Index = new EntityIndex<StringKey, StringKey, IPair<string, string>[]>(
                (id,set) => set.Select(p => Pair.Make(new StringKey(p.First), new StringKey(p.Second))));    
        }

        [Test]
        public void empty()
        {
            Assert.IsNull(Index.Get(IdA).Result);
            Assert.IsEmpty(Index.GetAll(new[] { IdA, IdB }).Result);
        }

        [Test]
        public void get_after_insert()
        {
            Index.Update(IdA,new []{ Pair.Make("a","b") });
            var pair = Index.Get(IdA).Result;
            Assert.AreEqual(1, pair.Length);
            Assert.AreEqual("a", pair[0].First);
            Assert.AreEqual("b", pair[0].Second);
        }

        [Test]
        public void get_after_update()
        {
            Index.Update(IdA, new IPair<string,string>[] {});
            Index.Update(IdA, new[] { Pair.Make("a", "b") });
            var pair = Index.Get(IdA).Result;
            Assert.AreEqual(1, pair.Length);
            Assert.AreEqual("a", pair[0].First);
            Assert.AreEqual("b", pair[0].Second);
        }

        [Test]
        public void get_all_after_insert()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("c", "d") });
            var all = Index.GetAll(new []{IdA,IdB,IdC}).Result;

            var a = all[IdA];
            Assert.AreEqual(1, a.Length);
            Assert.AreEqual("a", a[0].First);
            Assert.AreEqual("b", a[0].Second);

            var b = all[IdB];
            Assert.AreEqual(1, b.Length);
            Assert.AreEqual("c", b[0].First);
            Assert.AreEqual("d", b[0].Second);
        }

        [Test]
        public void query_generic()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("c", "d") });

            var query = Index.Query(new StringKey("a"), 10).Result;

            Assert.AreEqual(1, query.Length);
            Assert.AreEqual(IdA,query[0].First);
            Assert.AreEqual(1, query[0].Second.Length);
            Assert.AreEqual("a", query[0].Second[0].First);
            Assert.AreEqual("b", query[0].Second[0].Second);
        }

        [Test]
        public void query_order()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("a", "d") });

            var query = Index.Query(new StringKey("a"), 10).Result;

            Assert.AreEqual(2, query.Length);
            Assert.AreEqual(IdA, query[0].First);
            Assert.AreEqual(1, query[0].Second.Length);
            Assert.AreEqual("a", query[0].Second[0].First);
            Assert.AreEqual("b", query[0].Second[0].Second);
            Assert.AreEqual(IdB, query[1].First);
            Assert.AreEqual(1, query[1].Second.Length);
            Assert.AreEqual("a", query[1].Second[0].First);
            Assert.AreEqual("d", query[1].Second[0].Second);
        }

        [Test]
        public void query_order_descending()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("a", "d") });

            var query = Index.Query(new StringKey("a"), 10, 0, null, null, true).Result;

            Assert.AreEqual(2, query.Length);
            Assert.AreEqual(IdA, query[1].First);
            Assert.AreEqual(1, query[1].Second.Length);
            Assert.AreEqual("a", query[1].Second[0].First);
            Assert.AreEqual("b", query[1].Second[0].Second);
            Assert.AreEqual(IdB, query[0].First);
            Assert.AreEqual(1, query[0].Second.Length);
            Assert.AreEqual("a", query[0].Second[0].First);
            Assert.AreEqual("d", query[0].Second[0].Second);
        }

        [Test]
        public void query_count()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("a", "d") });

            var query = Index.Query(new StringKey("a"), 1).Result;

            Assert.AreEqual(1, query.Length);
            Assert.AreEqual(IdA, query[0].First);
            Assert.AreEqual(1, query[0].Second.Length);
            Assert.AreEqual("a", query[0].Second[0].First);
            Assert.AreEqual("b", query[0].Second[0].Second);
        }

        [Test]
        public void query_count_offset()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("a", "d") });

            var query = Index.Query(new StringKey("a"), 2, 1).Result;

            Assert.AreEqual(1, query.Length);
            Assert.AreEqual(IdB, query[0].First);
            Assert.AreEqual(1, query[0].Second.Length);
            Assert.AreEqual("a", query[0].Second[0].First);
            Assert.AreEqual("d", query[0].Second[0].Second);
        }

        [Test]
        public void query_multi()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "a"), Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("a", "c"), Pair.Make("a", "d") });

            var query = Index.Query(new StringKey("a"), 2).Result;

            Assert.AreEqual(2, query.Length);
            Assert.AreEqual(IdA, query[0].First);
            Assert.AreEqual(2, query[0].Second.Length);
            Assert.AreEqual("a", query[0].Second[0].First);
            Assert.AreEqual("a", query[0].Second[0].Second);
            Assert.AreEqual("a", query[0].Second[1].First);
            Assert.AreEqual("b", query[0].Second[1].Second);
            Assert.AreEqual(IdA, query[1].First);
        }

        [Test]
        public void query_between()
        {
            Index.Update(IdA, new[] { Pair.Make("a", "a"), Pair.Make("a", "b") });
            Index.Update(IdB, new[] { Pair.Make("a", "c"), Pair.Make("a", "d") });

            var query = Index.Query(new StringKey("a"), 10, 0, new StringKey("b"), new StringKey("c")).Result;

            Assert.AreEqual(2, query.Length);
            Assert.AreEqual(IdA, query[0].First);
            Assert.AreEqual(2, query[0].Second.Length);
            Assert.AreEqual("a", query[0].Second[0].First);
            Assert.AreEqual("a", query[0].Second[0].Second);
            Assert.AreEqual(IdB, query[1].First);
            Assert.AreEqual(2, query[1].Second.Length);
            Assert.AreEqual("a", query[1].Second[0].First);
            Assert.AreEqual("c", query[1].Second[0].Second);
        }
    }
}
