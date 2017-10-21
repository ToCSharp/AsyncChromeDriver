// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zu.Chrome.DriverCore;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Chrome
{
    public class ChromeDriverTargetLocator : ITargetLocator
    {
        private AsyncChromeDriver asyncChromeDriver;
        public ChromeDriverTargetLocator(AsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public Task<string> GetWindowHandle(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetWindowHandles(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> SwitchToActiveElement(CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IAlert> SwitchToAlert(CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SwitchToDefaultContent(CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task SwitchToFrame(int frameIndex, CancellationToken cancellationToken = default (CancellationToken))
        {
            var script = "function(xpath) {" + "  return document.evaluate(xpath, document, null, " + "      XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;" + "}";
            var xpath = "(/html/body//iframe|/html/frameset//frame)";
            xpath += $"[{(frameIndex + 1).ToString()}]";
            var args = new List<string>{xpath}; //$"\"{xpath}\"" };
            try
            {
                var frame = await asyncChromeDriver.WebView.GetFrameByFunction(asyncChromeDriver.Session.GetCurrentFrameId(), script, args, cancellationToken).ConfigureAwait(false);
                var argsJson = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                var res = await asyncChromeDriver.WebView.CallFunction(script, argsJson, asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
                var elementId = ResultValueConverter.ToElementId(res?.Result?.Value, asyncChromeDriver.Session.GetElementKey());
                var chromeDriverId = Util.GenerateId();
                var kSetFrameIdentifier = "function(frame, id) {" + "  frame.setAttribute('cd_frame_id_', id);" + "}";
                argsJson = $"{asyncChromeDriver.Session.GetElementJsonString(elementId)}, \"{chromeDriverId}\"";
                var res2 = await asyncChromeDriver.WebView.CallFunction(kSetFrameIdentifier, argsJson, asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
                asyncChromeDriver.Session.SwitchToSubFrame(frame, chromeDriverId);
            }
            catch
            {
                throw;
            }
        }

        public async Task SwitchToFrame(string frameName, CancellationToken cancellationToken = default (CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(frameName))
            {
                asyncChromeDriver.Session?.SwitchToTopFrame();
                return;
            }

            var script = "function(xpath) {" + "  return document.evaluate(xpath, document, null, " + "      XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;" + "}";
            var xpath = "(/html/body//iframe|/html/frameset//frame)";
            xpath += $"[@name=\"{frameName}\" or @id=\"{frameName}\"]";
            var args = new List<string>{xpath};
            try
            {
                var frame = await asyncChromeDriver.WebView.GetFrameByFunction(asyncChromeDriver.Session.GetCurrentFrameId(), script, args, cancellationToken).ConfigureAwait(false);
                var argsJson = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                var res = await asyncChromeDriver.WebView.CallFunction(script, argsJson, asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
                var elementId = ResultValueConverter.ToElementId(res?.Result?.Value, asyncChromeDriver.Session.GetElementKey());
                var chromeDriverId = Util.GenerateId();
                var kSetFrameIdentifier = "function(frame, id) {" + "  frame.setAttribute('cd_frame_id_', id);" + "}";
                argsJson = $"{asyncChromeDriver.Session.GetElementJsonString(elementId)}, \"{chromeDriverId}\"";
                var res2 = await asyncChromeDriver.WebView.CallFunction(kSetFrameIdentifier, argsJson, asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
                asyncChromeDriver.Session.SwitchToSubFrame(frame, chromeDriverId);
            }
            catch
            {
                throw;
            }
        }

        public async Task SwitchToFrameByElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var is_displayed = await asyncChromeDriver.ElementUtils.IsElementDisplayed(elementId, cancellationToken).ConfigureAwait(false);
            var script = "function(elem) { return elem; }";
            var args = new List<string>{$"{{\"{asyncChromeDriver.Session.GetElementKey()}\":\"{elementId}\"}}"};
            try
            {
                var frame = await asyncChromeDriver.WebView.GetFrameByFunction(asyncChromeDriver.Session.GetCurrentFrameId(), script, args, cancellationToken).ConfigureAwait(false);
                var argsJson = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                var res = await asyncChromeDriver.WebView.CallFunction(script, argsJson, asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
                var elementId2 = ResultValueConverter.ToElementId(res?.Result?.Value, asyncChromeDriver.Session.GetElementKey());
                var chromeDriverId = Util.GenerateId();
                var kSetFrameIdentifier = "function(frame, id) {" + "  frame.setAttribute('cd_frame_id_', id);" + "}";
                argsJson = $"{asyncChromeDriver.Session.GetElementJsonString(elementId2)}, \"{chromeDriverId}\"";
                var res2 = await asyncChromeDriver.WebView.CallFunction(kSetFrameIdentifier, argsJson, asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
                asyncChromeDriver.Session.SwitchToSubFrame(frame, chromeDriverId);
            }
            catch
            {
                throw;
            }
        }

        public Task SwitchToParentFrame(CancellationToken cancellationToken = default (CancellationToken))
        {
            asyncChromeDriver.Session?.SwitchToParentFrame();
            return Task.CompletedTask;
        }

        public Task SwitchToWindow(string windowName, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}