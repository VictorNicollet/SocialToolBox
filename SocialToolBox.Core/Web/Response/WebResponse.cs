using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Web.Response
{
    /// <summary>
    /// A base web response class. Extended by several 
    /// possible response classes. Uses a visitor pattern for the
    /// actual sending.
    /// </summary>
    public abstract class WebResponse : IDisposable
    {
        /// <summary>
        /// All the cookies added to this web response.
        /// </summary>
        private readonly List<WebResponseCookie> _cookies = new List<WebResponseCookie>();

        /// <summary>
        /// All the cookies added to this web response.
        /// </summary>
        public IEnumerable<WebResponseCookie> Cookies { get { return _cookies; } }

        public void AddCookie(string cookie, string domain, string value, TimeSpan? expires)
        {
            _cookies.Add(new WebResponseCookie(cookie, domain, value, expires));
        }

        /// <summary>
        /// The status code.
        /// </summary>
        public readonly int Code;

        /// <summary>
        /// The visitor used to send this web response.
        /// </summary>
        private readonly IWebResponseVisitor _sender;

        protected WebResponse(int code, IWebResponseVisitor sender)
        {
            Code = code;
            _sender = sender;
        }

        public Task Send()
        {
            return Visit(_sender);
        }

        /// <summary>
        /// Implementation of the web response visitor pattern, to 
        /// allow access to the various response types.
        /// </summary>
        protected abstract Task Visit(IWebResponseVisitor visitor);

        public abstract void Dispose();
    }
}
