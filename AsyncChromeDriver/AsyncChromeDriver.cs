// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser;
using Zu.Chrome.DriverCore;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.AsyncInteractions;
using System.IO;
using Zu.Chrome.DevTools;
using System.Linq;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Chrome
{
    public class AsyncChromeDriver : IAsyncChromeDriver
    {
        #region IAsyncWebBrowserClient
        public IMouse Mouse => mouse ?? (mouse = new ChromeDriverMouse(this));

        public IKeyboard Keyboard => keyboard ?? (keyboard = new ChromeDriverKeyboard(this));

        public IOptions Options => options ?? (options = new ChromeDriverOptions(this));

        public IAlert Alert => alert ?? (alert = new ChromeDriverAlert(this));

        public ICoordinates Coordinates => coordinates ?? (coordinates = new ChromeDriverCoordinates(this));

        public ITakesScreenshot Screenshot => screenshot ?? (screenshot = new ChromeDriverScreenshot(this));

        public ITouchScreen TouchScreen => touchScreen ?? (touchScreen = new ChromeDriverTouchScreen(this));

        public INavigation Navigation => navigation ?? (navigation = new ChromeDriverNavigation(this));

        public IJavaScriptExecutor JavaScriptExecutor => javaScriptExecutor ?? (javaScriptExecutor = new ChromeDriverJavaScriptExecutor(this));

        public ITargetLocator TargetLocator => targetLocator ?? (targetLocator = new ChromeDriverTargetLocator(this));

        public IElements Elements => elements ?? (elements = new ChromeDriverElements(this));

        public IActionExecutor ActionExecutor => actionExecutor ?? (actionExecutor = new ChromeDriverActionExecutor(this));

        private ChromeDriverNavigation navigation;
        private ChromeDriverTouchScreen touchScreen;
        private ChromeDriverScreenshot screenshot;
        private ChromeDriverCoordinates coordinates;
        private ChromeDriverAlert alert;
        private ChromeDriverOptions options;
        private ChromeDriverKeyboard keyboard;
        private ChromeDriverMouse mouse;
        private ChromeDriverJavaScriptExecutor javaScriptExecutor;
        private ChromeDriverTargetLocator targetLocator;
        private ChromeDriverElements elements;
        private ChromeDriverActionExecutor actionExecutor;
        #endregion

        public bool isConnected = false;
        public ChromeDevToolsConnection DevTools
        {
            get;
            set;
        }

        public FrameTracker FrameTracker
        {
            get;
            private set;
        }

        public DomTracker DomTracker
        {
            get;
            private set;
        }

        public Session Session
        {
            get;
            private set;
        }

        public WebView WebView
        {
            get;
            private set;
        }

        public ElementCommands ElementCommands
        {
            get;
            private set;
        }

        public ElementUtils ElementUtils
        {
            get;
            private set;
        }

        public WindowCommands WindowCommands
        {
            get;
            private set;
        }

        public ChromeDriverConfig Config
        {
            get;
            set;
        }

        public int Port
        {
            get => Config.Port;
            set => Config.Port = value;
        }

        public string UserDir
        {
            get => Config.UserDir;
            set => Config.SetUserDir(value);
        }

        public bool IsTempProfile
        {
            get => Config.IsTempProfile;
            set => Config.IsTempProfile = value;
        }

        public bool DoConnectWhenCheckConnected
        {
            get;
            set;
        } = true;

        static int sessionId = 0;
        public ChromeProcessInfo chromeProcess;
        private bool _isClosed = false;
        public delegate void DevToolsEventHandler(object sender, string methodName, JToken eventData);
        public event DevToolsEventHandler DevToolsEvent;
        public AsyncChromeDriver BrowserDevTools
        {
            get;
            set;
        }

        public ChromeDriverConfig BrowserDevToolsConfig
        {
            get;
            set;
        }

        static Random rnd = new Random();
        public AsyncChromeDriver(bool openInTempDir = true) : this(11000 + rnd.Next(2000))
        {
            Config.SetIsTempProfile(openInTempDir);
        }

        public AsyncChromeDriver(string profileDir, int port) : this(port)
        {
            UserDir = profileDir;
        }

        public AsyncChromeDriver(string profileDir) : this(11000 + rnd.Next(2000))
        {
            UserDir = profileDir;
        }

        public AsyncChromeDriver(DriverConfig config) : this(config as ChromeDriverConfig ?? new ChromeDriverConfig(config))
        {
        }

        public AsyncChromeDriver(ChromeDriverConfig config)
        {
            Config = config;
            if (Config.Port == 0)
                Config.Port = 11000 + rnd.Next(2000);
            if (Config.DoOpenWSProxy || Config.DoOpenBrowserDevTools)
            {
                if (Config.DevToolsConnectionProxyPort == 0)
                    Config.DevToolsConnectionProxyPort = 15000 + rnd.Next(2000);
                DevTools = new BrowserDevTools.ChromeDevToolsConnectionProxy(Port, Config.DevToolsConnectionProxyPort, Config.WSProxyConfig);
            }
            else
                DevTools = new ChromeDevToolsConnection(Port);
            CreateDriverCore();
        }

        public AsyncChromeDriver(int port)
        {
            Config = new ChromeDriverConfig();
            Port = port;
            DevTools = new ChromeDevToolsConnection(Port);
            CreateDriverCore();
        }

        public void CreateDriverCore()
        {
            Session = new Session(sessionId++, this);
            FrameTracker = new FrameTracker(DevTools);
            DomTracker = new DomTracker(DevTools);
            WebView = new WebView(DevTools, FrameTracker, this);
            //Mouse = new ChromeDriverMouse(WebView, Session);
            //Keyboard = new ChromeDriverKeyboard(WebView);
            //Options = new BrowserOptions();
            ElementUtils = new ElementUtils(WebView, Session);
            ElementCommands = new ElementCommands(this);
            WindowCommands = new WindowCommands(this);
        }

        public virtual async Task<string> Connect(CancellationToken cancellationToken = default(CancellationToken))
        {
            isConnected = true;
            UnsubscribeDevToolsSessionEvent();
            DoConnectWhenCheckConnected = false;
            if (!Config.DoNotOpenChromeProfile)
            {
                chromeProcess = await OpenChromeProfile(Config).ConfigureAwait(false);
                if (Config.IsTempProfile)
                    await Task.Delay(Config.TempDirCreateDelay).ConfigureAwait(false);
            }

            int connection_attempts = 0;
            const int MAX_ATTEMPTS = 5;
            while (true)
            {
                connection_attempts++;
                try
                {
                    await DevTools.Connect().ConfigureAwait(false);
                    break;
                }
                catch (Exception ex)
                {
                    //LiveLogger.WriteLine("Connection attempt {0} failed with: {1}", connection_attempts, ex);
                    if (_isClosed || connection_attempts >= MAX_ATTEMPTS)
                    {
                        throw;
                    }
                    else
                    {
                        await Task.Delay(200).ConfigureAwait(false);
                    }
                }
            }

            SubscribeToDevToolsSessionEvent();
            await FrameTracker.Enable().ConfigureAwait(false);
            await DomTracker.Enable().ConfigureAwait(false);
            if (Config.DoOpenBrowserDevTools)
                await OpenBrowserDevTools().ConfigureAwait(false);
            return $"Connected to Chrome port {Port}";
        }

        public string GetBrowserDevToolsUrl()
        {
            var httpPort = Config?.WSProxyConfig?.DoProxyHttpTraffic == true ? Config.WSProxyConfig.HTTPServerPort : Port;
            return "http://127.0.0.1:" + httpPort + "/devtools/inspector.html?ws=127.0.0.1:" + Config.DevToolsConnectionProxyPort + "/WSProxy";
        }

        public virtual async Task OpenBrowserDevTools()
        {
            if (BrowserDevToolsConfig == null)
                BrowserDevToolsConfig = new ChromeDriverConfig();
            BrowserDevTools = new AsyncChromeDriver(BrowserDevToolsConfig);
            await BrowserDevTools.Navigation.GoToUrl(GetBrowserDevToolsUrl()).ConfigureAwait(false);
        }

        public async Task CheckConnected(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!DoConnectWhenCheckConnected)
                return;
            DoConnectWhenCheckConnected = false;
            if (!isConnected)
            {
                await Connect(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<ChromeProcessInfo> OpenChromeProfile(ChromeDriverConfig config)
        {
            ChromeProcessInfo res = null;
            await Task.Run(() => res = ChromeProfilesWorker.OpenChromeProfile(config)).ConfigureAwait(false); // userDir, Port, isHeadless));
            return res;
        }

        public void CloseSync()
        {
            BrowserDevTools?.CloseSync();
            if (isConnected)
            {
                DevTools.Disconnect();
                isConnected = false;
            }

            if (chromeProcess?.Proc != null && !chromeProcess.Proc.HasExited)
            {
                try
                {
                    chromeProcess.Proc.CloseMainWindow();
                }
                catch
                {
                    try
                    {
                        chromeProcess.Proc.Kill();
                    }
                    catch
                    {
                    }
                }

                while (!chromeProcess.Proc.HasExited)
                {
                    Thread.Sleep(250);
                }
            }

            chromeProcess?.Proc?.Dispose();
            if (chromeProcess?.ProcWithJobObject != null)
            {
                chromeProcess.ProcWithJobObject.TerminateProc();
            }

            chromeProcess = null;
            Thread.Sleep(1000);
            if (IsTempProfile && !string.IsNullOrWhiteSpace(UserDir))
            {
                try
                {
                    if (Directory.Exists(UserDir))
                        Directory.Delete(UserDir, true);
                }
                catch
                {
                    Thread.Sleep(3000);
                    try
                    {
                        if (Directory.Exists(UserDir))
                            Directory.Delete(UserDir, true);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public async Task<string> Close(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (BrowserDevTools != null)
                    await BrowserDevTools.Close(cancellationToken).ConfigureAwait(false);
            }
            catch
            {
            }

            if (isConnected)
                await Disconnect().ConfigureAwait(false);
            if (chromeProcess?.Proc != null && !chromeProcess.Proc.HasExited)
            {
                try
                {
                    chromeProcess.Proc.CloseMainWindow();
                }
                catch
                {
                    try
                    {
                        chromeProcess.Proc.Kill();
                    }
                    catch
                    {
                    }
                }

                while (!chromeProcess.Proc.HasExited)
                {
                    await Task.Delay(250).ConfigureAwait(false);
                }
            }

            chromeProcess?.Proc?.Dispose();
            if (chromeProcess?.ProcWithJobObject != null)
            {
                chromeProcess.ProcWithJobObject.TerminateProc();
            }

            chromeProcess = null;
            await Task.Delay(1000).ConfigureAwait(false);
            if (IsTempProfile && !string.IsNullOrWhiteSpace(UserDir))
            {
                try
                {
                    if (Directory.Exists(UserDir))
                        Directory.Delete(UserDir, true);
                }
                catch
                {
                    await Task.Delay(3000).ConfigureAwait(false);
                    try
                    {
                        if (Directory.Exists(UserDir))
                            Directory.Delete(UserDir, true);
                    }
                    catch
                    {
                    }
                }
            }

            return "ok";
        }

        public async Task<string> GetPageSource(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WindowCommands.GetPageSource(null, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task<string> GetTitle(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WindowCommands.GetTitle(null, cancellationToken).ConfigureAwait(false);
            return res;
        }

        protected void SubscribeToDevToolsSessionEvent()
        {
            DevTools.Session.DevToolsEvent += DevToolsSessionEvent;
        }

        protected void UnsubscribeDevToolsSessionEvent()
        {
            if (DevTools.Session != null)
                DevTools.Session.DevToolsEvent -= DevToolsSessionEvent;
        }

        private void DevToolsSessionEvent(object sender, string methodName, JToken eventData)
        {
            DevToolsEvent?.Invoke(sender, methodName, eventData);
        }

        public async Task Disconnect(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() => DevTools.Disconnect()).ConfigureAwait(false);
            isConnected = false;
            //DoConnectWhenCheckConnected = true;
        }

        public async Task<DevToolsCommandResult> SendDevToolsCommand(DevToolsCommandData commandData, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var res = await DevTools.Session.SendCommand(commandData.CommandName, commandData.Params, cancellationToken, commandData.MillisecondsTimeout).ConfigureAwait(false);
                return new DevToolsCommandResult { Id = commandData.Id, Result = res };
            }
            catch (Exception ex)
            {
                return new DevToolsCommandResult { Id = commandData.Id, Error = ex.ToString() };
            }
        }
    }
}