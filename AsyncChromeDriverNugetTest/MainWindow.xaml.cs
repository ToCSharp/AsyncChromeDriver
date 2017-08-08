using System;
using System.IO;
using System.Windows;
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
                screenshot.SaveAsFile(path, Zu.WebBrowser.BasicTypes.ScreenshotImageFormat.Png);
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
                await asyncChromeDriver.DevTools.Session.Page.Enable(new BaristaLabs.ChromeDevTools.Runtime.Page.EnableCommand());
                asyncChromeDriver.DevTools.Session.Page.SubscribeToDomContentEventFiredEvent(async (e2) =>
                {
                    var screenshot = await asyncChromeDriver.DevTools.Session.Page.CaptureScreenshot(new BaristaLabs.ChromeDevTools.Runtime.Page.CaptureScreenshotCommand());
                    SaveScreenshot(screenshot.Data);
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

        private async void CloseChrome()
        {
            if (asyncChromeDriver != null)
            {
                try
                {
                    await asyncChromeDriver.Close();
                }
                catch { }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseChrome();
        }
    }
}
