using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ObjectStateAssumptionsTest : DriverTestFixture
    {
        [Test]
        public async Task UninitializedWebDriverDoesNotThrowException()
        {
            await variousMethodCallsToCheckAssumptions();
        }

        /**
        * This test case differs from @see testUninitializedWebDriverDoesNotThrowNPE as it initializes
        * WebDriver with an initial call to get(). It also should not fail.
        */
        [Test]
        public async Task InitializedWebDriverDoesNotThrowException()
        {
            await driver.GoToUrl(simpleTestPage);
            await variousMethodCallsToCheckAssumptions();
        }

        /**
        * Test the various options, again for an uninitialized driver, NPEs are thrown.
        */
        [Test]
        public async Task OptionsForUninitializedWebDriverDoesNotThrowException()
        {
            IOptions options = driver.Options();
            ReadOnlyCollection<Zu.WebBrowser.BasicTypes.Cookie> allCookies = await options.Cookies.AllCookies();
        }

        /**
        * Add the various method calls you want to try here...
        */
        private async Task variousMethodCallsToCheckAssumptions()
        {
            string currentUrl = await driver.GetUrl();
            string currentTitle = await driver.Title();
            string pageSource = await driver.PageSource();
            By byHtml = By.XPath("//html");
            await driver.FindElement(byHtml);
            await driver.FindElements(byHtml);
        }
    }
}
