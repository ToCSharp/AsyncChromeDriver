// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Chrome
{
    internal class ChromeDriverNavigation: INavigation
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverNavigation(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task Back(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await asyncChromeDriver.WindowCommands.GoBack(cancellationToken);
            asyncChromeDriver.Session?.SwitchToTopFrame();
        }

        public async Task Forward(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await asyncChromeDriver.WindowCommands.GoForward(cancellationToken);
            asyncChromeDriver.Session?.SwitchToTopFrame();
        }

        public Task<string> GetUrl(CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.WindowCommands.GetCurrentUrl();
        }

        public Task GoToUrl(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.WindowCommands.GoToUrl(url, null, cancellationToken);
        }

        public Task GoToUrl(Uri url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.WindowCommands.GoToUrl(url.ToString(), null, cancellationToken);
        }

        public async Task Refresh(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await asyncChromeDriver.WebView.Reload(cancellationToken);
            asyncChromeDriver.Session?.SwitchToTopFrame();
        }
    }
}