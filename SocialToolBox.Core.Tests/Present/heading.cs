using System;
using NUnit.Framework;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Tests.Present
{
    [TestFixture]
    public sealed class heading
    {
        [Test]
        public void has_label()
        {
            var h = new Heading("thelabel", 1);
            Assert.AreEqual("thelabel", h.Label);
        }

        [Test]
        public void has_level()
        {
            var h = new Heading("l", 3);
            Assert.AreEqual(3, h.Level);
        }

        [Test]
        public void level_between_1_and_6()
        {
            for (var i = -10; i < 10; ++i)
            {
                if (i >= 1 && i <= 6)
                {
                    Assert.AreEqual(i, new Heading("l", i).Level);
                }
                else
                {
                    Assert.Throws<ArgumentOutOfRangeException>(() => 
                        Assert.AreEqual(i, new Heading("l", i).Level));
                }
            }
        }
    }
}
