using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class I18Test : DriverTestFixture
    {
        // The Hebrew word shalom (peace) encoded in order Shin (sh) Lamed (L) Vav (O) final-Mem (M).
        private string shalom = "\u05E9\u05DC\u05D5\u05DD";

        // The Hebrew word tmunot (images) encoded in order Taf (t) Mem (m) Vav (u) Nun (n) Vav (o) Taf (t).
        private string tmunot = "\u05EA\u05DE\u05D5\u05E0\u05D5\u05EA";

        // This is the Chinese link text
        private string linkText = "\u4E2D\u56FD\u4E4B\u58F0";

        [Test]
        public async Task ShouldBeAbleToReadChinese()
        {
            await driver.GoToUrl(chinesePage);
            await driver.FindElement(By.LinkText(linkText)).Click();
        }

        [Test]
        public async Task ShouldBeAbleToEnterHebrewTextFromLeftToRight()
        {
            await driver.GoToUrl(chinesePage);
            IWebElement input = await driver.FindElement(By.Name("i18n"));
            await input.SendKeys(shalom);

            Assert.AreEqual(shalom, await input.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToEnterHebrewTextFromRightToLeft()
        {
            await driver.GoToUrl(chinesePage);
            IWebElement input = await driver.FindElement(By.Name("i18n"));
            await input.SendKeys(tmunot);

            Assert.AreEqual(tmunot, await input.GetAttribute("value"));
        }


        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldBeAbleToReturnTheTextInAPage()
        {
            string url = EnvironmentManager.Instance.UrlBuilder.WhereIs("encoding");
            await driver.GoToUrl(url);

            string text = await driver.FindElement(By.TagName("body")).Text();

            Assert.AreEqual(shalom, text);
        }
    }
}
