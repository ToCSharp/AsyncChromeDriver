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
        public IMouse Mouse { get { if (mouse == null) mouse = new ChromeDriverMouse(this); return mouse; } }

        public IKeyboard Keyboard { get { if (keyboard == null) keyboard = new ChromeDriverKeyboard(this); return keyboard; } }

        public IOptions Options { get { if (options == null) options = new ChromeDriverOptions(this); return options; } }

        public IAlert Alert { get { if (alert == null) alert = new ChromeDriverAlert(this); return alert; } }

        public ICoordinates Coordinates { get { if (coordinates == null) coordinates = new ChromeDriverCoordinates(this); return coordinates; } }

        public ITakesScreenshot Screenshot { get { if (screenshot == null) screenshot = new ChromeDriverScreenshot(this); return screenshot; } }

        public ITouchScreen TouchScreen { get { if (touchScreen == null) touchScreen = new ChromeDriverTouchScreen(this); return touchScreen; } }

        public INavigation Navigation { get { if (navigation == null) navigation = new ChromeDriverNavigation(this); return navigation; } }

        public IJavaScriptExecutor JavaScriptExecutor { get { if (javaScriptExecutor == null) javaScriptExecutor = new ChromeDriverJavaScriptExecutor(this); return javaScriptExecutor; } }

        public ITargetLocator TargetLocator { get { if (targetLocator == null) targetLocator = new ChromeDriverTargetLocator(this); return targetLocator; } }

        public IElements Elements { get { if (elements == null) elements = new ChromeDriverElements(this); return elements; } }

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
        #endregion


        private bool isConnected = false;

        public ChromeDevToolsConnection DevTools { get; set; }

        public FrameTracker FrameTracker { get; private set; }
        public Session Session { get; private set; }
        public WebView WebView { get; private set; }
        public ElementCommands ElementCommands { get; private set; }
        public ElementUtils ElementUtils { get; private set; }
        public WindowCommands WindowCommands { get; private set; }

        public ChromeDriverConfig Config { get; set; }
        public int Port { get => Config.Port; set => Config.Port = value; }
        public string UserDir { get => Config.UserDir; set => Config.SetUserDir(value); }
        public bool IsTempProfile { get => Config.IsTempProfile; set => Config.IsTempProfile = value; }

        public bool DoConnectWhenCheckConnected { get; set; } = true;

        static int sessionId = 0;

        public ChromeProcessInfo chromeProcess;
        private bool _isClosed = false;

        public delegate void DevToolsEventHandler(object sender, string methodName, JToken eventData);
        public event DevToolsEventHandler DevToolsEvent;

        public AsyncChromeDriver(bool openInTempDir = true)
            : this(11000 + new Random().Next(2000))
        {
            Config.SetIsTempProfile(openInTempDir);
        }
        public AsyncChromeDriver(string profileDir, int port)
            : this(port)
        {
            UserDir = profileDir;
        }

        public AsyncChromeDriver(string profileDir)
            : this(11000 + new Random().Next(2000))
        {
            UserDir = profileDir;
        }

        public AsyncChromeDriver(DriverConfig config)
            : this(new ChromeDriverConfig(config))
        {
        }

        public AsyncChromeDriver(ChromeDriverConfig config)
        {
            Config = config;
            if (Config.Port == 0) Config.Port = 11000 + new Random().Next(2000);
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
            WebView = new WebView(DevTools, FrameTracker, this);
            //Mouse = new ChromeDriverMouse(WebView, Session);
            //Keyboard = new ChromeDriverKeyboard(WebView);
            //Options = new BrowserOptions();
            ElementUtils = new ElementUtils(WebView, Session);
            ElementCommands = new ElementCommands(WebView, Session, ElementUtils, this);
            WindowCommands = new WindowCommands(WebView, Session, this);
        }

        public virtual async Task<string> Connect(CancellationToken cancellationToken = default(CancellationToken))
        {
            UnsubscribeDevToolsSessionEvent();
            DoConnectWhenCheckConnected = false;
            if (!Config.DoNotOpenChromeProfile)
            {
                chromeProcess = await OpenChromeProfile(Config);
                if (Config.IsTempProfile) await Task.Delay(Config.TempDirCreateDelay);
            }
            int connection_attempts = 0;
            const int MAX_ATTEMPTS = 5;
            while (true)
            {
                connection_attempts++;
                try
                {
                    await DevTools.Connect();
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
                        await Task.Delay(200);
                    }
                }
            }
            SubscribeToDevToolsSessionEvent();
            await FrameTracker.Enable();
            return $"Connected to Chrome port {Port}";
        }

        public async Task CheckConnected(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!DoConnectWhenCheckConnected) return;
            DoConnectWhenCheckConnected = false;
            if (!isConnected)
            {
                isConnected = true;
                await Connect(cancellationToken);
            }
        }

        public async Task<ChromeProcessInfo> OpenChromeProfile(ChromeDriverConfig config)
        {
            ChromeProcessInfo res = null;
            await Task.Run(() => res = ChromeProfilesWorker.OpenChromeProfile(config)); // userDir, Port, isHeadless));
            return res;
        }

        public void CloseSync()
        {
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
                    if (Directory.Exists(UserDir)) Directory.Delete(UserDir, true);
                }
                catch
                {
                    Thread.Sleep(3000);
                    try
                    {
                        if (Directory.Exists(UserDir)) Directory.Delete(UserDir, true);
                    }
                    catch { }
                }

            }

        }

        public async Task<string> Close(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (isConnected) await Disconnect();
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
                    await Task.Delay(250);
                }
            }
            chromeProcess?.Proc?.Dispose();
            if (chromeProcess?.ProcWithJobObject != null)
            {
                chromeProcess.ProcWithJobObject.TerminateProc();
            }
            chromeProcess = null;
            await Task.Delay(1000);
            if (IsTempProfile && !string.IsNullOrWhiteSpace(UserDir))
            {
                try
                {
                    if (Directory.Exists(UserDir)) Directory.Delete(UserDir, true);
                }
                catch
                {
                    await Task.Delay(3000);
                    try
                    {
                        if (Directory.Exists(UserDir)) Directory.Delete(UserDir, true);
                    }
                    catch { }
                }

            }
            return "ok";
        }

        public async Task<string> GetPageSource(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WindowCommands.GetPageSource(null, cancellationToken);
            return res;
        }

        public async Task<string> GetTitle(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WindowCommands.GetTitle(null, cancellationToken);
            return res;
        }

        protected void SubscribeToDevToolsSessionEvent()
        {
            DevTools.Session.DevToolsEvent += DevToolsSessionEvent;
        }

        protected void UnsubscribeDevToolsSessionEvent()
        {
            if (DevTools.Session != null) DevTools.Session.DevToolsEvent -= DevToolsSessionEvent;
        }

        private void DevToolsSessionEvent(object sender, string methodName, JToken eventData)
        {
            DevToolsEvent?.Invoke(sender, methodName, eventData);
        }

        public async Task Disconnect(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() => DevTools.Disconnect());
            isConnected = false;
            //DoConnectWhenCheckConnected = true;
        }



        public async Task<DevToolsCommandResult> SendDevToolsCommand(DevToolsCommandData commandData, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var res = await DevTools.Session?.SendCommand(commandData.CommandName, commandData.Params, cancellationToken, commandData.MillisecondsTimeout);
                return new DevToolsCommandResult
                {
                    Id = commandData.Id,
                    Result = res
                };
            }
            catch (Exception ex)
            {
                return new DevToolsCommandResult
                {
                    Id = commandData.Id,
                    Error = ex.ToString()
                };
            }
        }

    }
}
