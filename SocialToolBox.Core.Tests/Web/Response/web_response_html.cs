using NUnit.Framework;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Response
{
    [TestFixture]
    public class web_response_html : ResponseVisitorFixture
    {
        [Test]
        public void has_correct_mime_type()
        {
            Assert.AreEqual("text/html", WebResponseHtml.MimeType);
        }

        [Test]
        public void contains_correct_payload()
        {
            WithVisitor(Visitor.OnHtml(h => Assert.AreEqual("<!DOCTYPE html>", h.Html)));
            Do(r => r.Html("<!DOCTYPE html>"));
        }

        [Test]
        public void contains_correct_code()
        {
            WithVisitor(Visitor.OnHtml(h => Assert.AreEqual(200, h.Code)));
            Do(r => r.Html("<!DOCTYPE html>"));
        }

        [Test]
        public void contains_correct_custom_code()
        {
            WithVisitor(Visitor.OnHtml(h => Assert.AreEqual(404, h.Code)));
            Do(r => r.Html("<!DOCTYPE html>", 404));
        }
    }
}
