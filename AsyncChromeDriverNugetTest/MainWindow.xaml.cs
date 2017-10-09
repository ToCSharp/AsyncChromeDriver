using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Zu.AsyncWebDriver.Remote;
using Zu.Chrome;

namespace AsyncChromeDriverNugetTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebDriver webDriver;
        private AsyncChromeDriver asyncChromeDriver;
        private List<AsyncChromeDriver> driversToClose = new List<AsyncChromeDriver>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
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
                //screenshot.SaveAsFile(path, Zu.WebBrowser.BasicTypes.ScreenshotImageFormat.Png);
                using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
                {
                    System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
                    screenshotImage.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                tbRes.Text = ex.ToString();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (asyncChromeDriver == null)
            {
                asyncChromeDriver = new AsyncChromeDriver();
            }
            try
            {
                await asyncChromeDriver.CheckConnected();
                await asyncChromeDriver.DevTools.Session.Page.Enable();
                asyncChromeDriver.DevTools.Session.Page.SubscribeToLoadEventFiredEvent(async (e2) => 
                {
                    var screenshot = await asyncChromeDriver.DevTools.Session.Page.CaptureScreenshot();
                    var filePath = SaveScreenshot(screenshot.Data);
                    await Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        tbRes.Text = "Screenshot saved: " + filePath;
                    });
                });
                //await asyncChromeDriver.GoToUrl("https://www.google.com/");
                await asyncChromeDriver.DevTools.Session.Page.Navigate(new BaristaLabs.ChromeDevTools.Runtime.Page.NavigateCommand
                {
                    Url = "https://www.google.com/"
                });

            }
            catch (Exception ex)
            {
                tbRes.Text = ex.ToString();
            }

        }

        private static string SaveScreenshot(string base64String)
        {
            if (!string.IsNullOrWhiteSpace(base64String))
            {
                string path = GetFilePathToSaveScreenshot();
                File.WriteAllBytes(path, Convert.FromBase64String(base64String));
                return path;
            }
            return null;
        }

        private static string GetFilePathToSaveScreenshot()
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
            return path;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CloseChrome();
        }

        private void CloseChrome()
        {
            if (asyncChromeDriver != null)
            {
                try
                {
                    asyncChromeDriver.CloseSync();
                }
                catch { }
            }
        }

        private  void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseChrome();
            foreach (var item in driversToClose)
            {
                try
                {
                    item?.CloseSync();
                }
                catch { }
            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var drv1 = new AsyncChromeDriver();
            var drv2 = new AsyncChromeDriver();
            var drv3 = new AsyncChromeDriver();
            driversToClose.Add(drv1);
            driversToClose.Add(drv2);
            driversToClose.Add(drv3);
            await drv1.Connect();
            await drv2.Connect();
            await drv3.Connect();
            await drv2.Navigation.GoToUrl("http://www.google.co.uk");

        }
    }
}
