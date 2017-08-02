// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.

using BaristaLabs.ChromeDevTools.Runtime.Runtime;
using BaristaLabs.ChromeDevTools.Runtime.Page;
using BaristaLabs.ChromeDevTools.Runtime.Input;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace Zu.Chrome.DriverCore
{
    public class WebView
    {
        public ChromeDevTools DevTools;
        private FrameTracker FrameTracker;
        private AsyncChromeDriver asyncChromeDriver;

        public WebView(ChromeDevTools devTools, FrameTracker frameTracker, AsyncChromeDriver asyncChromeDriver)
        {
            DevTools = devTools;
            FrameTracker = frameTracker;
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<EvaluateCommandResponse> CallFunction(
                                         string function,
                                         /*JToken*/string/*[]*/ args,
                                         string frame = null,
                                         bool returnByValue = true,
                                         bool w3c = false,
                                         CancellationToken cancellationToken = default(CancellationToken))
        {
            var json = args == null ? "" : args.ToString();
            var expression = $"({call_function.JsSource}).apply(null, [null, {function}, [{args}], {w3c.ToString().ToLower()}])";
            var res = await EvaluateScript(expression, frame, returnByValue, cancellationToken);
            //var res = await devTools?.Session.Runtime.CallFunctionOn(new CallFunctionOnCommand
            //{
            //    FunctionDeclaration = function,
            //    Arguments = args,

            //});
            return res;
        }

        public async Task<EvaluateCommandResponse> EvaluateScript(
                                        string expression,
                                        string frame = null,
                                        bool returnByValue = true,
                                        CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncChromeDriver.CheckConnected();
            var contextId = string.IsNullOrWhiteSpace(frame) ? null : (long?)FrameTracker.GetContextIdForFrame(frame);
            return await DevTools?.Session.Runtime.Evaluate(new EvaluateCommand
            {
                Expression = expression,
                ContextId = contextId,
                ReturnByValue = returnByValue
            }, cancellationToken);

        }
        public async Task<string> GetUrl(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncChromeDriver.CheckConnected();
            var res = await DevTools?.Session.Page.GetNavigationHistory(new GetNavigationHistoryCommand(), cancellationToken);
            return res.Entries?.ElementAtOrDefault((int)res.CurrentIndex)?.Url;
        }
        public async Task<NavigateCommandResponse> Load(string url, int? timeout = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncChromeDriver.CheckConnected();
            var res = await DevTools?.Session.Page.Navigate(new NavigateCommand
            {
                Url = url
            }, cancellationToken, timeout);
            return res;
        }
        public async Task<string> Reload(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncChromeDriver.CheckConnected();
            var res = await DevTools?.Session.Page.Reload(new ReloadCommand(), cancellationToken);
            return res.ToString();
        }
        public async Task<NavigateToHistoryEntryCommandResponse> TraverseHistory(int delta, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncChromeDriver.CheckConnected();
            var res = await DevTools?.Session.Page.GetNavigationHistory(new GetNavigationHistoryCommand(), cancellationToken);
            if (delta == -1)
            {
                if (res.CurrentIndex > 0)
                {
                    return await DevTools?.Session.Page.NavigateToHistoryEntry(new NavigateToHistoryEntryCommand { EntryId = res.CurrentIndex + delta }, cancellationToken);
                    //return await EvaluateScript("window.history.back();");
                }
                else { return null; }
            }
            else if (delta == 1)
                if (res.CurrentIndex + 1 < res.Entries.Count())
                {
                    return await DevTools?.Session.Page.NavigateToHistoryEntry(new NavigateToHistoryEntryCommand { EntryId = res.CurrentIndex + delta }, cancellationToken);
                    //return await EvaluateScript("window.history.forward();");
                }
                else { return null; }
            else return null;
            //else throw new ArgumentOutOfRangeException(nameof(delta));
        }

        internal string SetFileInputFiles(string elementId, string keys, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<EvaluateCommandResponse> TraverseHistoryWithJavaScript(int delta, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (delta == -1)
                return EvaluateScript("window.history.back();", null, true, cancellationToken);
            else if (delta == 1)
                return EvaluateScript("window.history.forward();", null, true, cancellationToken);
            else return null;
            //else throw new ArgumentOutOfRangeException(nameof(delta));    
        }
   
        public async Task DispatchKeyEvents(string keys, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncChromeDriver.CheckConnected();
            foreach (var key in keys)
            {
                //var index = (int)key - 0xE000U; > 0
                if (AsyncWebDriver.Keys.KeyToVirtualKeyCode.ContainsKey(key))
                {
                    var virtualKeyCode = AsyncWebDriver.Keys.KeyToVirtualKeyCode[key];
                    //if (index == 7 || index == 6)
                    //{
                    //    await DevTools?.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand
                    //    {
                    //        Type = "char",
                    //        Text = Convert.ToString("\r", CultureInfo.InvariantCulture)
                    //    });
                    //}
                    //else
                    //{
                    if (virtualKeyCode == 0) continue;
                        var res = await DevTools?.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand
                        {
                            Type = "rawKeyDown",
                            //NativeVirtualKeyCode = virtualKeyCode,
                            WindowsVirtualKeyCode = virtualKeyCode,
                        }, cancellationToken);
                        await DevTools?.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand
                        {
                            Type = "keyUp",
                            //NativeVirtualKeyCode = virtualKeyCode,
                            WindowsVirtualKeyCode = virtualKeyCode,
                        }, cancellationToken);
                    //}
                }
                else
                {
                    await DevTools?.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand
                    {
                        Type = "char",
                        Text = Convert.ToString(key, CultureInfo.InvariantCulture)
                    }, cancellationToken);
                }
            }
        }
    }
}
