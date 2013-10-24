using NUnit.Framework;
using SocialToolBox.Core.Present.Builders;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Tests.Present.Builders
{
    [TestFixture]
    public sealed class item_builder
    {
        public ItemBuilder Builder;

        [SetUp]
        public void SetUp()
        {
            Builder = null;
        }

        public void Yields(string name, WebUrl url)
        {
            var item = Builder.Build();
            Assert.AreEqual(name, item.Name);
            Assert.AreSame(url, item.Url);
        }

        [Test]
        public void has_name()
        {
            Builder = new ItemBuilder("thename");
            Yields("thename", null);
        }

        [Test]
        public void has_url()
        {
            Builder = new ItemBuilder("thename");
            var url = new WebUrl();

            Assert.AreSame(Builder, Builder.WithUrl(url));
            Yields("thename",url);
        }
    }
}
