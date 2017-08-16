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

namespace Zu.Chrome
{
    public class AsyncChromeDriver : IAsyncWebBrowserClient
    {
        public FrameTracker FrameTracker;
        private bool isConnected = false;

        public ChromeDevTools DevTools { get; set; }
        public WebView WebView { get; set; }


        public string FilesBasePath { get; set; } = "js\\";
        public Contexts CurrentContext { get; set; }
        public int Port { get; set; }
        public IMouse Mouse { get; set; }
        public IKeyboard Keyboard { get; set; }
        public Session Session { get; private set; }
        public ElementCommands ElementCommands { get; private set; }
        public ElementUtils ElementUtils { get; private set; }
        public WindowCommands WindowCommands { get; private set; }
        public string UserDir { get; set; }
        public bool DoConnectWhenCheckConnected { get; set; } = true;

        static int sessionId = 0;
        public bool IsTempUserDir { get; set; } = false;
        public ChromeProcessInfo chromeProcess;
        public delegate void DevToolsEventHandler(object sender, string methodName, JToken eventData);
        public event DevToolsEventHandler DevToolsEvent;

        public AsyncChromeDriver(bool openInTempDir = true)
            : this(11000 + new Random().Next(2000))
        {
            if (openInTempDir)
            {
                IsTempUserDir = true;
                UserDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            }
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

        public AsyncChromeDriver(int port)
        {
            CurrentContext = Contexts.Chrome;
            Port = port;
            DevTools = new ChromeDevTools(Port);
            CreateDriverCore();
        }

        public void CreateDriverCore()
        {
            Session = new Session(sessionId++);
            FrameTracker = new FrameTracker(DevTools);
            WebView = new WebView(DevTools, FrameTracker, this);
            Mouse = new ChromeDriverMouse(WebView, Session);
            Keyboard = new ChromeDriverKeyboard(WebView);
            ElementUtils = new ElementUtils(WebView, Session);
            ElementCommands = new ElementCommands(WebView, Session, ElementUtils, this);
            WindowCommands = new WindowCommands(WebView, Session, this);
        }

        public virtual async Task<string> Connect(CancellationToken cancellationToken = default(CancellationToken))
        {
            UnsubscribeDevToolsSessionEvent();
            DoConnectWhenCheckConnected = false;
            if (!string.IsNullOrWhiteSpace(UserDir)) chromeProcess = await OpenChromeProfile(UserDir);
            await DevTools.Connect();
            SubscribeToDevToolsSessionEvent();
            await FrameTracker.Enable();
            return $"Connected to Chrome port {Port}";
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

        private async Task<ChromeProcessInfo> OpenChromeProfile(string userDir)
        {
            ChromeProcessInfo res = null;
            await Task.Run(() => res = ChromeProfilesWorker.OpenChromeProfile(userDir, Port));
            return res;
        }

        public async Task<string> Disconnect(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() => DevTools.Disconnect());
            isConnected = false;
            DoConnectWhenCheckConnected = true;
            return "ok";
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
            if(chromeProcess?.ProcWithJobObject != null)
            {
                chromeProcess.ProcWithJobObject.TerminateProc();
            }
            chromeProcess = null;
            await Task.Delay(1000);
            if (IsTempUserDir)
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
            return "ok";
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

        public Task<string> AcceptDialog(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void AddEventListener(string to, Action<JToken> action)
        {
            throw new NotImplementedException();
        }

        public Task<JToken> AddSendEventFunc(string name = "top.zuSendEvent", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> AddSendEventFuncIfNo(string name = "top.zuSendEvent", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> ClearElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> ClearImportedScripts(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> ClickElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementCommands.ClickElement(elementId);
        }


        public Task<string> CloseChromeWindow(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> DismissDialog(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> Eval(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> Eval2(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> EvalFile(string fileName, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> EvalInChrome(string expression, string fileName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> EvalInContent(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> ExecuteScript(string script, string filename = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> FindElement(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return WindowCommands.FindElement(strategy, expr, startNode, null, cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return WindowCommands.FindElements(strategy, expr, startNode, null, cancellationToken);
        }

        public Task<string> GetActiveElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementUtils.GetActiveElement();
        }

        public Task<string> GetActiveFrame(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> GetChromeWindowHandle(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> GetChromeWindowHandles(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> GetClientContext(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Contexts> GetContext(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUrl(CancellationToken cancellationToken = default(CancellationToken))
        {
            return WindowCommands.GetCurrentUrl();
        }

        public Task<string> GetElementAttribute(string elementId, string attrName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementUtils.GetElementAttribute(elementId, attrName);
        }

        public Task<string> GetElementProperty(string elementId, string propName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<WebRect> GetElementRect(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementUtils.GetElementRegion(elementId);
        }

        public async Task<string> GetElementTagName(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await ElementUtils.GetElementTagName(elementId);
            return res.ToString();
        }

        public Task<string> GetElementText(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> GetElementValueOfCssProperty(string elementId, string propName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetPageSource(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WindowCommands.GetPageSource(null, cancellationToken);
            return res;
        }

        public Task<string> GetString(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTextFromDialog(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTitle(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<string> GoToUrl(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await WindowCommands.GoToUrl(url, null, cancellationToken);
        }

        public Task<string> GetWindowHandle(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> GetWindowHandles(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> GetWindowPosition(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> GetWindowSize(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> GetWindowType(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<string> GoBack(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WindowCommands.GoBack(Session, cancellationToken);
            Session?.SwitchToTopFrame();
            return res;
        }

        public async Task<string> GoForward(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WindowCommands.GoForward(Session, cancellationToken);
            Session?.SwitchToTopFrame();
            return res;
        }

        public Task<string> ImportScript(string script, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsElementDisplayed(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementUtils.IsElementDisplayed(elementId);
        }

        public Task<bool> IsElementEnabled(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementUtils.IsElementEnabled(elementId);
        }

        public Task<bool> IsElementSelected(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementUtils.IsOptionElementSelected(elementId);
        }

        public Task<string> MaximizeWindow(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> ObjectExists(string path, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<string> Refresh(CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await WebView.Reload(cancellationToken);
            Session?.SwitchToTopFrame();
            return res;
        }

        public void RemoveEventListener(Action<JToken> action)
        {
            throw new NotImplementedException();
        }

        public Task<JToken> SendEvent(string to, string mess, string funcname = "top.zuSendEvent", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SendKeysToDialog(string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SendKeysToElement(string elementId, string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementCommands.SendKeysToElement(elementId, value);
        }

        public Task<string> SessionTearDown(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> SetContext(Contexts context, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> SetContextChrome(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> SetContextContent(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SetTimeouts(TimeoutType elementId, int ms, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SetWindowPosition(int x, int y, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SetWindowSize(int width, int height, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SwitchToFrame(string frameId, string element = null, bool doFocus = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SwitchToParentFrame(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<string> SwitchToWindow(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<JToken> ExecuteScript(string script, string filename = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<JToken> ExecuteAsyncScript(string script, string filename = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<WebPoint> GetElementLocation(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementCommands.GetElementLocation(elementId);
        }

        public Task<WebSize> GetElementSize(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ElementUtils.GetElementSize(elementId);
        }

        public Task<string> SubmitElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<Screenshot> TakeScreenshot(string elementId, string highlights, string full, string hash, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (DevTools?.Session == null) return null;
            var screenshot = await DevTools.Session.Page.CaptureScreenshot(new BaristaLabs.ChromeDevTools.Runtime.Page.CaptureScreenshotCommand());
            return new Screenshot(screenshot?.Data);
        }
    }
}
