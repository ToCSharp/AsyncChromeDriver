using System;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;

namespace OpenQA.Selenium
{
    public class TestUtilities
    {
        private static IJavaScriptExecutor GetExecutor(IWebDriver driver)
        {
            return driver as IJavaScriptExecutor;
        }

        private static async Task<string> GetUserAgent(IWebDriver driver)
        {
            try
            {
                return (string)await GetExecutor(driver).ExecuteScript("return navigator.userAgent;");
            }
            catch (Exception)
            {
                // Some drivers will only execute JS once a page has been loaded. Since those
                // drivers aren't Firefox or IE, we don't worry about that here.
                //
                // Non-javascript-enabled HtmlUnit throws an UnsupportedOperationException here.
                // Let's just ignore that.
                return "";
            }
        }



        //public static bool IsNativeEventsEnabled(IWebDriver driver)
        //{
        //    IHasCapabilities hasCaps = driver as IHasCapabilities;
        //    if (hasCaps != null)
        //    {
        //        object cap = hasCaps.Capabilities.GetCapability(OpenQA.Selenium.Remote.CapabilityType.HasNativeEvents);
        //        if (cap != null && cap is bool)
        //        {
        //            return (bool)cap;
        //        }
        //    }

        //    return false;
        //}
    }
}
