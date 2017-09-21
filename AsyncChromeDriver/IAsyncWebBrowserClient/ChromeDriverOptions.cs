// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Zu.WebBrowser;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Chrome
{
    public class ChromeDriverOptions: IOptions
    {
        private IAsyncChromeDriver asyncChromeDriver;
        private ChromeDriverTimeouts timeouts;
        private ChromeDriverCookieJar cookies;
        private ChromeDriverLogs logs;
        private ChromeDriverWindow window;

        public ChromeDriverOptions(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public ICookieJar Cookies { get { if (cookies == null) cookies = new ChromeDriverCookieJar(asyncChromeDriver); return cookies; } }

        public IWindow Window { get { if (window == null) window = new ChromeDriverWindow(asyncChromeDriver); return window; } }

        public ILogs Logs { get { if (logs == null) logs = new ChromeDriverLogs(asyncChromeDriver); return logs; } }

        public ITimeouts Timeouts { get { if (timeouts == null) timeouts = new ChromeDriverTimeouts(asyncChromeDriver); return timeouts; } }

        public bool HasLocationContext => throw new System.NotImplementedException();

        public ILocationContext LocationContext => throw new System.NotImplementedException();

        public bool HasApplicationCache => throw new System.NotImplementedException();

        public IApplicationCache ApplicationCache => throw new System.NotImplementedException();

        public ILocalStorage LocalStorage => throw new System.NotImplementedException();

        public ISessionStorage SessionStorage => throw new System.NotImplementedException();
    }
}