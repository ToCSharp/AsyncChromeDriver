using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class TextPagesTest: DriverTestFixture
    {
        private string textPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("plain.txt");

        [Test]
        public async Task ShouldBeAbleToLoadASimplePageOfText()
        {
           await driver.GoToUrl(textPage);
            string source =  await driver.PageSource();
            Assert.That(source, Does.Contain("Test"));
        }


        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task FindingAnElementOnAPlainTextPageWillNeverWork()
        {
           await driver.GoToUrl(textPage);
            //Assert.That(async () => await driver.FindElement(By.Id("foo")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("foo")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }
    }
}
