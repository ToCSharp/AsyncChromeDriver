using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncChromeDriver.Tests
{
 
    [TestFixture]
    public class ClickTest : DriverTestFixture
    {
        [SetUp]
        public async Task SetupMethod()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("clicks.html"));
        }

        [TearDown]
        public async Task TearDownMethod()
        {
            await driver.SwitchTo().DefaultContent();
        }

        [Test]
        public async Task CanClickOnALinkAndFollowIt()
        {
            await driver.WaitForElementWithId("normal").Click();
            await WaitFor(driver.Title(), "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title(), "Browser title was not 'XHTML Test Page'");
        }

        [Test]
        public async Task CanClickOnALinkThatOverflowsAndFollowIt()
        {
            await driver.FindElement(By.Id("overflowLink")).Click();

            await WaitFor(driver.Title(), "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
        }

        [Test]
        public async Task CanClickOnAnAnchorAndNotReloadThePage()
        {
            await ((IJavaScriptExecutor)driver).ExecuteScript("document.latch = true");

            await driver.FindElement(By.Id("anchor")).Click();

            bool samePage = (bool)await ((IJavaScriptExecutor)driver).ExecuteScript("return document.latch");

            Assert.AreEqual(true, samePage, "Latch was reset");
        }

        [Test]
        public async Task CanClickOnALinkThatUpdatesAnotherFrame()
        {
            await driver.SwitchTo().Frame("source");

            await driver.FindElement(By.Id("otherframe")).Click();
            await driver.SwitchTo().DefaultContent().SwitchTo().Frame("target");

            Assert.That(await driver.PageSource(), Does.Contain("Hello WebDriver"));
        }

        [Test]
        public async Task ElementsFoundByJsCanLoadUpdatesInAnotherFrame()
        {
            await driver.SwitchTo().Frame("source");

            IWebElement toClick = (IWebElement)await ((IJavaScriptExecutor)driver).ExecuteScript("return document.getElementById('otherframe');");
            await toClick.Click();
            await driver.SwitchTo().DefaultContent();
            await driver.SwitchTo().Frame("target");

            Assert.That(await driver.PageSource(), Does.Contain("Hello WebDriver"));
        }

        [Test]
        public async Task JsLocatedElementsCanUpdateFramesIfFoundSomehowElse()
        {
            await driver.SwitchTo().Frame("source");

            // Prime the cache of elements
            await driver.FindElement(By.Id("otherframe"));

            // This _should_ return the same element
            IWebElement toClick = (IWebElement)await ((IJavaScriptExecutor)driver).ExecuteScript("return document.getElementById('otherframe');");
            await toClick.Click();
            await driver.SwitchTo().DefaultContent();
            await driver.SwitchTo().Frame("target");

            Assert.That(await driver.PageSource(), Does.Contain("Hello WebDriver"));
        }

        [Test]
        public async Task CanClickOnAnElementWithTopSetToANegativeNumber()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("styledPage.html"));
            IWebElement searchBox = await driver.FindElement(By.Name("searchBox"));
            await searchBox.SendKeys("Cheese");
            await driver.FindElement(By.Name("btn")).Click();

            string log = await driver.FindElement(By.Id("log")).Text();
            Assert.AreEqual("click", log);
        }

        [Test]
        public async Task ShouldSetRelatedTargetForMouseOver()
        {
            await driver.GoToUrl(javascriptPage);

            await driver.FindElement(By.Id("movable")).Click();

            string log = await driver.FindElement(By.Id("result")).Text();

            // Note: It is not guaranteed that the relatedTarget property of the mouseover
            // event will be the parent, when using native events. Only check that the mouse
            // has moved to this element, not that the parent element was the related target.
            if (this.IsNativeEventsEnabled) {
                Assert.That(log, Does.StartWith("parent matches?"));
            } else {
                Assert.AreEqual("parent matches? true", log);
            }
        }

        [Test]
        public async Task ShouldClickOnFirstBoundingClientRectWithNonZeroSize()
        {
            await driver.FindElement(By.Id("twoClientRects")).Click();
            await WaitFor(async () => await driver.Title() == "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldOnlyFollowHrefOnce()
        {
            await driver.GoToUrl(clicksPage);
            int windowHandlesBefore = await driver.WindowHandles().Count();

            await driver.FindElement(By.Id("new-window")).Click();
            await WaitFor(async () => await driver.WindowHandles().Count() >= windowHandlesBefore + 1, "Window handles was not " + (windowHandlesBefore + 1).ToString());
            Assert.AreEqual(windowHandlesBefore + 1, await driver.WindowHandles().Count());
        }

        [Test]
        public async Task ClickingLabelShouldSetCheckbox()
        {
            await driver.GoToUrl(formsPage);

            await driver.FindElement(By.Id("label-for-checkbox-with-label")).Click();

            Assert.That(await driver.FindElement(By.Id("checkbox-with-label")).Selected(), "Checkbox should be selected");
        }

        [Test]
        public async Task CanClickOnALinkWithEnclosedImage()
        {
            await driver.FindElement(By.Id("link-with-enclosed-image")).Click();
            await WaitFor(async () => await driver.Title() == "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task CanClickOnAnImageEnclosedInALink()
        {
            await driver.FindElement(By.Id("link-with-enclosed-image")).FindElement(By.TagName("img")).Click();
            await WaitFor(async () => await driver.Title() == "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task CanClickOnALinkThatContainsTextWrappedInASpan()
        {
            await driver.FindElement(By.Id("link-with-enclosed-span")).Click();
            await WaitFor(async () => await driver.Title() == "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task CanClickOnALinkThatContainsEmbeddedBlockElements()
        {
            await driver.FindElement(By.Id("embeddedBlock")).Click();
            await WaitFor(async () => await driver.Title() == "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task CanClickOnAnElementEnclosedInALink()
        {
            await driver.FindElement(By.Id("link-with-enclosed-span")).FindElement(By.TagName("span")).Click();
            await WaitFor(async () => await driver.Title() == "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        // See http://code.google.com/p/selenium/issues/attachmentText?id=2700
        [Test]
        public async Task ShouldBeAbleToClickOnAnElementInTheViewport()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_out_of_bounds.html"));

            IWebElement button = await driver.FindElement(By.Id("button"));
            await button.Click();
        }

        [Test]
        public async Task ClicksASurroundingStrongTag()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("ClickTest_testClicksASurroundingStrongTag.html"));
            await driver.FindElement(By.TagName("a")).Click();
            await WaitFor(async () => await driver.Title() == "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
        }

        [Test]
        public async Task CanClickAnImageMapArea()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/google_map.html"));
            await driver.FindElement(By.Id("rectG")).Click();
            await WaitFor(async () => await driver.Title() == "Target Page 1", "Browser title was not 'Target Page 1'");

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/google_map.html"));
            await driver.FindElement(By.Id("circleO")).Click();
            await WaitFor(async () => await driver.Title() == "Target Page 2", "Browser title was not 'Target Page 2'");

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/google_map.html"));
            await driver.FindElement(By.Id("polyLE")).Click();
            await WaitFor(async () => await driver.Title() == "Target Page 3", "Browser title was not 'Target Page 3'");
        }

        [Test]
        public async Task ShouldBeAbleToClickOnAnElementGreaterThanTwoViewports()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_too_big.html"));

            await driver.FindElement(By.Id("click")).Click();

            await WaitFor(async () => await driver.Title() == "clicks", "Browser title was not 'clicks'");
        }

        [Test]
        public async Task ShouldBeAbleToClickOnRightToLeftLanguageLink()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_rtl.html"));

            await driver.FindElement(By.Id("ar_link")).Click();

            await WaitFor(async () => await driver.Title() == "clicks", "Expected title to be 'clicks'");
            Assert.AreEqual("clicks", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToClickOnLinkInAbsolutelyPositionedFooter()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("fixedFooterNoScroll.html"));

            await driver.FindElement(By.Id("link")).Click();
            await WaitFor(driver.Title(), "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToClickOnLinkInAbsolutelyPositionedFooterInQuirksMode()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("fixedFooterNoScrollQuirksMode.html"));

            await driver.FindElement(By.Id("link")).Click();
            await WaitFor(driver.Title(), "XHTML Test Page", "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToClickOnLinksWithNoHrefAttribute()
        {
            await driver.GoToUrl(javascriptPage);

            await driver.FindElement(By.LinkText("No href")).Click();

            await WaitFor(driver.Title(),  "Changed", "Expected title to be 'Changed'");
            Assert.AreEqual("Changed", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToClickOnALinkThatWrapsToTheNextLine()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/link_that_wraps.html"));

            await driver.FindElement(By.Id("link")).Click();

            await WaitFor(driver.Title(), "Submitted Successfully!", "Expected title to be 'Submitted Successfully!'");
            Assert.AreEqual("Submitted Successfully!", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToClickOnASpanThatWrapsToTheNextLine()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/span_that_wraps.html"));

            await driver.FindElement(By.Id("span")).Click();

            await WaitFor(driver.Title(), "Submitted Successfully!", "Expected title to be 'Submitted Successfully!'");
            Assert.AreEqual("Submitted Successfully!", await driver.Title());
        }

        [Test]
        public async Task ClickingOnADisabledElementIsANoOp()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/disabled_element.html"));

            IWebElement element = await driver.FindElement(By.Name("disabled"));
            await element.Click();
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldBeAbleToClickLinkContainingLineBreak()
        {
            await driver.GoToUrl(simpleTestPage);
            await driver.FindElement(By.Id("multilinelink")).Click();
            await WaitFor(async () => await driver.Title() == "We Arrive Here", "Browser title was not 'We Arrive Here'");
            Assert.AreEqual("We Arrive Here", await driver.Title());
        }
    }
}
