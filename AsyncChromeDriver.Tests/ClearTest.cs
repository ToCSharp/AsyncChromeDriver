using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class ClearTest : DriverTestFixture
    {
        [Test]
        public async Task WritableTextInputShouldClear()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement element = await driver.FindElement(By.Id("writableTextInput")).Clear();
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
        }

        [Test]
        public async Task TextInputShouldNotClearWhenDisabled()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement element = await driver.FindElement(By.Id("textInputnotenabled"));
            Assert.That(await element.Enabled(), Is.False);
            //Assert.That(async () => await element.Clear(), Throws.InstanceOf<InvalidElementStateException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Clear(),
                exception => Assert.AreEqual("InvalidElementState", exception.Error));
        }

        [Test]
        public async Task TextInputShouldNotClearWhenReadOnly()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement element = await driver.FindElement(By.Id("readOnlyTextInput"));
            //Assert.That(async () => await element.Clear(), Throws.InstanceOf<InvalidElementStateException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Clear(),
                exception => Assert.AreEqual("InvalidElementState", exception.Error));
        }

        [Test]
        public async Task WritableTextAreaShouldClear()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement element = await driver.FindElement(By.Id("writableTextArea")).Clear();
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
        }

        [Test]
        public async Task TextAreaShouldNotClearWhenDisabled()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement element = await driver.FindElement(By.Id("textAreaNotenabled"));
            //Assert.That(async () => await element.Clear(), Throws.InstanceOf<InvalidElementStateException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Clear(),
                exception => Assert.AreEqual("InvalidElementState", exception.Error));
        }

        [Test]
        public async Task TextAreaShouldNotClearWhenReadOnly()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement element = await driver.FindElement(By.Id("textAreaReadOnly"));
            //Assert.That(async () => await element.Clear(), Throws.InstanceOf<InvalidElementStateException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Clear(),
                exception => Assert.AreEqual("InvalidElementState", exception.Error));
        }

        [Test]
        public async Task ContentEditableAreaShouldClear()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement element = await driver.FindElement(By.Id("content-editable")).Clear();
            Assert.AreEqual(string.Empty, await element.Text());
        }

        [Test]
        public async Task ShouldBeAbleToClearNoTypeInput()
        {
            await ShouldBeAbleToClearInput(By.Name("no_type"), "input with no type");
        }

        [Test]
        public async Task ShouldBeAbleToClearNumberInput()
        {
            await ShouldBeAbleToClearInput(By.Name("number_input"), "42");
        }

        [Test]
        public async Task ShouldBeAbleToClearEmailInput()
        {
            await ShouldBeAbleToClearInput(By.Name("email_input"), "admin@localhost");
        }

        [Test]
        public async Task ShouldBeAbleToClearPasswordInput()
        {
            await ShouldBeAbleToClearInput(By.Name("password_input"), "qwerty");
        }

        [Test]
        public async Task ShouldBeAbleToClearSearchInput()
        {
            await ShouldBeAbleToClearInput(By.Name("search_input"), "search");
        }

        [Test]
        public async Task ShouldBeAbleToClearTelInput()
        {
            await ShouldBeAbleToClearInput(By.Name("tel_input"), "911");
        }

        [Test]
        public async Task ShouldBeAbleToClearTextInput()
        {
            await ShouldBeAbleToClearInput(By.Name("text_input"), "text input");
        }

        [Test]
        public async Task ShouldBeAbleToClearUrlInput()
        {
            await ShouldBeAbleToClearInput(By.Name("url_input"), "http://seleniumhq.org/");
        }


        [Test]
        public async Task ShouldBeAbleToClearDatetimeInput()
        {
            await ShouldBeAbleToClearInput(By.Name("datetime_input"), "2017-11-22T11:22");
        }


        private async Task ShouldBeAbleToClearInput(By locator, string oldValue)
        {
            await ShouldBeAbleToClearInput(locator, oldValue, string.Empty);
        }

        private async Task ShouldBeAbleToClearInput(By locator, string oldValue, string clearedValue)
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("inputs.html"));
            IWebElement element = await driver.FindElement(locator);
            Assert.AreEqual(oldValue, await element.GetAttribute("value"));
            await element.Clear();
            Assert.AreEqual(clearedValue, await element.GetAttribute("value"));
        }
    }
}
