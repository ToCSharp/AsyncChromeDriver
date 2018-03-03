// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Chrome
{
    public class ChromeDriverTimeouts: ITimeouts
    {
        private IAsyncChromeDriver _asyncChromeDriver;

        public ChromeDriverTimeouts(IAsyncChromeDriver asyncChromeDriver)
        {
            _asyncChromeDriver = asyncChromeDriver;
        }

        public TimeSpan AsynchronousJavaScript { get; set; }
        public TimeSpan ImplicitWait { get; set; }
        public TimeSpan PageLoad { get; set; }

        public Task<TimeSpan> GetAsynchronousJavaScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(AsynchronousJavaScript);
        }

        public Task<TimeSpan> GetImplicitWait(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(ImplicitWait);
        }

        public Task<TimeSpan> GetPageLoad(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(PageLoad);
        }

        public Task SetAsynchronousJavaScript(TimeSpan time, CancellationToken cancellationToken = default(CancellationToken))
        {
            AsynchronousJavaScript = time;
            return Task.CompletedTask;
        }

        public Task SetImplicitWait(TimeSpan implicitWait, CancellationToken cancellationToken = default(CancellationToken))
        {
            ImplicitWait = implicitWait;
            return Task.CompletedTask;
        }

        public Task SetPageLoad(TimeSpan time, CancellationToken cancellationToken = default(CancellationToken))
        {
            PageLoad = time;
            return Task.CompletedTask;
        }
    }
}