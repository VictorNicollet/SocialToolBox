namespace SocialToolBox.Core.Web.Response
{
    /// <summary>
    /// A web response that carries an HTML payload.
    /// </summary>
    public class WebResponseHtml : WebResponse
    {
        /// <summary>
        /// The MIME-type with which HTML content is served.
        /// </summary>
        public const string MimeType = "text/html";
        
        /// <summary>
        /// The HTML payload of this response.
        /// </summary>
        public readonly string Html;

        public WebResponseHtml(string html, int code, IWebResponseVisitor sender) : base(code, sender)
        {
            Html = html;
        }

        protected override void Visit(IWebResponseVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Dispose()
        {
        }
    }
}
