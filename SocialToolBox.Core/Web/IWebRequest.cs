using System.IO;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// A web request, as received from the client.
    /// </summary>
    public interface IWebRequest
    {
        /// <summary>
        /// The domain name on which the request was received.
        /// </summary>
        string Domain { get; }

        /// <summary>
        /// The full path on which the request was received.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The segments of the path that were not matched by the request handler's prefix.
        /// </summary>
        string[] UnmatchedPath { get; }

        /// <summary>
        /// Get the current value of a cookie.
        /// </summary>
        string Cookie(string name);

        /// <summary>
        /// The value of the named POST argument.
        /// </summary>
        string Post(string name);

        /// <summary>
        /// The value of the named GET argument.
        /// </summary>
        string Get(string name);

        /// <summary>
        /// In POST or PUT, the raw payload received by the server.
        /// </summary>
        string Payload { get; }

        /// <summary>
        /// The visitor used to send responses.
        /// </summary>
        IWebResponseVisitor ResponseSender { get; }
    }
}
