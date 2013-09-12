using System;
using System.IO;
using NUnit.Framework;

namespace SocialToolBox.Core.Tests.Web.Response
{
    [TestFixture]
    public class web_response_data : ResponseVisitorFixture
    {
        [Test]
        public void data_has_correct_mimetype()
        {
            WithVisitor(Visitor.OnData(d => 
                Assert.AreEqual("application/octet-stream", d.MimeType)));
            Do(r => r.Data(new MemoryStream(), "application/octet-stream"));
        }

        [Test]
        public void file_has_correct_mimetype()
        {
            WithVisitor(Visitor.OnData(d =>
                Assert.AreEqual("application/octet-stream", d.MimeType)));
            Do(r => r.File(new MemoryStream(), "foo.exe", "application/octet-stream"));
        }

        [Test]
        public void data_has_no_name()
        {
            WithVisitor(Visitor.OnData(d => Assert.IsNull(d.Filename)));
            Do(r => r.Data(new MemoryStream(), "application/octet-stream"));
        }

        [Test]
        public void file_has_correct_name()
        {
            WithVisitor(Visitor.OnData(d =>
                Assert.AreEqual("foo.exe", d.Filename)));
            Do(r => r.File(new MemoryStream(), "foo.exe", "application/octet-stream"));
        }

        [Test]
        public void data_has_correct_content()
        {
            var stream = new MemoryStream();

            WithVisitor(Visitor.OnData(d => Assert.AreSame(stream, d.Stream)));
            Do(r => r.Data(stream, "application/octet-stream"));
        }

        [Test]
        public void file_has_correct_content()
        {
            var stream = new MemoryStream();

            WithVisitor(Visitor.OnData(d => Assert.AreSame(stream, d.Stream)));
            Do(r => r.File(stream, "foo.exe", "application/octet-stream"));
        }

        [Test]
        public void data_closes_stream()
        {
            var stream = new MemoryStream();

            WithVisitor(Visitor.OnData(d => stream.Seek(0, SeekOrigin.Begin)));
            Do(r => r.Data(stream, "application/octet-stream"));
        
            Assert.Throws<ObjectDisposedException>(() => stream.Seek(0, SeekOrigin.Begin));
        }

        [Test]
        public void file_closes_stream()
        {
            var stream = new MemoryStream();

            WithVisitor(Visitor.OnData(d => stream.Seek(0, SeekOrigin.Begin)));
            Do(r => r.File(stream, "foo.exe", "application/octet-stream"));

            Assert.Throws<ObjectDisposedException>(() => stream.Seek(0, SeekOrigin.Begin));
        }
    }
}
