using System;
using System.Collections.Generic;
using NUnit.Framework;
using SocialToolBox.Core.Database.Index;

namespace SocialToolBox.Core.Tests.Database.Index
{
    [TestFixture]
    public class index_key_comparer
    {
        [IndexKey]
        public class KeyMock
        {
            [IndexField(1)] 
            public int Integer;

            [IndexField(0)] 
            public int? NullInteger { get; set; }

            [IndexField(2)] 
            public string String;

            [IndexField(3, IsCaseSensitive = false)]
            public string StringCaseInsensitive;

            [IndexField(4)] 
            public bool Boolean;

            [IndexField(4)] 
            public bool? NullBoolean;

            [IndexField(5)] 
            public DateTime Time;

            [IndexField(6)]
            public DateTime? NullTime { get; set; }

            public int NotCompared;
        }

        private IComparer<KeyMock> _comparer;

        [SetUp]
        public void SetUp()
        {
            _comparer = new IndexKeyComparer<KeyMock>();
        }

        [Test]
        public void equal()
        {
            Assert.AreEqual(0, _comparer.Compare(new KeyMock(), new KeyMock()));
        }

        [Test]
        public void equal_with_not_compared()
        {
            Assert.AreEqual(0, _comparer.Compare(new KeyMock{NotCompared = 1}, new KeyMock()));
        }

        [Test]
        public void int_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock{Integer=3}, new KeyMock()));
        }

        [Test]
        public void is_symmetrical()
        {
            Assert.Greater(0, _comparer.Compare(new KeyMock(), new KeyMock { Integer = 3 }));
        }

        [Test]
        public void null_int_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock {NullInteger=3}, new KeyMock()));
        }
        
        [Test]
        public void string_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock {String="A"}, new KeyMock()));
        }

        [Test]
        public void string_ci_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock {StringCaseInsensitive="A"}, new KeyMock()));
        }

        [Test]
        public void string_ci_equality()
        {
            Assert.AreEqual(0, _comparer.Compare(new KeyMock { StringCaseInsensitive="i"}, new KeyMock{StringCaseInsensitive="I"}));
        }

        [Test]
        public void bool_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock {Boolean = true}, new KeyMock()));
        }

        [Test]
        public void null_bool_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock {NullBoolean = false}, new KeyMock()));
        }

        [Test]
        public void datetime_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock {Time=DateTime.Parse("2012/05/21")}, new KeyMock()));
        }

        [Test]
        public void null_datetime_inequality()
        {
            Assert.Less(0, _comparer.Compare(new KeyMock {NullTime=DateTime.Parse("2012/05/21")}, new KeyMock()));
        }

        [Test]
        public void respect_order()
        {
            Assert.Less(0, _comparer.Compare(
                // Field order = 0 is greater
                new KeyMock{NullInteger = 1},
                // Field order = 1 is greater
                new KeyMock{Integer = 1}
                ));
        }

        [Test]
        public void same_order_respect_declaration()
        {
            Assert.Less(0, _comparer.Compare(
                new KeyMock { Boolean = true },
                new KeyMock { NullBoolean = true }
                ));
        }
    }
}
