using System.Runtime;
using NUnit.Framework;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.Args;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web
{
    [TestFixture]
    public class facet
    {
        public IWebDriver Driver;

        /// <summary>
        /// A mock facet merely published internal endpoint registration functions
        /// from <see cref="Facet"/>
        /// </summary>
        private class MockFacet : Facet
        {
            public MockFacet(IWebDriver driver, string prefix) : base(driver, prefix) {}

            public new WebEndpointBuilder<TArgs> OnGet<TArgs>(string url)
                where TArgs : class, IWebUrlArgument, new()
            {
                return base.OnGet<TArgs>(url);
            }

            public new WebEndpointBuilder<TArgs> OnPost<TArgs>(string url)
                where TArgs : class, IWebUrlArgument, new()
            {
                return base.OnPost<TArgs>(url);
            }
        }

        /// <summary>
        /// A mock handler, used to test registration.
        /// </summary>
        private class MockHandler : WebRequestHandler<AnyArgs>
        {
            protected override WebResponse Process()
            {
                return Html("");
            }
        }

        [SetUp]
        public void SetUp()
        {
            Driver = new WebDriver();
        }

        /// <summary>
        /// Returns a new mock facet.
        /// </summary>
        private MockFacet Mock(string url = "base")
        {
            return new MockFacet(Driver, url);
        }

        [Test]
        public void correct_driver()
        {
            var facet = Mock();
            Assert.AreEqual(Driver, facet.Driver);
        }

        [Test]
        public void empty_namespace()
        {
            var facet = Mock("");
            Assert.AreEqual("/", facet.Namespace);
        }

        [Test]
        public void default_namespace()
        {
            var facet = Mock();
            Assert.AreEqual("/base/", facet.Namespace);
        }

        [Test]
        public void unclean_namespace()
        {
            var facet = Mock("/base//");
            Assert.AreEqual("/base/", facet.Namespace);
        }

        [Test]
        public void segmented_namespace()
        {
            var facet = Mock("/base/foo/");
            Assert.AreEqual("/base/foo/", facet.Namespace);
        }

        [Test]
        public void get_endpoint()
        {
            var facet = Mock();
            var endpoint = facet.OnGet<AnyArgs>("get").Use(new MockHandler());
            CollectionAssert.AreEqual(new[]{"base","get"}, endpoint.BasePath);
            Assert.AreEqual(HttpVerb.Get, endpoint.Verbs);
        }

        [Test]
        public void post_endpoint()
        {
            var facet = Mock();
            var endpoint = facet.OnPost<AnyArgs>("post").Use(new MockHandler());
            CollectionAssert.AreEqual(new[]{"base","post"}, endpoint.BasePath);
            Assert.AreEqual(HttpVerb.Post, endpoint.Verbs);
        }
    }
}
