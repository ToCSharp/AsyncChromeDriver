using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class TargetLocatorTest : DriverTestFixture
    {

        [Test]
        public async Task ShouldThrowExceptionAfterSwitchingToNonExistingFrameIndex()
        {
            await driver.GoToUrl(framesPage);
            //Assert.That(async () => await driver.SwitchTo().Frame(10), Throws.InstanceOf<NoSuchFrameException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.SwitchTo().Frame(10),
                exception => Assert.AreEqual("NoSuchFrameException", exception.Error));
        }

        [Test]
        public async Task ShouldThrowExceptionAfterSwitchingToNonExistingFrameName()
        {
            await driver.GoToUrl(framesPage);
            //Assert.That(async () => await driver.SwitchTo().Frame("æ©ñµøöíúüþ®éåä²doesnotexist"), Throws.InstanceOf<NoSuchFrameException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.SwitchTo().Frame("æ©ñµøöíúüþ®éåä²doesnotexist"),
                exception => Assert.AreEqual("NoSuchFrameException", exception.Error));
        }

        [Test]
        public async Task ShouldThrowExceptionAfterSwitchingToNullFrameName()
        {
            string frameName = null;
            await driver.GoToUrl(framesPage);
            //Assert.That(async () => await driver.SwitchTo().Frame(frameName), Throws.InstanceOf<ArgumentNullException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.SwitchTo().Frame(frameName),
                exception => Assert.AreEqual("NoSuchFrameException", exception.Error));
        }

        [Test]
        public async Task ShouldSwitchToIframeByNameAndBackToDefaultContent()
        {
            await driver.GoToUrl(iframesPage);
            await driver.SwitchTo().Frame("iframe1");
            IWebElement element = await driver.FindElement(By.Name("id-name1"));
            Assert.That(element, Is.Not.Null);

            await driver.SwitchTo().DefaultContent();
            element = await driver.FindElement(By.Id("iframe_page_heading"));
            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public async Task ShouldSwitchToIframeByIndexAndBackToDefaultContent()
        {
            await driver.GoToUrl(iframesPage);
            await driver.SwitchTo().Frame(0);
            IWebElement element = await driver.FindElement(By.Name("id-name1"));
            Assert.That(element, Is.Not.Null);

            await driver.SwitchTo().DefaultContent();
            element = await driver.FindElement(By.Id("iframe_page_heading"));
            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public async Task ShouldSwitchToFrameByNameAndBackToDefaultContent()
        {
            await driver.GoToUrl(framesPage);

            await driver.SwitchTo().Frame("first");
            Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "1");

            await driver.SwitchTo().DefaultContent();
            try {
                // DefaultContent should not have the element in it.
                Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "1");
                Assert.Fail("Should not be able to get element in frame from DefaultContent");
            } catch (NoSuchElementException) {
                Assert.Fail("catch WebBrowserException");
            } catch (WebBrowserException ex) {
                Assert.AreEqual("no such element", ex.Error);
            }

            await driver.SwitchTo().Frame("second");
            Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "2");

            await driver.SwitchTo().DefaultContent();
            try {
                // DefaultContent should not have the element in it.
                Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "1");
                Assert.Fail("Should not be able to get element in frame from DefaultContent");
            } catch (NoSuchElementException) {
                Assert.Fail("catch WebBrowserException");
            } catch (WebBrowserException ex) {
                Assert.AreEqual("no such element", ex.Error);
            }
        }

        [Test]
        public async Task ShouldSwitchToFrameByIndexAndBackToDefaultContent()
        {
            await driver.GoToUrl(framesPage);

            await driver.SwitchTo().Frame(0);
            Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "1");

            await driver.SwitchTo().DefaultContent();
            try {
                // DefaultContent should not have the element in it.
                Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "1");
                Assert.Fail("Should not be able to get element in frame from DefaultContent");
            } catch (NoSuchElementException) {
                Assert.Fail("catch WebBrowserException");
            } catch (WebBrowserException ex) {
                Assert.AreEqual("no such element", ex.Error);
            }

            await driver.SwitchTo().Frame(1);
            Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "2");

            await driver.SwitchTo().DefaultContent();
            try {
                // DefaultContent should not have the element in it.
                Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "1");
                Assert.Fail("Should not be able to get element in frame from DefaultContent");
            } catch (NoSuchElementException) {
                Assert.Fail("catch WebBrowserException");
            } catch (WebBrowserException ex) {
                Assert.AreEqual("no such element", ex.Error);
            }
        }

    }
}
