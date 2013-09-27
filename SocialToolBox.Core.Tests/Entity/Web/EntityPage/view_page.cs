using System.Linq;
using NUnit.Framework;
using SocialToolBox.Core.Database;
using SocialToolBox.Core.Mocks.Entity;
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

        [Test]
        public void get_bob()
        {
            var response = (WebResponsePage) Facet.View.Query(EntityModuleMock.IdBob.ToArgs()).Run();
            var page = response.Page as ColumnPage;
            Assert.IsNotNull(page);
            Assert.AreEqual(1, page.Columns.Length);
            Assert.AreEqual(EntityModuleMock.NameBob, page.Columns[0].First().ToString());
        }
    }
}
