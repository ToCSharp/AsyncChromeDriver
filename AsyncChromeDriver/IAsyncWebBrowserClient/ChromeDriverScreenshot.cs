// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeDriverScreenshot : ITakesScreenshot
    {
        private IAsyncChromeDriver asyncChromeDriver;
        public ChromeDriverScreenshot(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<Screenshot> GetScreenshot(CancellationToken cancellationToken = default (CancellationToken))
        {
            if (asyncChromeDriver?.DevTools?.Session == null)
                return null;
            var screenshot = await asyncChromeDriver.DevTools.Session.Page.CaptureScreenshot().ConfigureAwait(false);
            return new Screenshot(screenshot?.Data);
        }
    }
}