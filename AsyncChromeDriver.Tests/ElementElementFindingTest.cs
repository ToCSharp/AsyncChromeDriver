using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;

namespace OpenQA.Selenium
{
    // TODO(andre.nogueira): Find better name. This class is to distinguish 
    // finding elements in the driver (whole page), and inside other elements
    [TestFixture]
    public class ElementElementFindingTest : DriverTestFixture
    {
        #region FindElemement Tests

        [Test]
        public async Task ShouldFindElementById()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Id("test_id_div"));
            IWebElement child = await parent.FindElement(By.Id("test_id"));
            Assert.That(await child.Text(), Is.EqualTo("inside"));
        }

        [Test]
        public async Task ShouldFindElementByLinkText()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("div1"));
            IWebElement child = await parent.FindElement(By.PartialLinkText("hello world"));
            Assert.That(await child.Text(), Is.EqualTo("hello world"));
        }

        [Test]
        public async Task ShouldFindElementByName()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("div1"));
            IWebElement child = await parent.FindElement(By.Name("link1"));
            Assert.That(await child.Text(), Is.EqualTo("hello world"));
        }

        [Test]
        public async Task ShouldFindElementByXPath()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Id("test_id_div"));
            IWebElement child = await parent.FindElement(By.XPath("p"));
            Assert.That(await child.Text(), Is.EqualTo("inside"));
        }

        [Test]
        public async Task ShouldFindElementByClassName()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("classes"));
            IWebElement child = await parent.FindElement(By.ClassName("oneother"));
            Assert.That(await child.Text(), Is.EqualTo("But not me"));
        }

        [Test]
        public async Task ShouldFindElementByPartialLinkText()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("div1"));
            IWebElement child = await parent.FindElement(By.PartialLinkText(" world"));
            Assert.That(await child.Text(), Is.EqualTo("hello world"));
        }

        [Test]
        public async Task ShouldFindElementByTagName()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Id("test_id_div"));
            IWebElement child = await parent.FindElement(By.TagName("p"));
            Assert.That(await child.Text(), Is.EqualTo("inside"));
        }
        #endregion

        #region FindElemements Tests

        [Test]
        public async Task ShouldFindElementsById()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("form2"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.Id("2"));
            Assert.That(children.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldFindElementsByLinkText()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("div1"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.PartialLinkText("hello world"));
            Assert.That(children.Count, Is.EqualTo(2));
            Assert.That(await children[0].Text(), Is.EqualTo("hello world"));
            Assert.That(await children[1].Text(), Is.EqualTo("hello world"));
        }

        [Test]
        public async Task ShouldFindElementsByName()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("form2"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.Name("selectomatic"));
            Assert.That(children.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldFindElementsByXPath()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("classes"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.XPath("span"));
            Assert.That(children.Count, Is.EqualTo(3));
            Assert.That(await children[0].Text(), Is.EqualTo("Find me"));
            Assert.That(await children[1].Text(), Is.EqualTo("Also me"));
            Assert.That(await children[2].Text(), Is.EqualTo("But not me"));
        }

        [Test]
        public async Task ShouldFindElementsByClassName()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("classes"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.ClassName("one"));
            Assert.That(children.Count, Is.EqualTo(2));
            Assert.That(await children[0].Text(), Is.EqualTo("Find me"));
            Assert.That(await children[1].Text(), Is.EqualTo("Also me"));
        }

        [Test]
        public async Task ShouldFindElementsByPartialLinkText()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("div1"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.PartialLinkText("hello "));
            Assert.That(children.Count, Is.EqualTo(2));
            Assert.That(await children[0].Text(), Is.EqualTo("hello world"));
            Assert.That(await children[1].Text(), Is.EqualTo("hello world"));
        }

        [Test]
        public async Task ShouldFindElementsByTagName()
        {
           await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("classes"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.TagName("span"));
            Assert.That(children.Count, Is.EqualTo(3));
            Assert.That(await children[0].Text(), Is.EqualTo("Find me"));
            Assert.That(await children[1].Text(), Is.EqualTo("Also me"));
            Assert.That(await children[2].Text(), Is.EqualTo("But not me"));
        }

        #endregion
    }
}
