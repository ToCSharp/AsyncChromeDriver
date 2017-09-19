// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Chrome
{
    internal class ChromeDriverJavaScriptExecutor: IJavaScriptExecutor
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverJavaScriptExecutor(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<object> ExecuteAsyncScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            return await asyncChromeDriver.WindowCommands.ExecuteAsyncScript(script, args.Select(v => v.ToString()).ToList());
        }

        public async Task<object> ExecuteScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            return await asyncChromeDriver.WindowCommands.ExecuteScript(script, args.Select(v => v.ToString()).ToList());
        }
    }
}