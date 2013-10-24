using System.IO;
using System.Threading.Tasks;

namespace SocialToolBox.Core.Web.Response
{
    public class WebResponseData : WebResponse
    {
        /// <summary>
        /// The filename (present only if downloaded as attachment).
        /// </summary>
        public readonly string Filename;

        /// <summary>
        /// The mime-type of the download.
        /// </summary>
        public readonly string MimeType;

        /// <summary>
        /// The stream containing the data to be sent.
        /// </summary>
        public readonly Stream Stream;

        public WebResponseData(Stream stream, string filename, string mimetype, int code, IWebResponseVisitor sender) : base(code, sender)
        {
            Stream = stream;
            Filename = filename;
            MimeType = mimetype;
        }

        protected override Task Visit(IWebResponseVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override void Dispose()
        {
            Stream.Dispose();
        }
    }
}
