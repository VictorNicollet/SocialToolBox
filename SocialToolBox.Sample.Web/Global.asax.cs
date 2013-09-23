using System.Web;
using SocialToolBox.Core.Web;
using SocialToolBox.Core.Web.IIS;

namespace SocialToolBox.Sample.Web
{
    public class Global : HttpApplication, IApplicationWithDispatcher
    {
        /// <summary>
        /// Instantiate all modules in the application.
        /// </summary>
        private readonly Modules _applicationModules = new Modules();

        /// <summary>
        /// Returns the dispatcher from the application modules.
        /// </summary>
        public IWebDriver Dispatcher
        {
            get
            {
                return _applicationModules.Web;
            }
        }
    }
}