using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class CssValueTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldPickUpStyleOfAnElement()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("green-parent"));
            string backgroundColour = await element.GetCssValue("background-color");

            Assert.That(backgroundColour, Is.EqualTo("#008000").Or.EqualTo("rgba(0, 128, 0, 1)").Or.EqualTo("rgb(0, 128, 0)"));

            element = await driver.FindElement(By.Id("red-item"));
            backgroundColour = await element.GetCssValue("background-color");

            Assert.That(backgroundColour, Is.EqualTo("#ff0000").Or.EqualTo("rgba(255, 0, 0, 1)").Or.EqualTo("rgb(255, 0, 0)"));
        }

        [Test]
        public async Task GetCssValueShouldReturnStandardizedColour()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("colorPage.html"));

            IWebElement element = await driver.FindElement(By.Id("namedColor"));
            string backgroundColour = await element.GetCssValue("background-color");
            Assert.That(backgroundColour, Is.EqualTo("rgba(0, 128, 0, 1)").Or.EqualTo("rgb(0, 128, 0)"));

            element = await driver.FindElement(By.Id("rgb"));
            backgroundColour = await element.GetCssValue("background-color");
            Assert.That(backgroundColour, Is.EqualTo("rgba(0, 128, 0, 1)").Or.EqualTo("rgb(0, 128, 0)"));
        }

        [Test]
        public async Task ShouldAllowInheritedStylesToBeUsed()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("green-item"));
            string backgroundColour = await element.GetCssValue("background-color");

            Assert.That(backgroundColour, Is.EqualTo("transparent").Or.EqualTo("rgba(0, 0, 0, 0)"));
        }
    }
}
