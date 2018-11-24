using NUnit.Framework;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;
using Size = Zu.WebBrowser.BasicTypes.WebSize;
namespace OpenQA.Selenium
{
    [TestFixture]
    public class StaleElementReferenceTest : DriverTestFixture
    {
        [Test]
        public async Task OldPage()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await elem.Click(), Throws.InstanceOf<StaleElementReferenceException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await elem.Click(),
                exception => Assert.AreEqual("stale element reference", exception.Error));
        }

        [Test]
        public async Task ShouldNotCrashWhenCallingGetSizeOnAnObsoleteElement()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => { WebSize elementSize = await elem.Size(); }, Throws.InstanceOf<StaleElementReferenceException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await elem.Size(),
                exception => Assert.AreEqual("stale element reference", exception.Error));
        }

        [Test]
        public async Task ShouldNotCrashWhenQueryingTheAttributeOfAStaleElement()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement heading = await driver.FindElement(By.XPath("//h1"));
            await driver.GoToUrl(simpleTestPage);
            //Assert.That(async () => { string className = await heading.GetAttribute("class"); }, Throws.InstanceOf<StaleElementReferenceException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await heading.GetAttribute("class"),
                exception => Assert.AreEqual("stale element reference", exception.Error));
        }

        [Test]
        public async Task RemovingAnElementDynamicallyFromTheDomShouldCauseAStaleRefException()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement toBeDeleted = await driver.FindElement(By.Id("deleted"));
            Assert.That(await toBeDeleted.Displayed(), Is.True);

            await driver.FindElement(By.Id("delete")).Click();

            bool wasStale = await WaitFor(async () => {
                try {
                    string tagName = await toBeDeleted.TagName();
                    return false;
                } catch (StaleElementReferenceException) {
                    return true;
                }
            }, "Element did not become stale.");
            Assert.That(wasStale, Is.True, "Element should be stale at this point");
        }
    }
}
