// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Zu.WebBrowser.BrowserOptions;

namespace Zu.Chrome
{
    public class ChromeDriverWebStorage: IWebStorage
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverWebStorage(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public ILocalStorage LocalStorage => throw new System.NotImplementedException();

        public ISessionStorage SessionStorage => throw new System.NotImplementedException();
    }
}