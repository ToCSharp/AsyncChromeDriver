using NUnit.Framework;
using OpenQA.Selenium.Environment;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class MiscTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldReturnTitleOfPageIfSet()
        {
            await driver.GoToUrl(xhtmlTestPage);
            Assert.That(await driver.Title(), Is.EqualTo("XHTML Test Page"));

            await driver.GoToUrl(simpleTestPage);
            Assert.That(await driver.Title(), Is.EqualTo("Hello WebDriver"));
        }

        [Test]
        public async Task ShouldReportTheCurrentUrlCorrectly()
        {
            await driver.GoToUrl(macbethPage);
            Assert.AreEqual(macbethPage, await driver.GetUrl());

            await driver.GoToUrl(simpleTestPage);
            Assert.AreEqual(simpleTestPage, await driver.GetUrl());

            await driver.GoToUrl(javascriptPage);
            Assert.AreEqual(javascriptPage, await driver.GetUrl());
        }

        [Test]
        public async Task ShouldReturnTagName()
        {
            await driver.GoToUrl(formsPage);
            IWebElement selectBox = await driver.FindElement(By.Id("cheese"));
            Assert.That((await selectBox.TagName()).ToLower(), Is.EqualTo("input"));
        }

        [Test]
        public async Task ShouldReturnTheSourceOfAPage()
        {
            string pageSource;
            await driver.GoToUrl(simpleTestPage);
            pageSource = await driver.PageSource().ToLower();

            Assert.That(pageSource, Does.StartWith("<html"));
            Assert.That(pageSource, Does.EndWith("</html>"));
            Assert.That(pageSource, Does.Contain("an inline element"));
            Assert.That(pageSource, Does.Contain("<p id="));
            Assert.That(pageSource, Does.Contain("lotsofspaces"));
            Assert.That(pageSource, Does.Contain("with document.write and with document.write again"));
        }

        [Test]
        public async Task ClickingShouldNotTrampleWOrHInGlobalScope()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("globalscope.html"));
            List<string> values = new List<string>() { "w", "h" };

            foreach (string val in values) {
                Assert.AreEqual(val, await GetGlobalVar(driver, val));
            }

            await driver.FindElement(By.Id("toclick")).Click();

            foreach (string val in values) {
                Assert.AreEqual(val, await GetGlobalVar(driver, val));
            }
        }

        private async Task<string> GetGlobalVar(IWebDriver driver, string value)
        {
            object val = await ((IJavaScriptExecutor)driver).ExecuteScript("return window." + value + ";");
            return val == null ? "null" : val.ToString();
        }
    }
}
