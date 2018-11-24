using System.Threading.Tasks;
using NUnit.Framework;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ErrorsTest : DriverTestFixture
    {
        /// <summary>
        /// Regression test for Selenium RC issue 363.
        /// http://code.google.com/p/selenium/issues/detail?id=363
        /// This will trivially pass on browsers that do not support the onerror
        /// handler (e.g. Internet Explorer).
        /// </summary>
        [Test]
        public async Task ShouldNotGenerateErrorsWhenOpeningANewPage()
        {
            await driver.GoToUrl(errorsPage);
            object result = await ((IJavaScriptExecutor)driver).ExecuteScript("return window.ERRORS.join('\\n');");
            Assert.AreEqual("", result, "Should have no errors");
        }

    }
}
