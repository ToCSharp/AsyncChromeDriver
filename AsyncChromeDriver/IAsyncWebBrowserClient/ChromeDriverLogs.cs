// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Chrome
{
    internal class ChromeDriverLogs: ILogs
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverLogs(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public Task<ReadOnlyCollection<string>> AvailableLogTypes(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<ReadOnlyCollection<LogEntry>> GetLog(string logKind, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}