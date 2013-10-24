using System.Threading.Tasks;

namespace SocialToolBox.Core.Web.Response
{
    public interface IWebResponseVisitor
    {
        Task Visit(WebResponseRedirect redirect);
        Task Visit(WebResponseJson json);
        Task Visit(WebResponseHtml html);
        Task Visit(WebResponseData data);
        Task Visit(WebResponsePage page);
    }
}
