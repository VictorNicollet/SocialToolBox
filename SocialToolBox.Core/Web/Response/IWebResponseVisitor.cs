namespace SocialToolBox.Core.Web.Response
{
    public interface IWebResponseVisitor
    {
        void Visit(WebResponseRedirect redirect);
        void Visit(WebResponseJson json);
        void Visit(WebResponseHtml html);
        void Visit(WebResponseData data);
    }
}
