using NUnit.Framework;
using SocialToolBox.Core.Present;

namespace SocialToolBox.Core.Tests.Present
{
    [TestFixture]
    public sealed class rich_user_content
    {
        [Test]
        public void has_content()
        {
            var h = HtmlString.Verbatim("abc");
            var ruc = new RichUserContent(h);
            Assert.AreSame(h, ruc.Html);
        }
    }
}
