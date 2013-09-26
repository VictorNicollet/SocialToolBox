using NUnit.Framework;
using SocialToolBox.Core.Database;

namespace SocialToolBox.Core.Tests.Entity.Web.EntityPage
{
    [TestFixture]
    public class view_page : MockFixture
    {
        [Test]
        public void get_empty()
        {
            var response = Facet.View.Query(Id.Parse("40404040404").ToArgs()).Run();
        }
    }
}
