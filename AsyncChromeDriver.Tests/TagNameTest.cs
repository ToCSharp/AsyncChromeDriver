using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class TagNameTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldReturnInput()
        {
            await driver.GoToUrl(formsPage);
            IWebElement selectBox = await driver.FindElement(By.Id("cheese"));
            Assert.AreEqual((await selectBox.TagName()).ToLower(), "input");
        }
    }
}
