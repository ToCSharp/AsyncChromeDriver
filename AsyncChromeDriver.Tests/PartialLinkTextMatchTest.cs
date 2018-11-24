using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class PartialLinkTextMatchTest : DriverTestFixture
    {
        [Test]
        public async Task LinkWithFormattingTags()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement res = await elem.FindElement(By.PartialLinkText("link with formatting tags"));
            Assert.That(res, Is.Not.Null);
            Assert.AreEqual("link with formatting tags", await res.Text());
        }

        [Test]
        public async Task LinkWithLeadingSpaces()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement res = await elem.FindElement(By.PartialLinkText("link with leading space"));
            Assert.That(res, Is.Not.Null);
            Assert.AreEqual("link with leading space", await res.Text());
        }

        [Test]
        public async Task LinkWithTrailingSpace()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement res = await elem.FindElement(By.PartialLinkText("link with trailing space"));
            Assert.That(res, Is.Not.Null);
            Assert.AreEqual("link with trailing space", await res.Text());
        }

        [Test]
        public async Task FindMultipleElements()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            ReadOnlyCollection<IWebElement> elements = await elem.FindElements(By.PartialLinkText("link"));
            Assert.That(elements, Is.Not.Null);
            Assert.AreEqual(6, elements.Count);
        }

        [Test]
        public async Task DriverCanGetLinkByLinkTestIgnoringTrailingWhitespace()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement link = null;
            link = await driver.FindElement(By.LinkText("link with trailing space"));
            Assert.AreEqual("linkWithTrailingSpace", await link.GetAttribute("id"));
        }

        [Test]
        public async Task ElementCanGetLinkByLinkTestIgnoringTrailingWhitespace()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement link = null;
            link = await elem.FindElement(By.LinkText("link with trailing space"));
            Assert.AreEqual("linkWithTrailingSpace", await link.GetAttribute("id"));
        }
    }
}
