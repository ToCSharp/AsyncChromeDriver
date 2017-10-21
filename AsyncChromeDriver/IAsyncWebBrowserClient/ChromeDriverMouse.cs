// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.Chrome.DriverCore;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Chrome
{
    public class ChromeDriverMouse : IMouse
    {
        public const string MovedMouseEventType = "mouseMoved";
        public const string ReleasedMouseEventType = "mouseReleased";
        public const string PressedMouseEventType = "mousePressed";
        public const string LeftMouseButton = "left";
        public const string MiddleMouseButton = "middle";
        public const string RightMouseButton = "right";
        public const string NoneMouseButton = "none";
        public const string TouchStart = "touchStart";
        public const string TouchEnd = "touchEnd";
        public const string TouchMove = "touchMove";
        public const string PointStateTouchStart = "touchPressed";
        public const string PointStateTouchEnd = "touchReleased";
        public const string PointStateTouchMove = "touchMoved";
        private WebView webView;
        private Session session;
        private IAsyncChromeDriver asyncChromeDriver;
        public ChromeDriverMouse(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
            webView = asyncChromeDriver.WebView;
            session = asyncChromeDriver.Session;
        }

        public Task Click(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task ContextClick(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DoubleClick(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task MouseDown(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task MouseMove(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task MouseMove(ICoordinates where, int offsetX, int offsetY, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task MouseUp(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task MouseMove(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.MovedMouseEventType, Button = ChromeDriverMouse.NoneMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 0}).ConfigureAwait(false);
            session.mouse_position = location;
        }

        public async Task Click(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            if (session.mouse_position != location)
            {
                await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.MovedMouseEventType, Button = ChromeDriverMouse.NoneMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 0}).ConfigureAwait(false);
            }

            await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.PressedMouseEventType, Button = ChromeDriverMouse.LeftMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
            await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.ReleasedMouseEventType, Button = ChromeDriverMouse.LeftMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
            session.mouse_position = location;
        }

        public async Task ContextClick(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.MovedMouseEventType, Button = ChromeDriverMouse.NoneMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 0}).ConfigureAwait(false);
            await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.PressedMouseEventType, Button = ChromeDriverMouse.RightMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
            await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.ReleasedMouseEventType, Button = ChromeDriverMouse.RightMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
            session.mouse_position = location;
        }

        public async Task DoubleClick(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await Click(location, cancellationToken).ConfigureAwait(false);
            await Click(location, cancellationToken).ConfigureAwait(false);
        }

        public async Task MouseDown(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            if (session.mouse_position != location)
            {
                await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.MovedMouseEventType, Button = ChromeDriverMouse.NoneMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 0}).ConfigureAwait(false);
            }

            await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.PressedMouseEventType, Button = ChromeDriverMouse.LeftMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
            session.mouse_position = location;
        }

        public async Task MouseUp(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            if (session.mouse_position != location)
            {
                await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.MovedMouseEventType, Button = ChromeDriverMouse.NoneMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 0}).ConfigureAwait(false);
            }

            await webView.DevTools.Session.Input.DispatchMouseEvent(new BaristaLabs.ChromeDevTools.Runtime.Input.DispatchMouseEventCommand{Type = ChromeDriverMouse.ReleasedMouseEventType, Button = ChromeDriverMouse.LeftMouseButton, X = location.X, Y = location.Y, Modifiers = session.sticky_modifiers, ClickCount = 1}).ConfigureAwait(false);
            session.mouse_position = location;
        }
    }
}