using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class GetMultipleAttributeTest : DriverTestFixture
    {
        [Test]
        public async Task MultipleAttributeShouldBeNullWhenNotSet()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithoutMultiple"));
            Assert.That(await element.GetAttribute("multiple"), Is.Null);
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSet()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithMultipleEqualsMultiple"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSelectHasMutilpeWithValueAsBlank()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithEmptyStringMultiple"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSelectHasMutilpeWithoutAValue()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithMultipleWithoutValue"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSelectHasMutilpeWithValueAsSomethingElse()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithRandomMultipleValue"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }
    }
}
