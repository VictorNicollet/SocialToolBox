using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Tests.Web.Response
{
    /// <summary>
    /// Provides a base class for implementing response visitors.
    /// </summary>
    public sealed class ResponseVisitorHelper : IWebResponseVisitor
    {
        private Action<WebResponseRedirect> _onRedirect;

        public ResponseVisitorHelper OnRedirect(Action<WebResponseRedirect> f)
        {
            _onRedirect = f;
            return this;
        }

        // ReSharper disable CSharpWarnings::CS1998
        public async Task Visit(WebResponseRedirect redirect)
        {
            if (null == _onRedirect)
                Assert.Fail("Encountered object {0}", redirect);
            
            _onRedirect(redirect);
        }

        private Action<WebResponseJson> _onJson;

        public ResponseVisitorHelper OnJson(Action<WebResponseJson> f)
        {
            _onJson = f;
            return this;
        }

        public async Task Visit(WebResponseJson json)
        {
            if (null == _onJson)
                Assert.Fail("Encountered object {0}", json);

            _onJson(json);
        }

        private Action<WebResponseHtml> _onHtml;

        public ResponseVisitorHelper OnHtml(Action<WebResponseHtml> f)
        {
            _onHtml = f;
            return this;
        }

        public async Task Visit(WebResponseHtml html)
        {
            if (null == _onHtml)
                Assert.Fail("Encountered object {0}", html);

            _onHtml(html);
        }

        private Action<WebResponseData> _onData;

        public ResponseVisitorHelper OnData(Action<WebResponseData> f)
        {
            _onData = f;
            return this;
        }

        public async Task Visit(WebResponseData data)
        {
            if (null == _onData)
                Assert.Fail("Encountered object {0}", data);

            _onData(data);
        }

        private Action<WebResponsePage> _onPage;

        public ResponseVisitorHelper OnPage(Action<WebResponsePage> f)
        {
            _onPage = f;
            return this;
        }

        public async Task Visit(WebResponsePage page)
        {
            if (null == _onPage)
                Assert.Fail("Encountered object {0}", page);

            _onPage(page);
        }
        // ReSharper restore CSharpWarnings::CS1998
    }
}
