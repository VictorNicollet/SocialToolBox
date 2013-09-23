using NUnit.Framework;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Tests.Present
{
    [TestFixture]
    public class html_string
    {
        [Test]
        public void empty_is_empty()
        {
            Assert.AreEqual("", HtmlString.Escape("").ToString());
        }

        [Test]
        public void escaping_works()
        {
            Assert.AreEqual("&lt;&amp;&gt;", HtmlString.Escape("<&>").ToString());
        }

        [Test]
        public void verbatim_does_no_escaping()
        {
            Assert.AreEqual("<&>", HtmlString.Verbatim("<&>").ToString());
        }

        [Test]
        public void concat()
        {
            var once = HtmlString.Verbatim("<&>");
            Assert.AreEqual("<&><&><&>", HtmlString.Concat(once, once, once).ToString());
        }

        [Test]
        public void format()
        {
            var once = HtmlString.Verbatim("<&>");
            Assert.AreEqual("<&>&&&lt;&amp;&gt;&&1225",HtmlString.Format("{0}&&{1}&&{2}", once, "<&>", 1225).ToString());
        }
    }
}
