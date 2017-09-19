// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Chrome
{
    internal class ChromeDriverTimeouts: ITimeouts
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverTimeouts(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public Task<TimeSpan> GetAsynchronousJavaScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetImplicitWait(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetPageLoad(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SetAsynchronousJavaScript(TimeSpan time, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SetImplicitWait(TimeSpan implicitWait, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SetPageLoad(TimeSpan time, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}