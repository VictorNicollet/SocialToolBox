using System.Collections.Generic;
using NUnit.Framework;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Present.Builders;

namespace SocialToolBox.Core.Tests.Present.Builders
{
    [TestFixture]
    public sealed class column_page_builder
    {
        public ColumnPageBuilder Builder;

        [SetUp]
        public void SetUp()
        {
            Builder = null;
        }

        public void Yields(string title, params IEnumerable<IPageNode>[] columns)
        {
            var page = Builder.Build();
            Assert.AreEqual(title, page.Title);
            CollectionAssert.AreEqual(columns, page.Columns);
        }

        [Test]
        public void has_title()
        {
            Builder = new ColumnPageBuilder("thetitle");
            Assert.AreEqual("thetitle", Builder.Title);
            Yields("thetitle");
        }

        [Test]
        public void has_primary()
        {
            var a = HtmlString.Escape("a");
            var b = HtmlString.Escape("b");

            Builder = new ColumnPageBuilder("t");
            Assert.AreSame(Builder, Builder.AddPrimary(a));
            
            Builder.AddPrimary(b);

            CollectionAssert.AreEqual(new[]{ a, b }, Builder.Primary);

            Yields("t", new []{ a, b });
        }

        [Test]
        public void has_primary_and_secondary()
        {
            var a = HtmlString.Escape("a");
            var b = HtmlString.Escape("b");
            var c = HtmlString.Escape("c");
            var d = HtmlString.Escape("d");

            Builder = new ColumnPageBuilder("t");
            Assert.AreSame(Builder, Builder.AddSecondary(c));
            Builder.AddPrimary(a);
            Builder.AddPrimary(b); 
            Builder.AddSecondary(d);

            CollectionAssert.AreEqual(new[] { c, d }, Builder.Secondary);

            Yields("t", new[] { a, b }, new [] { c, d });       
        }

        [Test]
        public void secondary_becomes_primary()
        {

            var a = HtmlString.Escape("a");
            var b = HtmlString.Escape("b");

            Builder = new ColumnPageBuilder("t");
            Builder.AddSecondary(a);
            Builder.AddSecondary(b);

            CollectionAssert.AreEqual(new[] { a, b }, Builder.Secondary);

            Yields("t", new[] { a, b });  
        }

        [Test]
        public void has_primary_and_secondary_tertiary()
        {
            var a = HtmlString.Escape("a");
            var b = HtmlString.Escape("b");
            var c = HtmlString.Escape("c");
            var d = HtmlString.Escape("d");

            Builder = new ColumnPageBuilder("t");
            Builder.AddPrimary(a);
            Assert.AreSame(Builder, Builder.AddTertiary(c));
            Builder.AddSecondary(b);
            Builder.AddTertiary(d);

            CollectionAssert.AreEqual(new[] { c, d }, Builder.Tertiary);

            Yields("t", new[] { a} , new[] { b }, new[] { c, d });
        }

        [Test]
        public void tertiary_becomes_primary()
        {

            var a = HtmlString.Escape("a");
            var b = HtmlString.Escape("b");

            Builder = new ColumnPageBuilder("t");
            Builder.AddTertiary(a);
            Builder.AddTertiary(b);

            CollectionAssert.AreEqual(new[] { a, b }, Builder.Tertiary);

            Yields("t", new[] { a, b });
        }

        [Test]
        public void tertiary_becomes_secondary()
        {

            var a = HtmlString.Escape("a");
            var b = HtmlString.Escape("b");

            Builder = new ColumnPageBuilder("t");
            Builder.AddTertiary(b);
            Builder.AddPrimary(a);

            CollectionAssert.AreEqual(new[] { a }, Builder.Primary);
            CollectionAssert.AreEqual(new[] { b }, Builder.Tertiary);

            Yields("t", new[] { a}, new []{ b });
        }
    }
}
