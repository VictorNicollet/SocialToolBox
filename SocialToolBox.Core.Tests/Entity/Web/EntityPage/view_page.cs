using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Entity.Web.EntityPage
{
    [TestFixture]
    public class view_page : MockFixture
    {
        [Test]
        public void get_empty()
        {
            var response = (WebResponsePage) Facet.View.Query(Id.Parse("40404040404").ToArgs()).Run();            
            Assert.IsTrue(response.Page is NotFound);
        }
    }
}
