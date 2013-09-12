namespace SocialToolBox.Core.Web.Response
{
    public interface IWebResponseVisitor
    {
        void Visit(WebResponseRedirect redirect);
        void Visit(WebResponseJson redirect);
        void Visit(WebResponseHtml redirect);
        void Visit(WebResponseData redirect);
    }
}
