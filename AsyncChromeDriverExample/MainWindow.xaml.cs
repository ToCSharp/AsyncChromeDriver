using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;
using Zu.Chrome;

namespace AsyncChromeDriverExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AsyncChromeDriver asyncChromeDriver;
        private WebDriver asyncDriver;
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
                asyncDriver = new WebDriver(asyncChromeDriver);
                await asyncChromeDriver.Connect();
                tbDevToolsRes.Text = "opened";
            }
            catch (Exception ex)
            {
                tbDevToolsRes.Text = ex.ToString();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await asyncDriver?.Close();
            //await asyncChromeDriver?.Close();
            tbDevToolsRes.Text = "closed";
        }

        ObservableCollection<ResponseReceivedEventInfo> responseEvents = new ObservableCollection<ResponseReceivedEventInfo>();
        ObservableCollection<WebSocketFrameReceivedEventInfo> wsEvents = new ObservableCollection<WebSocketFrameReceivedEventInfo>();

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

        string CookieToString(BaristaLabs.ChromeDevTools.Runtime.Network.Cookie c)
        {
            var c2 = new Zu.WebBrowser.BasicTypes.Cookie(c.Name, c.Value, c.Domain, c.Path,
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(c.Expires).ToLocalTime()); //, DateTimeOffset.FromUnixTimeMilliseconds((long)c.Expires).UtcDateTime);
            return c2.ToString();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (asyncDriver == null)
            {
                asyncDriver = new WebDriver(new AsyncChromeDriver());
            }
            var mouse = asyncDriver.Mouse;
            try
            {
                var res2 = await asyncDriver.GoToUrl("https://www.google.com/");
                var query = await asyncDriver.FindElement(By.Name("q"));
                if (query == null)
                {
                    await Task.Delay(1000);
                    query = await asyncDriver.FindElement(By.Name("q"));
                }
                foreach (var v in "ToCSharp")
                {
                    await Task.Delay(500 + new Random().Next(1000));
                    await query.SendKeys(v.ToString());
                }
                await Task.Delay(500);
                await query.SendKeys(Keys.Enter);
                await Task.Delay(2000);
                query = await asyncDriver.FindElement(By.Name("q"));
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(1000);
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(2000);
                await query.SendKeys(Keys.ArrowDown);
                await Task.Delay(1000);
                await query.SendKeys(Keys.ArrowUp);
                await Task.Delay(500);
                await query.SendKeys(Keys.Enter);
                var el = await asyncDriver.SwitchTo().ActiveElement();
                await asyncDriver.Keyboard.SendKeys(Keys.PageDown);

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
            if (asyncDriver != null)
            {
                await asyncDriver.Keyboard.SendKeys(Keys.Up);
            }

        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (asyncDriver != null)
            {
                await asyncDriver.Keyboard.SendKeys(Keys.Down);
            }

        }
    }
}
