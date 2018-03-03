using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Zu.AsyncWebDriver.Remote;
using Zu.Chrome;
using Zu.WebBrowser.BasicTypes;

namespace AsyncChromeDriverExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AsyncChromeDriver asyncChromeDriver;
        private ChromeProcessInfo chromeProcess;
        private WebDriver webDriver;
        private ChromeRequestListener chromeRequestListener;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                asyncChromeDriver = new AsyncChromeDriver();
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect();
                tbDevToolsRes.Text = "opened";
                tbDevToolsRes2.Text = $"opened on port {asyncChromeDriver.Port} in dir {asyncChromeDriver.UserDir} \nWhen close, dir will be DELETED";
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await webDriver?.Close();
            //await asyncChromeDriver?.Close();
            tbDevToolsRes.Text = "closed";
            tbDevToolsRes2.Text = "closed";
        }

        ObservableCollection<ResponseReceivedEventInfo> responseEvents = new ObservableCollection<ResponseReceivedEventInfo>();
        ObservableCollection<WebSocketFrameReceivedEventInfo> wsEvents = new ObservableCollection<WebSocketFrameReceivedEventInfo>();
        private AsyncChromeDriver asyncChromeDriverDev;

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lbDevToolsRequests.ItemsSource = responseEvents;
            lbDevToolsWS.ItemsSource = wsEvents;

            chromeRequestListener = new ChromeRequestListener(asyncChromeDriver);
            chromeRequestListener.ResponseReceived += (s, ev) => Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { responseEvents.Insert(0, ev); });
            chromeRequestListener.WebSocketFrameReceived += (s, ev) => Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { wsEvents.Insert(0, ev); });
            await chromeRequestListener.StartListen();
            tbDevToolsRes.Text = "enabled";

        }

        private async void Button_Click_26(object sender, RoutedEventArgs e)
        {
            if (chromeRequestListener != null)
            {
                var r = lbDevToolsRequests.SelectedItem as ResponseReceivedEventInfo;
                if (r == null) return;
                var res = await chromeRequestListener.GetCookies(r);
                if (res == null)
                {
                    tbDevToolsRes.Text = "";
                }
                else
                {
                    tbDevToolsRes.Text = string.Join(Environment.NewLine, res.Select(c => CookieToString(c))); //.ToString()));
                }
            }
        }

        private async void Button_Click_27(object sender, RoutedEventArgs e)
        {
            if (chromeRequestListener != null)
            {
                var res = await chromeRequestListener.GetAllCookies();
                if (res == null)
                {
                    tbDevToolsRes.Text = "";
                }
                else
                {
                    tbDevToolsRes.Text = string.Join(Environment.NewLine, res.Select(c => CookieToString(c)));
                }
            }
        }

        string CookieToString(Zu.ChromeDevTools.Network.Cookie c)
        {
            var c2 = new Cookie(c.Name, c.Value, c.Domain, c.Path,
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(c.Expires).ToLocalTime()); //, DateTimeOffset.FromUnixTimeMilliseconds((long)c.Expires).UtcDateTime);
            return c2.ToString();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (webDriver == null)
            {
                asyncChromeDriver = new AsyncChromeDriver();
                webDriver = new WebDriver(asyncChromeDriver);
            }
            try
            {
                await webDriver.Options().Timeouts.SetImplicitWait(TimeSpan.FromSeconds(3));
                // name = "q", 0 - time to wait element, not use ImplicitWait
                var prevQuery = await webDriver.FindElementByNameOrDefault("q", 0);
                var res2 = await webDriver.GoToUrl("https://www.google.com/");
                //var query = await webDriver.FindElementByName("q", prevQuery?.Id);
                var query = await webDriver.FindElementById("lst-ib", prevQuery?.Id);

                //await query.SendKeys("ToCSharp");
                var rnd = new Random();
                foreach (var v in "ToCSharp")
                {
                    await Task.Delay(500 + rnd.Next(1000));
                    await query.SendKeys(v.ToString());
                }
                await Task.Delay(500);
                prevQuery = await webDriver.FindElementByName("q");
                await query.SendKeys(Keys.Enter);
                query = await webDriver.FindElementByName("q", prevQuery?.Id);
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(1000);
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(2000);
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(1000);
                await query.SendKeys(Keys.ArrowUp);
                await Task.Delay(500);
                await query.SendKeys(Keys.Enter);
                var el = await webDriver.SwitchTo().ActiveElement();
                await webDriver.Keyboard.SendKeys(Keys.PageDown);
                var allCookies = await asyncChromeDriver.DevTools.Network.GetAllCookies();
                var screenshot = await asyncChromeDriver.DevTools.Page.CaptureScreenshot();
                if (!string.IsNullOrWhiteSpace(screenshot.Data))
                {
                    var dir = @"C:\temp";
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var i = 0;
                    var path = "";
                    do
                    {
                        i++;
                        path = Path.Combine(dir, $"screenshot{i}.png");
                    } while (File.Exists(path));
                    File.WriteAllBytes(path, Convert.FromBase64String(screenshot.Data));
                }

            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }
        }

        private async void lbDevToolsRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chromeRequestListener != null)
            {
                var res = await chromeRequestListener.GetResponseBody(lbDevToolsRequests.SelectedItem as ResponseReceivedEventInfo);
                tbDevToolsRes.Text = res;
            }
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (webDriver != null)
            {
                await webDriver.Keyboard.SendKeys(Keys.Up);
            }

        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (webDriver != null)
            {
                await webDriver.Keyboard.SendKeys(Keys.Down);
            }

        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (webDriver == null)
            {
                asyncChromeDriver = new AsyncChromeDriver();
                webDriver = new WebDriver(asyncChromeDriver);
            }
            try
            {
                await asyncChromeDriver.CheckConnected();
                await asyncChromeDriver.DevTools.Page.Enable();
                asyncChromeDriver.DevTools.Page.SubscribeToLoadEventFiredEvent(async (e2) =>
                {
                    var screenshot = await asyncChromeDriver.DevTools.Page.CaptureScreenshot();
                    SaveScreenshot(screenshot.Data);

                });
                var res2 = await webDriver.GoToUrl("https://www.google.com/");

            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }
        }

        private static void SaveScreenshot(string base64String)
        {
            if (!string.IsNullOrWhiteSpace(base64String))
            {
                string path = GetFilePathToSaveScreenshot();
                File.WriteAllBytes(path, Convert.FromBase64String(base64String));
            }
        }

        private static string GetFilePathToSaveScreenshot()
        {
            var dir = ""; // @"C:\temp";
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var i = 0;
            var path = "";
            do
            {
                i++;
                path = Path.Combine(dir, $"screenshot{i}.png");
            } while (File.Exists(path));
            return path;
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (webDriver == null)
            {
                asyncChromeDriver = new AsyncChromeDriver();
                webDriver = new WebDriver(asyncChromeDriver);
            }
            try
            {
                var res2 = await webDriver.GoToUrl("https://www.google.com/");
                var screenshot = await asyncChromeDriver.DevTools.Page.CaptureScreenshot();
                SaveScreenshot(screenshot.Data);

            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Must be synchronous in Window_Closing. Any await will close immediately
            asyncChromeDriverDev?.CloseSync();
            if (asyncChromeDriver != null) asyncChromeDriver.CloseSync();
            if (webDriver != null) webDriver.CloseSync();

        }

        private async void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (webDriver == null)
            {
                asyncChromeDriver = new AsyncChromeDriver();
                webDriver = new WebDriver(asyncChromeDriver);
            }
            try
            {
                var res2 = await webDriver.GoToUrl("https://www.google.com/");
                var screenshot = await webDriver.GetScreenshot();
                string path = GetFilePathToSaveScreenshot();
                using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
                {
                    System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
                    screenshotImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }
                //screenshot.SaveAsFile(path, Zu.WebBrowser.BasicTypes.ScreenshotImageFormat.Png);
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }
        }

        private async void Button_Click_9(object sender, RoutedEventArgs e)
        {
            var userDir = tbOpenProfileDir.Text;
            try
            {
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var width = 1200;
                    var height = 900;
                    int.TryParse(tbOpenProfileHeadlessWidth.Text, out width);
                    int.TryParse(tbOpenProfileHeadlessHeight.Text, out height);
                    asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless().SetWindowSize(width, height).SetUserDir(userDir));
                }
                else asyncChromeDriver = new AsyncChromeDriver(userDir);
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect(); // browser opens here
                // await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                var mess = $"opened on port {asyncChromeDriver.Port} in dir {asyncChromeDriver.UserDir} \nWhen close, dir will NOT be deleted";
                tbDevToolsRes.Text = mess;
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
                tbDevToolsRes2.Text = ex.ToString();
            }

        }

        private async void Button_Click_10(object sender, RoutedEventArgs e)
        {
            var userDir = tbOpenProfileDir.Text;
            if (int.TryParse(tbOpenProfilePort.Text, out int port))
            {
                try
                {
                    if (chbOpenProfileHeadless.IsChecked == true)
                    {
                        var width = 1200;
                        var height = 900;
                        int.TryParse(tbOpenProfileHeadlessWidth.Text, out width);
                        int.TryParse(tbOpenProfileHeadlessHeight.Text, out height);
                        asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless().SetWindowSize(width, height).SetUserDir(userDir).SetPort(port));
                    }
                    else asyncChromeDriver = new AsyncChromeDriver(userDir, port);
                    webDriver = new WebDriver(asyncChromeDriver);
                    // await asyncChromeDriver.Connect(); // browser opens here
                    await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                    var mess = $"opened on port {asyncChromeDriver.Port} in dir {asyncChromeDriver.UserDir} \nWhen close, dir will NOT be deleted";
                    tbDevToolsRes.Text = mess;
                    tbDevToolsRes2.Text = mess;
                }
                catch (Exception ex)
                {
                    tbDevToolsRes.Text = ex.ToString();
                    tbDevToolsRes2.Text = ex.ToString();
                }
            }
        }

        private async void Button_Click_11(object sender, RoutedEventArgs e)
        {
            var userDir = tbOpenProfileDir.Text;
            try
            {
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var width = 1200;
                    var height = 900;
                    int.TryParse(tbOpenProfileHeadlessWidth.Text, out width);
                    int.TryParse(tbOpenProfileHeadlessHeight.Text, out height);
                    asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless().SetWindowSize(width, height).SetIsTempProfile());
                }
                else asyncChromeDriver = new AsyncChromeDriver();
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect(); // browser opens here
                                                   // await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                var mess = $"opened on port {asyncChromeDriver.Port} in dir {asyncChromeDriver.UserDir} \nWhen close, dir will be DELETED";
                tbDevToolsRes.Text = mess;
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click_12(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chbOpenProfileHeadless.IsChecked == true)
                {
                    var width = 1200;
                    var height = 900;
                    int.TryParse(tbOpenProfileHeadlessWidth.Text, out width);
                    int.TryParse(tbOpenProfileHeadlessHeight.Text, out height);
                    asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless().SetWindowSize(width, height));
                }
                else asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig());
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect(); // browser opens here
                                                   // await webDriver.GoToUrl("https://www.google.com/"); // browser opens here
                var mess = $"opened on port {asyncChromeDriver.Port} in dir {asyncChromeDriver.UserDir} \nWhen close, dir will NOT be deleted";
                tbDevToolsRes.Text = mess;
                tbDevToolsRes2.Text = mess;
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click_13(object sender, RoutedEventArgs e)
        {
            try
            {
                var asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless());
                var webDriver = new WebDriver(asyncChromeDriver);
                await webDriver.GoToUrl("https://www.google.com/");
                await Task.Delay(500);
                var screenshot = await webDriver.GetScreenshot();
                //screenshot.SaveAsFile(GetFilePathToSaveScreenshot(), Zu.WebBrowser.BasicTypes.ScreenshotImageFormat.Png);
                using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
                {
                    System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
                    screenshotImage.Save(GetFilePathToSaveScreenshot(), System.Drawing.Imaging.ImageFormat.Png);
                }
                await webDriver.Close();
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
                tbDevToolsRes2.Text = ex.ToString();
            }
        }

        private async void Button_Click_14(object sender, RoutedEventArgs e)
        {
            try
            {
                asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetDoOpenWSProxy().SetPort(14392));
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect();
                tbDevToolsRes.Text = "opened";
                tbDevToolsRes2.Text = $"opened on port {asyncChromeDriver.Port} in dir {asyncChromeDriver.UserDir} \nWhen close, dir will be DELETED";
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
                tbDevToolsRes2.Text = ex.ToString();
            }

        }

        private async void Button_Click_15(object sender, RoutedEventArgs e)
        {
            asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetDoOpenWSProxy());
            webDriver = new WebDriver(asyncChromeDriver);
            await webDriver.Open();
            //chromeProcess = await asyncChromeDriver.OpenChromeProfile(asyncChromeDriver.Config);
            //asyncChromeDriver.DevTools = new Zu.Chrome.BrowserDevTools.ChromeDevToolsConnectionProxy(14392, wsProxyConfig: asyncChromeDriver.Config.WSProxyConfig);
            //asyncChromeDriver.isConnected = true;
            //await asyncChromeDriver.DevTools.Connect();
            asyncChromeDriverDev = new AsyncChromeDriver();
            await asyncChromeDriverDev.Connect();
            await asyncChromeDriverDev.Navigation.GoToUrl(asyncChromeDriver.GetBrowserDevToolsUrl());
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            asyncChromeDriverDev?.CloseSync();
            asyncChromeDriver?.CloseSync();
        }

        private async void Button_Click_17(object sender, RoutedEventArgs e)
        {
            try
            {
                asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetDoOpenBrowserDevTools());
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect();
                tbDevToolsRes.Text = "opened";
                tbDevToolsRes2.Text = $"opened on port {asyncChromeDriver.Port} in dir {asyncChromeDriver.UserDir} \nWhen close, dir will be DELETED";
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
                tbDevToolsRes2.Text = ex.ToString();
            }

        }

        private async void Button_Click_18(object sender, RoutedEventArgs e)
        {
            if (asyncChromeDriver?.BrowserDevTools == null) return;
            var devToolsWebDriver = new WebDriver(asyncChromeDriver.BrowserDevTools);
            var tabNetwork = await devToolsWebDriver.FindElementByIdOrDefault("tab-network");
            var page = await devToolsWebDriver.PageSource();
            if (tabNetwork != null) await tabNetwork.Click();
        }

        private async void Button_Click_19(object sender, RoutedEventArgs e)
        {
            try
            {
                asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetDoOpenBrowserDevTools());
                asyncChromeDriver.BrowserDevToolsConfig = new ChromeDriverConfig().SetDoOpenBrowserDevTools();
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect();
                tbDevToolsRes.Text = "opened";
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }
        }

        private async void Button_Click_20(object sender, RoutedEventArgs e)
        {
            if (webDriver == null) return;
            await webDriver.GoToUrl("https://www.google.com/");
        }

        private async void Button_Click_21(object sender, RoutedEventArgs e)
        {
            try
            {
                asyncChromeDriver = new AsyncChromeDriver(
                    new ChromeDriverConfig()
                    .SetDoOpenBrowserDevTools()
                    .SetWSProxyConfig(new ChromeWSProxyConfig { HttpServerSaveRequestedFiles = true })
                    );
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect();
                tbDevToolsFilesDir.Text = Path.Combine( asyncChromeDriver.Config.WSProxyConfig.DevToolsFilesDir, "devtools");
                tbDevToolsRes.Text = "opened";
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }

        }

        private async void Button_Click_22(object sender, RoutedEventArgs e)
        {
            var dir = tbDevToolsFilesDir.Text;
            if (string.IsNullOrWhiteSpace(dir)) dir = Directory.GetCurrentDirectory();
            dir = dir.TrimEnd('\\');
            if (dir.EndsWith("\\devtools")) dir = Path.GetDirectoryName(dir);
            try
            {
                asyncChromeDriver = new AsyncChromeDriver(
                    new ChromeDriverConfig()
                    .SetDoOpenBrowserDevTools()
                    .SetWSProxyConfig(new ChromeWSProxyConfig { DevToolsFilesDir = dir, HttpServerTryFindRequestedFileLocaly = true })
                    );
                webDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect();
                tbDevToolsRes.Text = "opened";
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }
        }
    }
}
