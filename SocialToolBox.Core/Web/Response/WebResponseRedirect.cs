using System.Threading.Tasks;

namespace SocialToolBox.Core.Web.Response
{
    public class WebResponseRedirect : WebResponse
    {
        /// <summary>
        /// The URL to which the redirect occurs.
        /// </summary>
        public readonly string Url;

        public WebResponseRedirect(string url, int code, IWebResponseVisitor sender) : base(code, sender)
        {
            Url = url;
        }

        protected override Task Visit(IWebResponseVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override void Dispose()
        {
        }
    }
}
