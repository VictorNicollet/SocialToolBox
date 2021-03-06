﻿using System;
using System.Data;
using System.Threading.Tasks;
using SocialToolBox.Core.Web.Response;

namespace SocialToolBox.Core.Web.Dispatch
{
    /// <summary>
    /// Non-generic wrapper around a request handler. Used to store
    /// different request handlers in a single dispatcher.
    /// </summary>
    abstract class WebRequestHandlerWrapper
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
        public abstract Task<WebResponse> Process(IWebRequest request);

        /// <summary>
        /// Wrap a request handler to make its generic type disappear.
        /// </summary>
        public static WebRequestHandlerWrapper Wrap<TArg>(IWebDriver driver, HttpVerb verbs, Func<WebRequestHandler<TArg>> handler)
            where TArg : class, IWebUrlArgument, new()
        {
            return new Implementation<TArg>(driver, verbs, handler);
        }        

        private class Implementation<TArg> : WebRequestHandlerWrapper
            where TArg : class, IWebUrlArgument, new()
        {
            private readonly Func<WebRequestHandler<TArg>> _getHandler;
            private readonly IWebDriver _driver;

            public Implementation(IWebDriver driver, HttpVerb verbs, Func<WebRequestHandler<TArg>> handler) : base(verbs)
            {
                _getHandler = handler;
                _driver = driver;
            }

            public override async Task<WebResponse> Process(IWebRequest request)
            {
                if ((request.Verb & AcceptableVerbs) == 0) return null;

                var args = new TArg();
                if (!args.TryParse(request)) return null;

                WebRequestHandler<TArg> handler;
                lock (_getHandler) handler = _getHandler();
                var response = await handler.Process(_driver, request, args);

                if (null == response) 
                    throw new NoNullAllowedException(
                        string.Format("{0}.Process should not return null.", handler.GetType()));

                return response;
            }
        }
    }

}
