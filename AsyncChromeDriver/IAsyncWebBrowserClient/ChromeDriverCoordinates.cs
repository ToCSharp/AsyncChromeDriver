// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    internal class ChromeDriverCoordinates: ICoordinates
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverCoordinates(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public string AuxiliaryLocator => throw new System.NotImplementedException();

        public Task<WebPoint> LocationInDom(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<WebPoint> LocationInViewport(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<WebPoint> LocationOnScreen(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}