using System.Linq;
using NUnit.Framework;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web.Args;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Entity.Web.EntityPage
{
    [TestFixture]
    public class all_pages : MockFixture
    {
        [Test]
        public void get_page_1()
        {
            var response = (WebResponsePage) Facet.All.Query(new PageArgs()).Run();
            var page = response.Page as ColumnPage;
            Assert.IsNotNull(page);
            Assert.AreEqual(1, page.Columns.Length);

            var list = page.Columns[0].First() as ListVertical;
            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Items.Count);
        }

        [Test]
        public void get_page_2()
        {
            var response = (WebResponsePage)Facet.All.Query(new PageArgs(1)).Run();
            var page = response.Page as ColumnPage;
            Assert.IsNotNull(page);
            Assert.AreEqual(1, page.Columns.Length);

            var list = page.Columns[0].First() as ListVertical;
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Items.Count);
        }
    }
}
