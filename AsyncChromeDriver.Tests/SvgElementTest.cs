using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class SvgElementTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldClickOnGraphVisualElements()
        {

            await driver.GoToUrl(svgPage);
            IWebElement svg = await driver.FindElement(By.CssSelector("svg"));

            ReadOnlyCollection<IWebElement> groupElements = await svg.FindElements(By.CssSelector("g"));
            Assert.AreEqual(5, groupElements.Count);
            await groupElements[1].Click();
            IWebElement resultElement = await driver.FindElement(By.Id("result"));
            await WaitFor(resultElement.Text(), "slice_red", "Element text was not 'slice_red'");
            Assert.AreEqual("slice_red", await resultElement.Text());
            await groupElements[2].Click();
            resultElement = await driver.FindElement(By.Id("result"));
            await WaitFor(async () => await resultElement.Text() == "slice_green", "Element text was not 'slice_green'");
            Assert.AreEqual("slice_green", await resultElement.Text());
        }

        [Test]
        public async Task ShouldClickOnGraphTextElements()
        {

            await driver.GoToUrl(svgPage);
            IWebElement svg = await driver.FindElement(By.CssSelector("svg"));
            ReadOnlyCollection<IWebElement> textElements = await svg.FindElements(By.CssSelector("text"));

            IWebElement appleElement = await FindAppleElement(textElements);
            Assert.That(appleElement, Is.Not.Null);
            await appleElement.Click();
            IWebElement resultElement = await driver.FindElement(By.Id("result"));
            await WaitFor(resultElement.Text(), "text_apple", "Element text was not 'text_apple'");
            Assert.AreEqual("text_apple", await resultElement.Text());
        }

        private async Task<IWebElement> FindAppleElement(IEnumerable<IWebElement> textElements)
        {
            foreach (IWebElement currentElement in textElements) {
                if ((await currentElement.Text()).Contains("Apple")) {
                    return currentElement;
                }
            }

            return null;
        }
    }
}
