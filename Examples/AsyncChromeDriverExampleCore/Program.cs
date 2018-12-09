using System;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Remote;
using Zu.Chrome;

namespace AsyncChromeDriverExampleCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello!");
            var asyncChromeDriver = new AsyncChromeDriver();
            var webDriver = new WebDriver(asyncChromeDriver);
            await webDriver.Open();
            Console.WriteLine("Must be opened.");
            await Task.Delay(2000);
            Console.WriteLine("Goto google");
            await webDriver.GoToUrl("https://www.google.com/");
            Console.WriteLine("Must be on google page.");
            Console.WriteLine("Press any key to close webDriver and app...");
            Console.ReadKey();
            await webDriver.Close();
        }
    }
}
