// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.ChromeDevTools.Runtime;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome.DriverCore
{
    public class ElementCommands
    {
        private WebView webView;
        private ElementUtils elementUtils;
        private AsyncChromeDriver asyncChromeDriver;
        public Session Session
        {
            get;
            private set;
        }

        public ElementCommands(AsyncChromeDriver asyncChromeDriver)
        {
            this.webView = asyncChromeDriver.WebView;
            Session = asyncChromeDriver.Session;
            this.elementUtils = asyncChromeDriver.ElementUtils;
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<string> ClickElement(string elementId)
        {
            if (asyncChromeDriver != null)
                await asyncChromeDriver.CheckConnected().ConfigureAwait(false);
            var tag_name = await elementUtils.GetElementTagName(elementId).ConfigureAwait(false);
            if (tag_name == "option")
            {
                bool is_toggleable = await elementUtils.IsOptionElementTogglable(elementId).ConfigureAwait(false);
                if (is_toggleable)
                {
                    await elementUtils.ToggleOptionElement(elementId).ConfigureAwait(false);
                    return "ToggleOptionElement";
                }
                else
                {
                    await elementUtils.SetOptionElementSelected(elementId).ConfigureAwait(false);
                    return "SetOptionElementSelected";
                }
            }
            else
            {
                WebPoint location = await elementUtils.GetElementClickableLocation(elementId).ConfigureAwait(false);
                var res = await webView.DevTools.Session.Input.DispatchMouseEvent(new ChromeDevTools.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.MovedMouseEventType, Button = ChromeDriverMouse.NoneMouseButton, X = location.X, Y = location.Y, Modifiers = Session.sticky_modifiers, ClickCount = 0}).ConfigureAwait(false);
                res = await webView.DevTools.Session.Input.DispatchMouseEvent(new ChromeDevTools.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.PressedMouseEventType, Button = ChromeDriverMouse.LeftMouseButton, X = location.X, Y = location.Y, Modifiers = Session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
                res = await webView.DevTools.Session.Input.DispatchMouseEvent(new ChromeDevTools.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.ReleasedMouseEventType, Button = ChromeDriverMouse.LeftMouseButton, X = location.X, Y = location.Y, Modifiers = Session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
                Session.mouse_position = location;
                //await new ChromeDriverMouse(webView).Click(location);
                return "Click";
            }
        }

        public async Task<WebPoint> GetElementLocation(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.CallFunction(atoms.GET_LOCATION, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", asyncChromeDriver.Session.GetCurrentFrameId()).ConfigureAwait(false);
            return ResultValueConverter.ToWebPoint(res?.Result?.Value);
        }

        internal Task<string> GetElementValueOfCssProperty(string elementId, string propertyName, CancellationToken cancellationToken = default (CancellationToken))
        {
            return elementUtils.GetElementEffectiveStyle(elementId, propertyName);
        }

        public async Task<EvaluateCommandResponse> FocusElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.CallFunction(focus_js.JsSource, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task<EvaluateCommandResponse> ClearElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.CallFunction(atoms.CLEAR, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task<EvaluateCommandResponse> SubmitElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.CallFunction(atoms.SUBMIT, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task<string> SendKeysToElement(string elementId, string keys, CancellationToken cancellationToken = default (CancellationToken))
        {
            var isInput = await elementUtils.IsElementAttributeEqualToIgnoreCase(elementId, "tagName", "input", cancellationToken).ConfigureAwait(false);
            var isFile = await elementUtils.IsElementAttributeEqualToIgnoreCase(elementId, "type", "file", cancellationToken).ConfigureAwait(false);
            if (isInput && isFile)
            {
                bool multiple = await elementUtils.IsElementAttributeEqualToIgnoreCase(elementId, "multiple", "true", cancellationToken).ConfigureAwait(false);
                return webView.SetFileInputFiles(elementId, keys);
            }
            else
            {
                var startTime = DateTime.Now;
                bool isDisplayed = false;
                bool isFocused = false;
                if (Session.ImplicitWait != default (TimeSpan))
                {
                    while (true)
                    {
                        isDisplayed = await elementUtils.IsElementDisplayed(elementId, cancellationToken).ConfigureAwait(false);
                        if (isDisplayed)
                            break;
                        isFocused = await elementUtils.IsElementFocused(elementId, cancellationToken).ConfigureAwait(false);
                        if (isFocused)
                            break;
                        if (Session.ImplicitWait == default (TimeSpan))
                            break;
                        if (DateTime.Now - startTime >= Session.ImplicitWait)
                        {
                            throw new WebBrowserException("Element is not displayed or focused", "invalid element state");
                        }

                        await Task.Delay(100).ConfigureAwait(false);
                    }
                }

                bool isEnabled = await elementUtils.IsElementEnabled(elementId, cancellationToken).ConfigureAwait(false);
                if (!isEnabled)
                    throw new WebBrowserException("Element is not enabled", "invalid element state");
                if (!isFocused)
                {
                    await FocusElement(elementId, cancellationToken).ConfigureAwait(false);
                }

                await webView.DispatchKeyEvents(keys).ConfigureAwait(false);
                return "ok";
            }
        }
    }
}