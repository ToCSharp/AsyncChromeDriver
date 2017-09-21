// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Chrome
{
    public class ChromeDriverTouchScreen : ITouchScreen
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverTouchScreen(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public Task DoubleTap(ICoordinates where, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task Down(int locationX, int locationY, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task Flick(int speedX, int speedY, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task Flick(ICoordinates where, int offsetX, int offsetY, int speed, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task LongPress(ICoordinates where, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task Move(int locationX, int locationY, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task Scroll(ICoordinates where, int offsetX, int offsetY, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task Scroll(int offsetX, int offsetY, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task SingleTap(ICoordinates where, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task Up(int locationX, int locationY, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}