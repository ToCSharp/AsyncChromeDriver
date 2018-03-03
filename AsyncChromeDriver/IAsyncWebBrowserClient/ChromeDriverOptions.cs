// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Zu.WebBrowser.BrowserOptions;

namespace Zu.Chrome
{
    public class ChromeDriverOptions: IOptions
    {
        private IAsyncChromeDriver _asyncChromeDriver;
        private ChromeDriverTimeouts _timeouts;
        private ChromeDriverCookieJar _cookies;
        private ChromeDriverLogs _logs;
        private ChromeDriverWindow _window;

        public ChromeDriverOptions(IAsyncChromeDriver asyncChromeDriver)
        {
            _asyncChromeDriver = asyncChromeDriver;
        }

        public ICookieJar Cookies { get { if (_cookies == null) _cookies = new ChromeDriverCookieJar(_asyncChromeDriver); return _cookies; } }

        public IWindow Window { get { if (_window == null) _window = new ChromeDriverWindow(_asyncChromeDriver); return _window; } }

        public ILogs Logs { get { if (_logs == null) _logs = new ChromeDriverLogs(_asyncChromeDriver); return _logs; } }

        public ITimeouts Timeouts { get { if (_timeouts == null) _timeouts = new ChromeDriverTimeouts(_asyncChromeDriver); return _timeouts; } }

        public bool HasLocationContext => throw new System.NotImplementedException();

        public ILocationContext LocationContext => throw new System.NotImplementedException();

        public bool HasApplicationCache => throw new System.NotImplementedException();

        public IApplicationCache ApplicationCache => throw new System.NotImplementedException();

        public ILocalStorage LocalStorage => throw new System.NotImplementedException();

        public ISessionStorage SessionStorage => throw new System.NotImplementedException();
    }
}