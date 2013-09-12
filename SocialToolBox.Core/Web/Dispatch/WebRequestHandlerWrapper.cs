using System.Data;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web.Dispatch
{
    /// <summary>
    /// Non-generic wrapper around a request handler. Used to store
    /// different request handlers in a single dispatcher.
    /// </summary>
    public abstract class WebRequestHandlerWrapper
    {
        /// <summary>
        /// All verbs for which this handler is triggered.
        /// </summary>
        public readonly HttpVerb AcceptableVerbs;

        protected WebRequestHandlerWrapper(HttpVerb verbs)
        {
            AcceptableVerbs = verbs;
        }

        /// <summary>
        /// Processes a request. Returns null if this handler should not handle
        /// this request (either because the arguments could not be parsed, or because
        /// the verb is not acceptable). Throws an exception if anything else fails.
        /// </summary>
        public abstract WebResponse Process(IWebRequest request);

        /// <summary>
        /// Wrap a request handler to make its generic type disappear.
        /// </summary>
        public static WebRequestHandlerWrapper Wrap<TArg>(HttpVerb verbs, IWebRequestHandler<TArg> handler)
            where TArg : class, IWebUrlArgument
        {
            return new Implementation<TArg>(verbs, handler);
        }        

        private class Implementation<TArg> : WebRequestHandlerWrapper
            where TArg : class, IWebUrlArgument
        {
            private readonly IWebRequestHandler<TArg> _handler; 

            public Implementation(HttpVerb verbs, IWebRequestHandler<TArg> handler) : base(verbs)
            {
                _handler = handler;
            }

            public override WebResponse Process(IWebRequest request)
            {
                if ((request.Verb & AcceptableVerbs) == 0) return null;

                WebResponse response;

                // Handlers are not re-entrant, so lock them whenever they
                // are used.
                // TODO: solve the handler re-entrance performance issue
                lock (_handler)
                {
                    var args = _handler.Parse(request);
                    if (null == args) return null;

                    response = _handler.Process(request, args);
                }

                if (null == response) 
                    throw new NoNullAllowedException(
                        string.Format("{0}.Process should not return null.", _handler.GetType()));

                return response;
            }
        }
    }

}
