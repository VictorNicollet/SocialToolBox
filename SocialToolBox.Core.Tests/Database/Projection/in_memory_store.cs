﻿using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Mocks.Database;
using SocialToolBox.Core.Mocks.Database.Projections;
using SocialToolBox.Core.Mocks.Database.Serialization;

namespace SocialToolBox.Core.Tests.Database.Projection
{
    [TestFixture]
    public class in_memory_store
    {
        public readonly Id IdA = Id.Parse("aaaaaaaaaaa");
        public readonly Id IdB = Id.Parse("bbbbbbbbbbb");
        public InMemoryStore<MockAccount> Store;

        [SetUp]
        public void SetUp()
        {
            var driver = new DatabaseDriver();
            Store = new InMemoryStore<MockAccount>(driver);
        }

        [Test]
        public void default_is_missing()
        {
            Assert.IsNull(Store.Get(IdA).Result);
        }

        [Test]
        public void save_and_load()
        {
            Store.Set(IdA, MockAccount.Bob).Wait();
            Assert.IsNull(Store.Get(IdB).Result);
            Assert.AreEqual(MockAccount.Bob, Store.Get(IdA).Result);
        }

        [Test]
        public void save_delete_and_load()
        {
            Store.Set(IdA, MockAccount.Bob).Wait();
            Store.Set(IdA, null).Wait();
            Assert.IsNull(Store.Get(IdA).Result);
        }
    }
}
