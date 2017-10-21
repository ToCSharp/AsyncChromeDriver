// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using BaristaLabs.ChromeDevTools.Runtime.Input;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Zu.Chrome.DriverCore;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeDriverKeyboard : IKeyboard
    {
        private WebView webView;
        private IAsyncChromeDriver asyncChromeDriver;
        public ChromeDriverKeyboard(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
            webView = asyncChromeDriver.WebView;
        }

        public async Task PressKey(string keyToPress, CancellationToken cancellationToken = default (CancellationToken))
        {
            if (!(keyToPress.Length == 1))
                throw new ArgumentOutOfRangeException(nameof(keyToPress));
            var key = keyToPress[0];
            if (Keys.KeyToVirtualKeyCode.ContainsKey(key))
            {
                var virtualKeyCode = Keys.KeyToVirtualKeyCode[key];
                if (virtualKeyCode == 0)
                    return;
                var res = await webView.DevTools.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand{Type = "rawKeyDown", //NativeVirtualKeyCode = virtualKeyCode,
 WindowsVirtualKeyCode = virtualKeyCode, }, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await webView.DevTools.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand{Type = "keyDown", //Type = "char",
 Text = Convert.ToString(key, CultureInfo.InvariantCulture)}, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task ReleaseKey(string keyToRelease, CancellationToken cancellationToken = default (CancellationToken))
        {
            if (!(keyToRelease.Length == 1))
                throw new ArgumentOutOfRangeException(nameof(keyToRelease));
            var key = keyToRelease[0];
            if (Keys.KeyToVirtualKeyCode.ContainsKey(key))
            {
                var virtualKeyCode = Keys.KeyToVirtualKeyCode[key];
                if (virtualKeyCode == 0)
                    return;
                await webView.DevTools.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand{Type = "keyUp", //NativeVirtualKeyCode = virtualKeyCode,
 WindowsVirtualKeyCode = virtualKeyCode, }, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await webView.DevTools.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand{Type = "keyUp", //Type = "char",
 Text = Convert.ToString(key, CultureInfo.InvariantCulture)}, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task SendKeys(string keySequence, CancellationToken cancellationToken = default (CancellationToken))
        {
            foreach (var key in keySequence)
            {
                if (Keys.KeyToVirtualKeyCode.ContainsKey(key))
                {
                    var virtualKeyCode = Keys.KeyToVirtualKeyCode[key];
                    if (virtualKeyCode == 0)
                        continue;
                    var res = await webView.DevTools.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand{Type = "rawKeyDown", //NativeVirtualKeyCode = virtualKeyCode,
 WindowsVirtualKeyCode = virtualKeyCode, }, cancellationToken).ConfigureAwait(false);
                    await webView.DevTools.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand{Type = "keyUp", //NativeVirtualKeyCode = virtualKeyCode,
 WindowsVirtualKeyCode = virtualKeyCode, }, cancellationToken).ConfigureAwait(false);
                //}
                }
                else
                {
                    await webView.DevTools.Session.Input.DispatchKeyEvent(new DispatchKeyEventCommand{Type = "char", Text = Convert.ToString(key, CultureInfo.InvariantCulture)}, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}