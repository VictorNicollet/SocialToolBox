using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SocialToolBox.Core.Present;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web.IIS
{
    /// <summary>
    /// Translates an internal web response into an IIS web 
    /// response in the current HTTP context. 
    /// </summary>
    public class WebResponseVisitor : IWebResponseVisitor
    {
        /// <summary>
        /// The current HTTP response.
        /// </summary>
        public readonly HttpResponse Response;

        public WebResponseVisitor(HttpContext context)
        {
            Response = context.Response;
        }

        /// <summary>
        /// Prepares the response by applying the status code and cookies.
        /// </summary>
        private void Prepare(WebResponse response)
        {
            Response.StatusCode = response.Code;
            foreach (var cookie in response.Cookies)
            {
                var iisCookie = new HttpCookie(cookie.Name, cookie.Value)
                {
                    Domain = cookie.Domain,
                    HttpOnly = true
                };

                if (cookie.Expires != null)
                {
                    iisCookie.Expires = DateTime.Now + cookie.Expires.Value;
                }

                Response.AppendCookie(iisCookie);
            }
        }

        // ReSharper disable CSharpWarnings::CS1998
        public async Task Visit(WebResponseRedirect redirect)
        {
            Prepare(redirect);            
            Response.Redirect(redirect.Url, true);
            Response.Flush();
        }

        public async Task Visit(WebResponseJson json)
        {
            Prepare(json);
            Response.ContentType = "application/json";
            var bytes = Encoding.UTF8.GetBytes(json.Json);
            Response.BinaryWrite(bytes);
            Response.Flush();
        }

        public async Task Visit(WebResponseHtml html)
        {
            Prepare(html);
            Response.ContentType = "text/html";
            var bytes = Encoding.UTF8.GetBytes(html.Html);
            Response.BinaryWrite(bytes);
            Response.Flush();
        }

        public async Task Visit(WebResponseData data)
        {
            Prepare(data);
            Response.ContentType = data.MimeType;
            if (data.Filename != null) 
                Response.AddHeader("Content-Disposition","attachment; filename=" + data.Filename);
            data.Stream.CopyTo(Response.OutputStream);
            Response.Flush();
        }
        // ReSharper restore CSharpWarnings::CS1998

        public async Task Visit(WebResponsePage page)
        {
            var output = new HtmlOutput();
            page.Page.RenderWith(page.Renderer, output);
            Response.ContentType = "text/html";

            var html = await output.Build();
            var bytes = Encoding.UTF8.GetBytes(html.ToString());
            Response.BinaryWrite(bytes);
            Response.Flush();
        }
    }
}
