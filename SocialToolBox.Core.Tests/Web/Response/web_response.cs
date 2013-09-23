using System;
using System.Linq;
using NUnit.Framework;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Response
{
    [TestFixture]
    public class web_response : ResponseVisitorFixture
    {
        [Test]
        public void default_no_cookies()
        {
            WithVisitor(Visitor.OnRedirect(r => Assert.AreEqual(0, r.Cookies.Count())));
            Do(r => r.Redirect(""));
        }

        [Test]
        public void add_one_cookie()
        {
            WithVisitor(Visitor.OnRedirect(r =>
                CollectionAssert.AreEqual(new[] {new WebResponseCookie("NAME", "DOMAIN", "VALUE", TimeSpan.FromHours(2))},
                    r.Cookies)));
            Do(r =>
            {
                var response = r.Redirect("");
                response.AddCookie("NAME","DOMAIN","VALUE",TimeSpan.FromHours(2));
                return response;
            });
        }
    }
}
