using NUnit.Framework;
using SocialToolBox.Core.Mocks.Present;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Tests.Web.Response
{
    [TestFixture]
    public class web_response_page : ResponseVisitorFixture
    {
        [Test]
        public void contains_correct_renderer()
        {
            var renderer = new NodeRenderer();
            WithVisitor(Visitor.OnPage(p => Assert.AreEqual(renderer, p.Renderer)));
            Do(r => r.Page(HtmlString.Escape(""), renderer));
        }

        [Test]
        public void contains_correct_page()
        {
            var page = HtmlString.Escape("<&>");
            WithVisitor(Visitor.OnPage(p => Assert.AreEqual(page, p.Page)));

            var renderer = new NodeRenderer();
            Do(r => r.Page(page, renderer));
        }
        [Test]
        public void contains_correct_code()
        {
            var page = HtmlString.Escape("<&>");
            WithVisitor(Visitor.OnPage(p => Assert.AreEqual(200, p.Code)));

            var renderer = new NodeRenderer();
            Do(r => r.Page(page, renderer));
        }

        [Test]
        public void contains_correct_custom_code()
        {
            var page = HtmlString.Escape("<&>");
            WithVisitor(Visitor.OnPage(p => Assert.AreEqual(404, p.Code)));

            var renderer = new NodeRenderer();
            Do(r => r.Page(page, renderer, 404));
        }
    }
}
