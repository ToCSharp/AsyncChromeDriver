// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeDriverConfig : DriverConfig
    {
        public bool DoOpenWSProxy { get; set; } = false;
        public ChromeWSProxyConfig WSProxyConfig { get; set; }

        public ChromeDriverConfig()
            : base()
        {

        }

        public ChromeDriverConfig(DriverConfig config)
            : this()
        {
            UserDir = config.UserDir;
            IsTempProfile = config.IsTempProfile;
            IsDefaultProfile = config.IsDefaultProfile;
            TempDirCreateDelay = config.TempDirCreateDelay;
            Port = config.Port;
            Headless = config.Headless;
            WindowSize = config.WindowSize;
            DoNotOpenChromeProfile = config.DoNotOpenChromeProfile;
        }

    }
    public static class ChromeDriverConfigFluent
    {
        public static T SetDoOpenWSProxy<T>(this T dc, ChromeWSProxyConfig wsProxyConfig = null) where T : ChromeDriverConfig
        {
            dc.DoOpenWSProxy = true;
            dc.WSProxyConfig = wsProxyConfig ?? new ChromeWSProxyConfig();
            return dc;
        }

    }
}