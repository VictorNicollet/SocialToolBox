using System;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// A web response, sent back to the user browser.
    /// </summary>
    public interface IWebResponse
    {
        /// <summary>
        /// Add a cookie to a web response. Returns a new web response, rather than
        /// altering the current one.
        /// </summary>
        IWebResponse AddCookie(string cookie, string domain, DateTime? expires);

        /// <summary>
        /// Send the completed web response. This function will be called by the web
        /// driver, not by user code.
        /// </summary>
        void Send();

        /// <summary>
        /// Discard the web response, closing any attached streams. 
        /// </summary>
        void Discard();
    }
}
