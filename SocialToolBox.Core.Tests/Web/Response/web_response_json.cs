using NUnit.Framework;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Response
{
    [TestFixture]
    public class web_response_json : ResponseVisitorFixture
    {
        [Test]
        public void has_correct_mime_type()
        {
            Assert.AreEqual("application/json", WebResponseJson.MimeType);
        }

        [Test]
        public void contains_correct_payload()
        {
            WithVisitor(Visitor.OnJson(j => Assert.AreEqual("[1,2]", j.Json)));
            Do(r => r.Json("[1,2]"));
        }

        [Test]
        public void contains_correct_code()
        {
            WithVisitor(Visitor.OnJson(j => Assert.AreEqual(200, j.Code)));
            Do(r => r.Json("[1,2]"));
        }

        [Test]
        public void contains_correct_custom_code()
        {
            WithVisitor(Visitor.OnJson(j => Assert.AreEqual(404, j.Code)));
            Do(r => r.Json("[1,2]", 404));
        }
    }
}
