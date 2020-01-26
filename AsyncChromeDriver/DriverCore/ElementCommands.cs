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
        private WebView _webView;
        private ElementUtils _elementUtils;
        private AsyncChromeDriver _asyncChromeDriver;
        public Session Session
        {
            get;
            private set;
        }

        public ElementCommands(AsyncChromeDriver asyncChromeDriver)
        {
            _webView = asyncChromeDriver.WebView;
            Session = asyncChromeDriver.Session;
            _elementUtils = asyncChromeDriver.ElementUtils;
            _asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<string> ClickElement(string elementId)
        {
            if (_asyncChromeDriver != null)
                await _asyncChromeDriver.CheckConnected().ConfigureAwait(false);
            var tagName = await _elementUtils.GetElementTagName(elementId).ConfigureAwait(false);
            if (tagName == "option")
            {
                bool isToggleable = await _elementUtils.IsOptionElementTogglable(elementId).ConfigureAwait(false);
                if (isToggleable)
                {
                    await _elementUtils.ToggleOptionElement(elementId).ConfigureAwait(false);
                    return "ToggleOptionElement";
                }
                else
                {
                    await _elementUtils.SetOptionElementSelected(elementId).ConfigureAwait(false);
                    return "SetOptionElementSelected";
                }
            }
            else
            {
                WebPoint location = await _elementUtils.GetElementClickableLocation(elementId).ConfigureAwait(false);
                await _webView.DevTools.Input.DispatchMouseEvent(new ChromeDevTools.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.MovedMouseEventType, Button = ChromeDevTools.Input.MouseButton.None, X = location.X, Y = location.Y, Modifiers = Session.StickyModifiers, ClickCount = 0}).ConfigureAwait(false);
                await _webView.DevTools.Input.DispatchMouseEvent(new ChromeDevTools.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.PressedMouseEventType, Button = ChromeDevTools.Input.MouseButton.Left, X = location.X, Y = location.Y, Modifiers = Session.StickyModifiers, ClickCount = 1}).ConfigureAwait(false);
                await _webView.DevTools.Input.DispatchMouseEvent(new ChromeDevTools.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.ReleasedMouseEventType, Button = ChromeDevTools.Input.MouseButton.Left, X = location.X, Y = location.Y, Modifiers = Session.StickyModifiers, ClickCount = 1}).ConfigureAwait(false);
                Session.MousePosition = location;
                //await new ChromeDriverMouse(webView).Click(location);
                return "Click";
            }
        }

        public async Task<WebPoint> GetElementLocation(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await _webView.CallFunction(atoms.GET_LOCATION, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", _asyncChromeDriver.Session.GetCurrentFrameId(), cancellationToken: cancellationToken).ConfigureAwait(false);
            return ResultValueConverter.ToWebPoint(res?.Result?.Value);
        }

        internal Task<string> GetElementValueOfCssProperty(string elementId, string propertyName, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _elementUtils.GetElementEffectiveStyle(elementId, propertyName, cancellationToken);
        }

        public async Task<EvaluateCommandResponse> FocusElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await _webView.CallFunction(focus_js.JsSource, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", _asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task<EvaluateCommandResponse> ClearElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await _webView.CallFunction(atoms.CLEAR, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", _asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task<EvaluateCommandResponse> SubmitElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await _webView.CallFunction(atoms.SUBMIT, $"{{\"{Session.GetElementKey()}\":\"{elementId}\"}}", _asyncChromeDriver.Session.GetCurrentFrameId(), true, false, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task<string> SendKeysToElement(string elementId, string keys, CancellationToken cancellationToken = default (CancellationToken))
        {
            var isInput = await _elementUtils.IsElementAttributeEqualToIgnoreCase(elementId, "tagName", "input", cancellationToken).ConfigureAwait(false);
            var isFile = await _elementUtils.IsElementAttributeEqualToIgnoreCase(elementId, "type", "file", cancellationToken).ConfigureAwait(false);
            if (isInput && isFile)
            {
                bool multiple = await _elementUtils.IsElementAttributeEqualToIgnoreCase(elementId, "multiple", "true", cancellationToken).ConfigureAwait(false);
                return _webView.SetFileInputFiles(elementId, keys);
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
                        isDisplayed = await _elementUtils.IsElementDisplayed(elementId, cancellationToken).ConfigureAwait(false);
                        if (isDisplayed)
                            break;
                        isFocused = await _elementUtils.IsElementFocused(elementId, cancellationToken).ConfigureAwait(false);
                        if (isFocused)
                            break;
                        if (Session.ImplicitWait == default (TimeSpan))
                            break;
                        if (DateTime.Now - startTime >= Session.ImplicitWait)
                        {
                            throw new WebBrowserException("Element is not displayed or focused", "invalid element state");
                        }

                        await Task.Delay(100, cancellationToken).ConfigureAwait(false);
                    }
                }

                bool isEnabled = await _elementUtils.IsElementEnabled(elementId, cancellationToken).ConfigureAwait(false);
                if (!isEnabled)
                    throw new WebBrowserException("Element is not enabled", "invalid element state");
                if (!isFocused)
                {
                    await FocusElement(elementId, cancellationToken).ConfigureAwait(false);
                }

                await _webView.DispatchKeyEvents(keys, cancellationToken).ConfigureAwait(false);
                return "ok";
            }
        }
    }
}