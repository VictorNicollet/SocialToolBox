using System;
using NUnit.Framework;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Tests.Web
{
    [TestFixture]
    public class web_url
    {
        [Test]
        public void secure_url()
        {
            var url = new WebUrl("localhost", new string[] {}, true);
            Assert.AreEqual("localhost", url.Domain);
            Assert.AreEqual(443, url.Port);
            Assert.IsTrue(url.IsSecure);
            Assert.AreEqual("https://localhost", url.ToString());
        }

        [Test]
        public void unsecure_url()
        {
            var url = new WebUrl("localhost", new string[] { });
            Assert.AreEqual("localhost", url.Domain);
            Assert.AreEqual(80, url.Port);
            Assert.IsFalse(url.IsSecure);
            Assert.AreEqual("http://localhost", url.ToString());            
        }

        [Test]
        public void secure_url_port()
        {
            var url = new WebUrl("localhost", new string[] { }, true, 8080);
            Assert.AreEqual("https://localhost:8080", url.ToString());
        }

        [Test]
        public void unsecure_url_port()
        {
            var url = new WebUrl("localhost", new string[] { }, false, 8080); 
            Assert.AreEqual("http://localhost:8080", url.ToString());
        }

        [Test]
        public void path()
        {
            var url = new WebUrl("localhost", new[] {"a", "b"});
            Assert.AreEqual("http://localhost/a/b", url.ToString());
        }

        [Test]
        public void equal()
        {
            var urla = new WebUrl("localhost", new[] { "a", "b", "c" });
            var urlb = new WebUrl("localhost", new[] { "a", "b", "c" });
            Assert.AreEqual(urla, urlb);
        }

        [Test]
        public void path_empty()
        {
            var url = new WebUrl("localhost", new[] {"a", "", "b", " "});
            Assert.AreEqual("http://localhost/a/b", url.ToString());
        }

        [Test]
        public void path_empty_sequence()
        {
            var url = new WebUrl("localhost", new[] { "a", "", "b", " " });
            CollectionAssert.AreEquivalent(new[] { "a", "b" }, url.Path);
        }

        [Test]
        public void path_escape()
        {
            var url = new WebUrl("localhost", new[] { "a", "x?/&= ", "b" });
            Assert.AreEqual("http://localhost/a/x%3F%2F%26%3D%20/b", url.ToString());
        }

        [Test]
        public void path_escape_sequence()
        {
            var url = new WebUrl("localhost", new[] { "a", "x?/&= ", "b"});
            CollectionAssert.AreEquivalent(new[] { "a", "x?/&= ", "b" }, url.Path);
        }

        [Test]
        public void path_add()
        {
            var url = new WebUrl("localhost", new[] { "a", "b" });
            url.AddPathSegment("c");
            url.AddPathSegment("d");
            Assert.AreEqual("http://localhost/a/b/c/d", url.ToString());
        }

        [Test]
        public void path_add_empty()
        {
            var url = new WebUrl("localhost", new string[] {});
            Assert.Throws<ArgumentException>(() => url.AddPathSegment(" "));
        }
    }
}
