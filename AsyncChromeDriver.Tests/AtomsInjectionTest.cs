using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class AtomsInjectionTest : DriverTestFixture
    {
        [Test]
        public async Task InjectingAtomShouldNotTrampleOnUnderscoreGlobal()
        {
            await driver.GoToUrl(underscorePage);
            await driver.FindElement(By.TagName("body"));
            Assert.AreEqual("123", await ((IJavaScriptExecutor)driver).ExecuteScript("return _.join('');"));
        }
    }
}
