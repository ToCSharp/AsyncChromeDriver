// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Zu.Chrome.DriverCore;
using Zu.WebBrowser;

namespace Zu.Chrome
{
    public interface IAsyncChromeDriver : IAsyncWebBrowserClient
    {
        ChromeDriverConfig Config { get; set; }

        ChromeDevToolsConnection DevTools { get; set; }

        FrameTracker FrameTracker { get; }
        Session Session { get; }
        WebView WebView { get; }
        ElementCommands ElementCommands { get; }
        ElementUtils ElementUtils { get; }
        WindowCommands WindowCommands { get; }
    }
}