namespace SocialToolBox.Core.Web.Response
{
    /// <summary>
    /// A response that carries a JSON payload.
    /// </summary>
    public class WebResponseJson : WebResponse
    {
        /// <summary>
        /// The MIME-type to be used with this payload.
        /// </summary>
        public const string MimeType = "application/json";

        /// <summary>
        /// The returned JSON payload.
        /// </summary>
        public readonly string Json;

        public WebResponseJson(string json, int code, IWebResponseVisitor sender) : base(code, sender)
        {
            Json = json;
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
