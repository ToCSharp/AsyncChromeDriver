// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeDriverConfig : DriverConfig
    {
        public bool DoOpenWSProxy { get; set; } = false;
        public ChromeWSProxyConfig WSProxyConfig { get; set; }

        public int DevToolsConnectionProxyPort { get; set; } = 0;

        public ChromeDriverConfig()
            : base()
        {

        }

        public ChromeDriverConfig(DriverConfig config)
            : this()
        {
            UserDir = config.UserDir;
            CommandLineArgumets = config.CommandLineArgumets;
            IsTempProfile = config.IsTempProfile;
            IsDefaultProfile = config.IsDefaultProfile;
            TempDirCreateDelay = config.TempDirCreateDelay;
            Port = config.Port;
            Headless = config.Headless;
            WindowSize = config.WindowSize;
            DoNotOpenChromeProfile = config.DoNotOpenChromeProfile;
            DoOpenBrowserDevTools = config.DoOpenBrowserDevTools;
            if(config is ChromeDriverConfig chromeConfig)
            {
                DoOpenWSProxy = chromeConfig.DoOpenWSProxy;
                WSProxyConfig = chromeConfig.WSProxyConfig;
                DevToolsConnectionProxyPort = chromeConfig.DevToolsConnectionProxyPort;
            }
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

        public static T SetWSProxyConfig<T>(this T dc, ChromeWSProxyConfig wsProxyConfig = null) where T : ChromeDriverConfig
        {
            dc.DoOpenWSProxy = true;
            dc.WSProxyConfig = wsProxyConfig ?? new ChromeWSProxyConfig();
            return dc;
        }

        public static T SetDevToolsConnectionProxyPort<T>(this T dc, int devToolsConnectionProxyPort) where T : ChromeDriverConfig
        {
            dc.DoOpenWSProxy = true;
            dc.DevToolsConnectionProxyPort = devToolsConnectionProxyPort;
            return dc;
        }

    }
}