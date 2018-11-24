using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class DriverElementFindingTest : DriverTestFixture
    {

        #region FindElemement Tests

        [Test]
        public async Task ShouldFindElementById()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement e = await driver.FindElement(By.Id("oneline"));
            Assert.That(await e.Text(), Is.EqualTo("A single line of text"));
        }

        [Test]
        public async Task ShouldFindElementByLinkText()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement e = await driver.FindElement(By.LinkText("link with leading space"));
            Assert.That(await e.Text(), Is.EqualTo("link with leading space"));
        }

        [Test]
        public async Task ShouldFindElementByName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement e = await driver.FindElement(By.Name("div1"));
            Assert.That(await e.Text(), Is.EqualTo("hello world hello world"));
        }

        [Test]
        public async Task ShouldFindElementByXPath()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement e = await driver.FindElement(By.XPath("/html/body/p[1]"));
            Assert.That(await e.Text(), Is.EqualTo("A single line of text"));
        }

        [Test]
        public async Task ShouldFindElementByClassName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement e = await driver.FindElement(By.ClassName("one"));
            Assert.That(await e.Text(), Is.EqualTo("Span with class of one"));
        }

        [Test]
        public async Task ShouldFindElementByPartialLinkText()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement e = await driver.FindElement(By.PartialLinkText("leading space"));
            Assert.That(await e.Text(), Is.EqualTo("link with leading space"));
        }

        [Test]
        public async Task ShouldFindElementByTagName()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement e = await driver.FindElement(By.TagName("H1"));
            Assert.That(await e.Text(), Is.EqualTo("Heading"));
        }
        #endregion

        //TODO(andre.nogueira): We're not checking the right elements are being returned!
        #region FindElemements Tests

        [Test]
        public async Task ShouldFindElementsById()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Id("test_id"));
            Assert.That(elements.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldFindElementsByLinkText()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.LinkText("hello world"));
            Assert.That(elements.Count, Is.EqualTo(12));
        }

        [Test]
        public async Task ShouldFindElementsByName()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Name("form1"));
            Assert.That(elements.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task ShouldFindElementsByXPath()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.XPath("//a"));
            Assert.That(elements.Count, Is.EqualTo(12));
        }

        [Test]
        public async Task ShouldFindElementsByClassName()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.ClassName("one"));
            Assert.That(elements.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task ShouldFindElementsByPartialLinkText()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.PartialLinkText("world"));
            Assert.That(elements.Count, Is.EqualTo(12));
        }

        [Test]
        public async Task ShouldFindElementsByTagName()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.TagName("a"));
            Assert.That(elements.Count, Is.EqualTo(12));
        }
        #endregion
    }
}
