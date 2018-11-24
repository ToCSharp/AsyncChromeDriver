using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Internal;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class WebElementTest : DriverTestFixture
    {
        [Test]
        public async Task ElementShouldImplementWrapsDriver()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement parent = await driver.FindElement(By.Id("containsSomeDiv"));
            Assert.That(parent, Is.InstanceOf<IWrapsDriver>());
        }

        [Test]
        public async Task ElementShouldReturnOriginDriver()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement parent = await driver.FindElement(By.Id("containsSomeDiv"));
            Assert.That(((IWrapsDriver)parent).WrappedDriver, Is.EqualTo(driver));
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldToggleElementAndCheckIfElementIsSelected()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement checkbox = await driver.FindElement(By.Id("checkbox1"));
            Assert.That(await checkbox.Selected(), Is.False);
            await checkbox.Click();
            Assert.That(await checkbox.Selected(), Is.True);
            await checkbox.Click();
            Assert.That(await checkbox.Selected(), Is.False);
        }

        [Test]
        public async Task ShouldThrowExceptionOnNonExistingElement()
        {
            await driver.GoToUrl(simpleTestPage);
            //Assert.That(async () => await driver.FindElement(By.Id("doesnotexist")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("doesnotexist")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldGetElementName()
        {
            await driver.GoToUrl(simpleTestPage);

            IWebElement oneliner = await driver.FindElement(By.Id("oneline"));
            Assert.AreEqual("p", await oneliner.TagName().ToLower());

        }

        [Test]
        public async Task ShouldGetElementText()
        {
            await driver.GoToUrl(simpleTestPage);

            IWebElement oneliner = await driver.FindElement(By.Id("oneline"));
            Assert.AreEqual("A single line of text", await oneliner.Text());

            IWebElement twoblocks = await driver.FindElement(By.Id("twoblocks"));
            Assert.AreEqual("Some text" +
                System.Environment.NewLine +
                "Some more text", await twoblocks.Text());

        }

        [Test]
        public async Task ShouldReturnWhetherElementIsDisplayed()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement hidden = await driver.FindElement(By.Id("hidden"));
            Assert.That(await hidden.Displayed(), Is.False, "Element with ID 'hidden' should not be displayed");

            IWebElement none = await driver.FindElement(By.Id("none"));
            Assert.That(await none.Displayed(), Is.False, "Element with ID 'none' should not be displayed");

            IWebElement displayed = await driver.FindElement(By.Id("displayed"));
            Assert.That(await displayed.Displayed(), Is.True, "Element with ID 'displayed' should not be displayed");
        }

        [Test]
        public async Task ShouldClearElement()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement textbox = await driver.FindElement(By.Id("keyUp"));
            await textbox.SendKeys("a@#$ç.ó");
            await textbox.Clear();
            Assert.AreEqual("", await textbox.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldClearRenderedElement()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement textbox = await driver.FindElement(By.Id("keyUp"));
            await textbox.SendKeys("a@#$ç.ó");
            await textbox.Clear();
            Assert.AreEqual("", await textbox.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldSendKeysToElement()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement textbox = await driver.FindElement(By.Id("keyUp"));
            await textbox.SendKeys("a@#$ç.ó");
            Assert.AreEqual("a@#$ç.ó", await textbox.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldSubmitElement()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement submit = await driver.FindElement(By.Id("submittingButton"));
            await submit.Submit();

            Assert.That(await driver.GetUrl(), Does.StartWith(resultPage));
        }

        [Test]
        public async Task ShouldClickLinkElement()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement changedDiv = await driver.FindElement(By.Id("dynamo"));
            IWebElement link = await driver.FindElement(By.LinkText("Update a div"));
            await link.Click();
            Assert.AreEqual("Fish and chips!", await changedDiv.Text());
        }

        [Test]
        public async Task ShouldGetAttributesFromElement()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement dynamo = await driver.FindElement(By.Id("dynamo"));
            IWebElement mousedown = await driver.FindElement(By.Id("mousedown"));
            Assert.AreEqual("mousedown", await mousedown.GetAttribute("id"));
            Assert.AreEqual("dynamo", await dynamo.GetAttribute("id"));

        }
    }
}
