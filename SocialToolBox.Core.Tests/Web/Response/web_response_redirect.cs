using NUnit.Framework;

namespace SocialToolBox.Core.Tests.Web.Response
{
    [TestFixture]
    public class web_response_redirect : ResponseVisitorFixture
    {
        [Test]
        public void contains_correct_url()
        {
            WithVisitor(Visitor.OnRedirect(r => Assert.AreEqual("http://example.com", r.Url)));
            Do(r => r.Redirect("http://example.com"));
        }

        [Test]
        public void contains_correct_code()
        {
            WithVisitor(Visitor.OnRedirect(r => Assert.AreEqual(303, r.Code)));
            Do(r => r.Redirect("http://example.com"));
        }

        [Test]
        public void contains_correct_custom_code()
        {
            WithVisitor(Visitor.OnRedirect(r => Assert.AreEqual(301, r.Code)));
            Do(r => r.Redirect("http://example.com", 301));
        }
    }
}
