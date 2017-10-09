// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeWSProxyConfig
    {
        public string DevToolsFilesDir { get; set; }
        public int HTTPServerPort { get; set; } = 0;
    }
}