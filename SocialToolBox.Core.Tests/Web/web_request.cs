using System.Collections.Generic;
using NUnit.Framework;
using SocialToolBox.Core.Mocks.Web;
using SocialToolBox.Core.Web;

namespace SocialToolBox.Core.Tests.Web
{
    [TestFixture]
    public class web_request
    {
        [Test]
        public void get_uses_url()
        {
            var url = new WebUrl("localhost", new[] {"a", "b"});
            url.AddParameter("c","d");

            var req = WebRequest.Get(url);

            Assert.AreEqual("localhost",req.Domain);
            Assert.AreEqual("a/b",req.Path);
            Assert.IsEmpty(req.UnmatchedPath);
            Assert.AreEqual("a/b",req.MatchedPath);
            Assert.AreEqual("d",req.Get("c"));
            Assert.IsNull(req.Get("e"));
        }

        [Test]
        public void post_uses_url()
        {
            var url = new WebUrl("localhost", new[] { "a", "b" });
            url.AddParameter("c", "d");

            var req = WebRequest.Post(url, new Dictionary<string, string>());

            Assert.AreEqual("localhost", req.Domain);
            Assert.AreEqual("a/b", req.Path);
            Assert.IsEmpty(req.UnmatchedPath);
            Assert.AreEqual("a/b", req.MatchedPath);
            Assert.AreEqual("d", req.Get("c"));
            Assert.IsNull(req.Get("e"));
        }

        [Test]
        public void post_payload_uses_url()
        {
            var url = new WebUrl("localhost", new[] { "a", "b" });
            url.AddParameter("c", "d");

            var req = WebRequest.Post(url, "");

            Assert.AreEqual("localhost", req.Domain);
            Assert.AreEqual("a/b", req.Path);
            Assert.IsEmpty(req.UnmatchedPath);
            Assert.AreEqual("a/b", req.MatchedPath);
            Assert.AreEqual("d", req.Get("c"));
            Assert.IsNull(req.Get("e"));
        }

        [Test]
        public void put_uses_url()
        {
            var url = new WebUrl("localhost", new[] { "a", "b" });
            url.AddParameter("c", "d");

            var req = WebRequest.Put(url, "");

            Assert.AreEqual("localhost", req.Domain);
            Assert.AreEqual("a/b", req.Path);
            Assert.IsEmpty(req.UnmatchedPath);
            Assert.AreEqual("a/b", req.MatchedPath);
            Assert.AreEqual("d", req.Get("c"));
            Assert.IsNull(req.Get("e"));
        }

        [Test]
        public void delete_uses_url()
        {
            var url = new WebUrl("localhost", new[] { "a", "b" });
            url.AddParameter("c", "d");

            var req = WebRequest.Delete(url);

            Assert.AreEqual("localhost", req.Domain);
            Assert.AreEqual("a/b", req.Path);
            Assert.IsEmpty(req.UnmatchedPath);
            Assert.AreEqual("a/b", req.MatchedPath);
            Assert.AreEqual("d", req.Get("c"));
            Assert.IsNull(req.Get("e"));
        }

        [Test]
        public void unmatch_path()
        {
            var url = new WebUrl("localhost", new[] { "a", "b", "c" });
            IWebRequest req = WebRequest.Get(url);

            Assert.AreEqual("a/b/c", req.Path);
            Assert.IsEmpty(req.UnmatchedPath);
            Assert.AreEqual("a/b/c", req.MatchedPath);

            req = req.UnmatchOne();
            Assert.AreEqual("a/b/c", req.Path);
            CollectionAssert.AreEqual(new[]{"c"}, req.UnmatchedPath);
            Assert.AreEqual("a/b", req.MatchedPath);

            req = req.UnmatchOne();
            Assert.AreEqual("a/b/c", req.Path);
            CollectionAssert.AreEqual(new[] { "b", "c" }, req.UnmatchedPath);
            Assert.AreEqual("a", req.MatchedPath);

            req = req.UnmatchOne();
            Assert.AreEqual("a/b/c", req.Path);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, req.UnmatchedPath);
            Assert.AreEqual("", req.MatchedPath);

            req = req.UnmatchOne();
            Assert.IsNull(req);
        }

        [Test]
        public void unmatch_path_keeps_rest()
        {
            var url = new WebUrl("localhost", new[] { "a", "b", "c" });
            var req = WebRequest.Get(url);

            req.PostArgs.Add("a","b");
            req.GetArgs.Add("c","d");
            req.Cookies.Add("e","f");
            req.Payload = "ghi";

            var req2 = req.UnmatchOne();

            Assert.AreEqual("b", req2.Post("a"));
            Assert.AreEqual("d",req2.Get("c"));
            Assert.AreEqual("f",req2.Cookie("e"));
            Assert.AreEqual("ghi",req2.Payload);

            Assert.AreEqual(req.Domain,req2.Domain);
            Assert.AreEqual(req.Verb, req2.Verb);
        }
    }
}
