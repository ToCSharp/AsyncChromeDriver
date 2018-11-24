using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ContentEditableTest : DriverTestFixture
    {
        [TearDown]
        public async Task SwitchToDefaultContent()
        {
            await driver.SwitchTo().DefaultContent();
        }

        [Test]
        public async Task TypingIntoAnIFrameWithContentEditableOrDesignModeSet()
        {
           await driver.GoToUrl(richTextPage);

            await driver.SwitchTo().Frame("editFrame");
            IWebElement element = await driver.SwitchTo().ActiveElement();
            await element.SendKeys("Fishy");

            await driver.SwitchTo().DefaultContent();
            IWebElement trusted = await driver.FindElement(By.Id("istrusted"));
            IWebElement id = await driver.FindElement(By.Id("tagId"));

            // Chrome does not set a trusted flag.
            Assert.That(await trusted.Text(), Is.AnyOf("[true]", "[n/a]", "[]"));
            Assert.That(await id.Text(), Is.AnyOf("[frameHtml]", "[theBody]"));
        }

        [Test]
        public async Task NonPrintableCharactersShouldWorkWithContentEditableOrDesignModeSet()
        {
           await driver.GoToUrl(richTextPage);

            await driver.SwitchTo().Frame("editFrame");
            IWebElement element = await driver.SwitchTo().ActiveElement();
            await element.SendKeys("Dishy" + Keys.Backspace + Keys.Left + Keys.Left);
            await element.SendKeys(Keys.Left + Keys.Left + "F" + Keys.Delete + Keys.End + "ee!");

            Assert.AreEqual("Fishee!", element.Text());
        }

        [Test]
        public async Task ShouldBeAbleToTypeIntoEmptyContentEditableElement()
        {
           await driver.GoToUrl(readOnlyPage);
            IWebElement editable = await driver.FindElement(By.Id("content-editable-blank"));

            await editable.SendKeys("cheese");

            Assert.That(await editable.Text(), Is.EqualTo("cheese"));
        }


        [Test]
        public async Task ShouldBeAbleToTypeIntoTinyMCE()
        {
           await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("tinymce.html"));
            await driver.SwitchTo().Frame("mce_0_ifr");

            IWebElement editable = await driver.FindElement(By.Id("tinymce"));

            await editable.Clear();
            await editable.SendKeys("cheese"); // requires focus on OS X

            Assert.That(await editable.Text(), Is.EqualTo("cheese"));
        }

    }
}
