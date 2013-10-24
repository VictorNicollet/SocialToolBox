using NUnit.Framework;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Present.Builders;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Tests.Present.Builders
{
    [TestFixture]
    public sealed class list_builder
    {
        public ListBuilder Builder;

        [SetUp]
        public void SetUp()
        {
            Builder = new ListBuilder();
        }

        public void Yields(Pagination pagination, params IPageNode[] nodes)
        {
            var built = Builder.BuildVertical();
            Assert.AreSame(pagination, built.Pagination);
            CollectionAssert.AreEqual(nodes, built.Items);
        }

        [Test]
        public void default_empty()
        {
            Yields(null);
        }

        [Test]
        public void has_pagination()
        {
            var p = Pagination.PrevNext(new WebUrl(), new WebUrl());
            Builder.WithPagination(p);
            
            Assert.AreSame(p, Builder.Pagination);
            Yields(p);
        }

        [Test]
        public void has_items()
        {
            var a = HtmlString.Escape("a");
            var b = HtmlString.Escape("b");
            Builder.Add(a);
            Builder.Add(b);
            CollectionAssert.AreEqual(new[]{ a, b }, Builder.Items);
            Yields(null, a, b);
        }
    }
}
