using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SocialToolBox.Core.Web.IIS
{
    /// <summary>
    /// The asynchronous action launched by the <see cref="HttpHandler"/>.
    /// Dispatches the request through the web driver.
    /// </summary>
    public class AsyncHttpAction : IAsyncResult
    {
        /// <summary>
        /// The task that performs the action.
        /// </summary>
        /// <remarks>
        /// It's easier to use tasks than to perform actual thread manipulation
        /// here.
        /// </remarks>
        private readonly Task _task;

        public AsyncHttpAction(IWebDriver driver, HttpContext context, AsyncCallback cb, object extraData)
        {
            AsyncState = extraData;

            _task = new Task(() =>
            {
                if (driver == null)
                {
                    var appWithDispatcher = context.ApplicationInstance as IApplicationWithDispatcher;
                    if (appWithDispatcher == null)
                        throw new MissingMemberException("Application does not implement IApplicationWithDispatcher");
                    driver = appWithDispatcher.Dispatcher;
                }

                var response = driver.Dispatch(new WebRequest(context));
                if (response != null) response.Send();

                cb(this);
            });
        }

        /// <summary>
        /// Start processing the action.
        /// </summary>
        public void Start()
        {
            _task.Start();
        }

        public bool IsCompleted { get { return _task.IsCompleted; } }
        public WaitHandle AsyncWaitHandle { get { return null; } }
        public object AsyncState { get; private set; }
        public bool CompletedSynchronously { get { return false; } }
    }
}
