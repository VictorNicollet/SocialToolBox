using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web.Dispatch
{
    /// <summary>
    /// Dispatches a request to a corresponding handler among all registered 
    /// handlers.
    /// </summary>
    public class WebRequestDispatcher
    {
        /// <summary>
        /// All registered handlers.
        /// </summary>
        private readonly Dictionary<string,List<WebRequestHandlerWrapper>> _handlers =
            new Dictionary<string, List<WebRequestHandlerWrapper>>();

        /// <summary>
        /// Registers a handler for a path.
        /// </summary>
        private void Register(string path, WebRequestHandlerWrapper handler)
        {
            List<WebRequestHandlerWrapper> handlersForPath;
            if (!_handlers.TryGetValue(path, out handlersForPath))
            {
                handlersForPath = new List<WebRequestHandlerWrapper>();
                _handlers.Add(path, handlersForPath);
            }

            handlersForPath.Add(handler);
        }

        /// <summary>
        /// Registers a handler for a path and one or more verbs.
        /// </summary>
        public void Register<T>(IWebDriver driver, string path, HttpVerb verbs, Func<WebRequestHandler<T>> handler)
            where T : class, IWebUrlArgument, new()
        {
            Register(path,WebRequestHandlerWrapper.Wrap(driver, verbs, handler));
        }

        /// <summary>
        /// Finds a request handler that will return a web response for
        /// the provided web request. Returns null if none was found.
        /// </summary>
        /// <remarks>
        /// Attempts using the longest possible path prefixes first.
        /// </remarks>
        public async Task<WebResponse> Dispatch(IWebRequest request)
        {
            while (null != request)
            {
                List<WebRequestHandlerWrapper> handlersForPath;
                if (_handlers.TryGetValue(request.MatchedPath, out handlersForPath))
                {
                    foreach (var handler in handlersForPath)
                    {
                        var response = await handler.Process(request);
                        if (null != response) return response;
                    }
                }

                request = request.UnmatchOne();
            }

            return null;
        }
    }
}
