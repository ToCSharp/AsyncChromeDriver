using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class SvgDocumentTest : DriverTestFixture
    {

        [Test]
        public async Task ExecuteScriptInSvgDocument()
        {

            await driver.GoToUrl(svgTestPage);
            IWebElement rect = await driver.FindElement(By.Id("rect"));

            Assert.AreEqual("blue", await rect.GetAttribute("fill"));
            await ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('rect').setAttribute('fill', 'yellow');");
            Assert.AreEqual("yellow", await rect.GetAttribute("fill"));
        }
    }
}
