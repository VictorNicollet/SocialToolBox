using System;
using System.IO;
using NUnit.Framework;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Tests.Database
{
    [TestFixture]
    public class id 
    {
        [Test]
        public void parse_works()
        {
            var id = Id.Parse("abcdef01234");
            Assert.AreEqual("abcdef01234", id.ToString());
        }

        [Test]
        public void equality_works()
        {
            Assert.AreEqual(Id.Parse("abcdef01234"), Id.Parse("abcdef01234"));
        }

        [Test]
        public void inequality_works()
        {
            Assert.AreNotEqual(Id.Parse("abcdef01234"), Id.Parse("zbcdef01234"));
        }

        [Test]
        public void comparison_work()
        {
            Assert.IsTrue(Id.Parse("abcdef01234").CompareTo(Id.Parse("zbcdef01234")) < 0);
        }

        [Test]
        public void parse_expects_11_characters()
        {
            Assert.Throws<ArgumentException>(
                () => Id.Parse("a"),
                "");
        }

        [Test]
        public void parse_respects_alphabet()
        {
            Assert.Throws<ArgumentException>(
                () => Id.Parse("abcdef0123-"),
                "");
        }

        [Test]
        public void generate_creates_parsable()
        {
            var id = Id.Generate();
            Assert.AreEqual(id, Id.Parse(id.ToString()));
        }

        [Test]
        public void generate_is_increasing()
        {
            var first = Id.Generate();
            var second = Id.Generate();
            Assert.IsTrue(first.CompareTo(second) < 0);
        }
    }
}
