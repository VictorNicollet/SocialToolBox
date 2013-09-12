namespace SocialToolBox.Core.Web
{
    /// <summary>
    /// A web URL argument is an object that can be written to a Web URL.
    /// </summary>
    public interface IWebUrlArgument
    {
        WebUrl AddTo(WebUrl url);
    }
}
