using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ElementEqualityTest : DriverTestFixture
    {
        [Test]
        public async Task SameElementLookedUpDifferentWaysShouldBeEqual()
        {
            await driver.GoToUrl(simpleTestPage);

            IWebElement body = await driver.FindElement(By.TagName("body"));
            IWebElement xbody = await driver.FindElement(By.XPath("//body"));

            Assert.AreEqual(body, xbody);
        }

        [Test]
        public async Task DifferentElementsShouldNotBeEqual()
        {
            await driver.GoToUrl(simpleTestPage);

            ReadOnlyCollection<IWebElement> ps = await driver.FindElements(By.TagName("p"));

            Assert.AreNotEqual(ps[0], ps[1]);
        }

        [Test]
        public async Task SameElementLookedUpDifferentWaysUsingFindElementShouldHaveSameHashCode()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement body = await driver.FindElement(By.TagName("body"));
            IWebElement xbody = await driver.FindElement(By.XPath("//body"));

            Assert.AreEqual(body.GetHashCode(), xbody.GetHashCode());
        }

        public async Task SameElementLookedUpDifferentWaysUsingFindElementsShouldHaveSameHashCode()
        {
            await driver.GoToUrl(simpleTestPage);
            ReadOnlyCollection<IWebElement> body = await driver.FindElements(By.TagName("body"));
            ReadOnlyCollection<IWebElement> xbody = await driver.FindElements(By.XPath("//body"));

            Assert.AreEqual(body[0].GetHashCode(), xbody[0].GetHashCode());
        }

        [Test]
        public async Task AnElementFoundInADifferentFrameViaJsShouldHaveSameId()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("missedJsReference.html"));

            await driver.SwitchTo().Frame("inner");
            IWebElement first = await driver.FindElement(By.Id("oneline"));

            await driver.SwitchTo().DefaultContent();
            IWebElement element = (IWebElement)await ((IJavaScriptExecutor)driver).ExecuteScript("return frames[0].document.getElementById('oneline');");

            await driver.SwitchTo().Frame("inner");

            IWebElement second = await driver.FindElement(By.Id("oneline"));
            Assert.AreEqual(first, element);
            Assert.AreEqual(second, element);
        }
    }
}
