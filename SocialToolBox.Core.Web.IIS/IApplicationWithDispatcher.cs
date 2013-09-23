namespace SocialToolBox.Core.Web.IIS
{
    /// <summary>
    /// Marks an application as having a public request 
    /// dispatcher, which can be used by HTTP modules.
    /// </summary>
    public interface IApplicationWithDispatcher
    {
        IWebDriver Dispatcher { get; }
    }
}
