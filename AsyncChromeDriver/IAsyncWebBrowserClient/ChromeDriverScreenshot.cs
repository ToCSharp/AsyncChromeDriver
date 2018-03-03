// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeDriverScreenshot : ITakesScreenshot
    {
        private IAsyncChromeDriver _asyncChromeDriver;
        public ChromeDriverScreenshot(IAsyncChromeDriver asyncChromeDriver)
        {
            _asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<Screenshot> GetScreenshot(CancellationToken cancellationToken = default (CancellationToken))
        {
            if (_asyncChromeDriver?.DevTools?.Session == null)
                return null;
            var screenshot = await _asyncChromeDriver.DevTools.Page.CaptureScreenshot().ConfigureAwait(false);
            return new Screenshot(screenshot?.Data);
        }
    }
}