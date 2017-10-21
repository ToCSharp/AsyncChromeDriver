// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zu.Chrome.DriverCore
{
    public class WindowCommands
    {
        private WebView webView;
        private Session session;
        private AsyncChromeDriver asyncChromeDriver;
        public WindowCommands(AsyncChromeDriver asyncChromeDriver)
        {
            this.webView = asyncChromeDriver.WebView;
            this.session = asyncChromeDriver.Session;
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<string> GoToUrl(string url, string frame = null, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.Load(url, null, cancellationToken).ConfigureAwait(false);
            return res.FrameId;
        }

        public async Task<string> GetCurrentUrl(string frame = null)
        {
            //var res = (await webView.CallFunction(
            //   "function() { return document.URL; }", null, frame))?.Result?.Value;
            //var url = (res as JObject)?["value"]?.ToString() ?? res?.ToString();
            var res = await webView.EvaluateScript("document.URL;", frame).ConfigureAwait(false);
            var url = res.Result?.Value?.ToString() ?? res.ExceptionDetails?.ToString();
            return url;
        }

        public async Task<string> GetPageSource(string frame = null, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.EvaluateScript("new XMLSerializer().serializeToString(document);", frame, true, cancellationToken).ConfigureAwait(false);
            return res.Result?.Value?.ToString() ?? res.ExceptionDetails?.ToString();
        }

        public async Task<string> GetTitle(string frame = null, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.EvaluateScript("document.title", frame, true, cancellationToken).ConfigureAwait(false);
            return res.Result?.Value?.ToString() ?? res.ExceptionDetails?.ToString();
        }

        public async Task<JToken> FindElement(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var func = atoms.FIND_ELEMENT;
            var frameId = session == null ? "" : session.GetCurrentFrameId();
            expr = Regex.Replace(expr, @"(['""\\#.:;,!?+<>=~*^$|%&@`{}\-/\[\]\(\)])", @"\$1");
            var args = $"{{\"{strategy}\":\"{expr}\"}}";
            if (startNode != null)
                args += $", {{\"{session.GetElementKey()}\":\"{startNode}\"}}";
            var res = await webView.CallFunction(func, args, frameId, true, false, cancellationToken).ConfigureAwait(false);
            var value = res?.Result?.Value as JToken;
            var exception = ResultValueConverter.ToWebBrowserException(value);
            if (exception != null)
                throw exception;
            return value;
        }

        public async Task<JToken> FindElements(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var func = atoms.FIND_ELEMENTS;
            var frameId = session == null ? "" : session.GetCurrentFrameId();
            expr = Regex.Replace(expr, @"(['""\\#.:;,!?+<>=~*^$|%&@`{}\-/\[\]\(\)])", @"\$1");
            var args = $"{{\"{strategy}\":\"{expr}\"}}";
            if (startNode != null)
                args += $", {{\"{session.GetElementKey()}\":\"{startNode}\"}}";
            var res = await webView.CallFunction(func, args, frameId, true, false, cancellationToken).ConfigureAwait(false);
            var value = res?.Result?.Value as JToken;
            var exception = ResultValueConverter.ToWebBrowserException(value);
            if (exception != null)
                throw exception;
            return value?["value"];
        }

        public async Task<string> GoBack(CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.TraverseHistory(-1, cancellationToken).ConfigureAwait(false);
            session?.SwitchToTopFrame();
            return "ok";
        }

        public async Task<string> GoForward(CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.TraverseHistory(1, cancellationToken).ConfigureAwait(false);
            session?.SwitchToTopFrame();
            return "ok";
        }

        public async Task<JToken> ExecuteScript(string script, List<string> args = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var frameId = session == null ? "" : session.GetCurrentFrameId();
            var func = "function(){" + script + "}";
            var argsStr = args?.Any() == true ? string.Join(", ", args) : "";
            var res = await webView.CallFunction(func, argsStr, frameId, cancellationToken: cancellationToken).ConfigureAwait(false);
            return res?.Result?.Value as JToken;
        }

        public async Task<JToken> ExecuteAsyncScript(string script, List<string> args = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var frameId = session == null ? "" : session.GetCurrentFrameId();
            var func = "function(){" + script + "}";
            var argsStr = args?.Any() == true ? string.Join(", ", args) : "";
            var res = await webView.CallUserAsyncFunction(func, argsStr, session.ScriptTimeout, cancellationToken: cancellationToken).ConfigureAwait(false);
            return res as JToken;
        }
    }
}