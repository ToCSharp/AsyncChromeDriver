// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Chrome
{
    internal class ChromeDriverTargetLocator : ITargetLocator
    {
        private AsyncChromeDriver asyncChromeDriver;

        public ChromeDriverTargetLocator(AsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public Task<string> GetWindowHandle(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetWindowHandles(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> SwitchToActiveElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IAlert> SwitchToAlert(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SwitchToDefaultContent(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SwitchToFrame(int frameIndex, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SwitchToFrame(string frameName, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(frameName))
            {
                asyncChromeDriver.Session?.SwitchToTopFrame();
                return Task.CompletedTask;
            }
            throw new NotImplementedException("SwitchToFrame by frameName");
        }

        public async Task SwitchToFrameByElement(string element, CancellationToken cancellationToken = default(CancellationToken))
        {
            var is_displayed = await asyncChromeDriver.ElementUtils.IsElementDisplayed(element, cancellationToken);
            throw new NotImplementedException("SwitchToFrame by element");
        }

        public Task SwitchToParentFrame(CancellationToken cancellationToken = default(CancellationToken))
        {
            asyncChromeDriver.Session?.SwitchToParentFrame();
            return Task.CompletedTask;
        }

        public Task SwitchToWindow(string windowName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}