using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class ElementPropertyTest : DriverTestFixture
    {
        [Test]
        [IgnoreBrowser(Browser.Chrome, "Chrome does not support get element property command")]
        [IgnoreBrowser(Browser.Opera)]
        [IgnoreBrowser(Browser.Remote)]
        [IgnoreBrowser(Browser.Safari, "Safari does not support get element property command")]
        public async Task ShouldReturnNullWhenGettingTheValueOfAPropertyThatIsNotListed()
        {
           await driver.GoToUrl(simpleTestPage;
            IWebElement head = await driver.FindElement(By.XPath("/html"));
            string attribute = head.GetProperty("cheese");
            Assert.That(attribute, Is.Null);
        }

        [Test]
        [IgnoreBrowser(Browser.Chrome, "Chrome does not support get element property command")]
        [IgnoreBrowser(Browser.Opera)]
        [IgnoreBrowser(Browser.Remote)]
        [IgnoreBrowser(Browser.Safari, "Safari does not support get element property command")]
        public async Task CanRetrieveTheCurrentValueOfAProperty()
        {
           await driver.GoToUrl(formsPage;
            IWebElement element = await driver.FindElement(By.Id("working"));
            Assert.AreEqual(string.Empty, element.GetProperty("value"));
            element.SendKeys("hello world");
            Assert.AreEqual("hello world", element.GetProperty("value"));
        }
    }
}
