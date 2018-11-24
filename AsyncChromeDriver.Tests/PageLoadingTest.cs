using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class PageLoadingTest : DriverTestFixture
    {
        private IWebDriver localDriver;

        [SetUp]
        public async Task RestartOriginalDriver()
        {
            driver = EnvironmentManager.Instance.GetCurrentDriver();
        }

        [TearDown]
        public async Task QuitAdditionalDriver()
        {
            if (localDriver != null) {
                await localDriver.Quit();
                localDriver = null;
            }
        }

        [Test]
        public async Task NoneStrategyShouldNotWaitForPageToLoad()
        {
            InitLocalDriver(PageLoadStrategy.None);

            string slowPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("sleep?time=5");

            DateTime start = DateTime.Now;
            localDriver.Url = slowPage;
            DateTime end = DateTime.Now;

            TimeSpan duration = end - start;
            // The slow loading resource on that page takes 6 seconds to return,
            // but with 'none' page loading strategy 'get' operation should not wait.
            Assert.That(duration.TotalMilliseconds, Is.LessThan(1000), "Took too long to load page: " + duration.TotalMilliseconds);
        }


        [Test]
        public async Task NoneStrategyShouldNotWaitForPageToRefresh()
        {
            InitLocalDriver(PageLoadStrategy.None);

            string slowPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("sleep?time=5");
            await // We discard the element, but want a check to make sure the page is loaded
                       WaitFor(() => localDriver.FindElement(By.TagName("body")), TimeSpan.FromSeconds(10), "did not find body");

            DateTime start = DateTime.Now;
            await localDriver.Navigate().Refresh();
            DateTime end = DateTime.Now;

            TimeSpan duration = end - start;
            // The slow loading resource on that page takes 6 seconds to return,
            // but with 'none' page loading strategy 'refresh' operation should not wait.
            Assert.That(duration.TotalMilliseconds, Is.LessThan(1000), "Took too long to load page: " + duration.TotalMilliseconds);
        }

        [Test]
        [IgnoreBrowser(Browser.Chrome, "Chrome driver does not support eager page load strategy")]
        public async Task EagerStrategyShouldNotWaitForResources()
        {
            InitLocalDriver(PageLoadStrategy.Eager);

            string slowPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("slowLoadingResourcePage.html");

            DateTime start = DateTime.Now;
            localDriver.Url = slowPage;
            await // We discard the element, but want a check to make sure the GET actually
                  // completed.
                       WaitFor(() => localDriver.FindElement(By.Id("peas")), TimeSpan.FromSeconds(10), "did not find element");
            DateTime end = DateTime.Now;

            // The slow loading resource on that page takes 6 seconds to return. If we
            // waited for it, our load time should be over 6 seconds.
            TimeSpan duration = end - start;
            Assert.That(duration.TotalMilliseconds, Is.LessThan(5 * 1000), "Took too long to load page: " + duration.TotalMilliseconds);
        }

        [Test]
        [IgnoreBrowser(Browser.Chrome, "Chrome driver does not support eager page load strategy")]
        public async Task EagerStrategyShouldNotWaitForResourcesOnRefresh()
        {
            InitLocalDriver(PageLoadStrategy.Eager);

            string slowPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("slowLoadingResourcePage.html");
            localDriver.Url = slowPage;
            await // We discard the element, but want a check to make sure the GET actually
                  // completed.
                       WaitFor(() => localDriver.FindElement(By.Id("peas")), TimeSpan.FromSeconds(10), "did not find element");

            DateTime start = DateTime.Now;
            await localDriver.Navigate().Refresh();
            await // We discard the element, but want a check to make sure the GET actually
                  // completed.
                       WaitFor(() => localDriver.FindElement(By.Id("peas")), TimeSpan.FromSeconds(10), "did not find element");
            DateTime end = DateTime.Now;

            // The slow loading resource on that page takes 6 seconds to return. If we
            // waited for it, our load time should be over 6 seconds.
            TimeSpan duration = end - start;
            Assert.That(duration.TotalMilliseconds, Is.LessThan(5 * 1000), "Took too long to load page: " + duration.TotalMilliseconds);
        }

        [Test]
        [IgnoreBrowser(Browser.Chrome, "Chrome driver does not support eager page load strategy")]
        public async Task EagerStrategyShouldWaitForDocumentToBeLoaded()
        {
            InitLocalDriver(PageLoadStrategy.Eager);

            string slowPage = EnvironmentManager.Instance.UrlBuilder.WhereIs("sleep?time=3");

            localDriver.Url = slowPage;
            await // We discard the element, but want a check to make sure the GET actually completed.
                       WaitFor(() => localDriver.FindElement(By.TagName("body")), TimeSpan.FromSeconds(10), "did not find body");
        }

        [Test]
        public async Task NormalStrategyShouldWaitForDocumentToBeLoaded()
        {
            await driver.GoToUrl(simpleTestPage);

            Assert.AreEqual(await driver.Title(), "Hello WebDriver");
        }

        [Test]
        public async Task ShouldFollowRedirectsSentInTheHttpResponseHeaders()
        {
            await driver.GoToUrl(redirectPage);
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldFollowMetaRedirects()
        {
            await driver.GoToUrl(metaRedirectPage);
            WaitFor(() => { return driver.Title == "We Arrive Here"; }, "Browser title was not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldBeAbleToGetAFragmentOnTheCurrentPage()
        {
            if (TestUtilities.IsMarionette(driver)) {
                // Don't run this test on Marionette.
                Assert.Ignore("Marionette doesn't see subsequent navigation to a fragment as a new navigation.");
            }

            await driver.GoToUrl(xhtmlTestPage);
            await driver.GoToUrl(xhtmlTestPage + "#text");
            await driver.FindElement(By.Id("id1"));
        }

        [Test]
        public async Task ShouldReturnWhenGettingAUrlThatDoesNotResolve()
        {
            try {
                // Of course, we're up the creek if this ever does get registered
                await driver.GoToUrl("http://www.thisurldoesnotexist.comx/");
            } catch (Exception e) {
                if (!IsIeDriverTimedOutException(e)) {
                    throw e;
                }
            }
        }

        [Test]
        [IgnoreBrowser(Browser.Safari, "Safari driver does not throw on malformed URL, causing long delay awaiting timeout")]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldThrowIfUrlIsMalformed()
        {
            Assert.That(() => await driver.GoToUrl("www.test.com", Throws.InstanceOf<WebDriverException>()));
        }

        [Test]
        [IgnoreBrowser(Browser.Safari, "Safari driver does not throw on malformed URL")]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldThrowIfUrlIsMalformedInPortPart()
        {
            Assert.That(() => await driver.GoToUrl("http://localhost:30001bla", Throws.InstanceOf<WebDriverException>()));
        }

        [Test]
        public async Task ShouldReturnWhenGettingAUrlThatDoesNotConnect()
        {
            // Here's hoping that there's nothing here. There shouldn't be
            await driver.GoToUrl("http://localhost:3001");
        }

        [Test]
        public async Task ShouldReturnUrlOnNotExistedPage()
        {
            string url = EnvironmentManager.Instance.UrlBuilder.WhereIs("not_existed_page.html");
            await driver.GoToUrl(url);
            Assert.AreEqual(url, await driver.GetUrl());
        }

        [Test]
        public async Task ShouldBeAbleToLoadAPageWithFramesetsAndWaitUntilAllFramesAreLoaded()
        {
            await driver.GoToUrl(framesetPage);

            await driver.SwitchTo().Frame(0);
            IWebElement pageNumber = await driver.FindElement(By.XPath("//span[@id='pageNumber']"));
            Assert.AreEqual(pageNumber.Text.Trim(), "1");

            await driver.SwitchTo().DefaultContent().SwitchTo().Frame(1);
            pageNumber = await driver.FindElement(By.XPath("//span[@id='pageNumber']"));
            Assert.AreEqual(pageNumber.Text.Trim(), "2");
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldDoNothingIfThereIsNothingToGoBackTo()
        {
            string originalTitle = await driver.Title();
            await driver.GoToUrl(formsPage);
            await driver.Navigate().Back();
            // We may have returned to the browser's home page
            string currentTitle = await driver.Title();
            Assert.That(currentTitle, Is.EqualTo(originalTitle).Or.EqualTo("We Leave From Here"));
            if (driver.Title == originalTitle) {
                await driver.Navigate().Back();
                Assert.AreEqual(originalTitle, await driver.Title());
            }
        }

        [Test]
        public async Task ShouldBeAbleToNavigateBackInTheBrowserHistory()
        {
            await driver.GoToUrl(formsPage);

            await driver.FindElement(By.Id("imageButton")).Submit();
            WaitFor(TitleToBeEqualTo("We Arrive Here"), "Browser title was not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
            await driver.Navigate().Back();
            WaitFor(TitleToBeEqualTo("We Leave From Here"), "Browser title was not 'We Leave From Here'");
            Assert.AreEqual(await driver.Title(), "We Leave From Here");
        }

        [Test]
        public async Task ShouldBeAbleToNavigateBackInTheBrowserHistoryInPresenceOfIframes()
        {
            await driver.GoToUrl(xhtmlTestPage);

            await driver.FindElement(By.Name("sameWindow")).Click();
            WaitFor(TitleToBeEqualTo("This page has iframes"), "Browser title was not 'This page has iframes'");
            Assert.AreEqual(await driver.Title(), "This page has iframes");
            await driver.Navigate().Back();
            WaitFor(TitleToBeEqualTo("XHTML Test Page"), "Browser title was not 'XHTML Test Page'");
            Assert.AreEqual(await driver.Title(), "XHTML Test Page");
        }

        [Test]
        public async Task ShouldBeAbleToNavigateForwardsInTheBrowserHistory()
        {
            await driver.GoToUrl(formsPage);

            await driver.FindElement(By.Id("imageButton")).Submit();
            WaitFor(TitleToBeEqualTo("We Arrive Here"), "Browser title was not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
            await driver.Navigate().Back();
            WaitFor(TitleToBeEqualTo("We Leave From Here"), "Browser title was not 'We Leave From Here'");
            Assert.AreEqual(await driver.Title(), "We Leave From Here");
            await driver.Navigate().Forward();
            WaitFor(TitleToBeEqualTo("We Arrive Here"), "Browser title was not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        [IgnoreBrowser(Browser.IE, "Browser does not support using insecure SSL certs")]
        [IgnoreBrowser(Browser.Safari, "Browser does not support using insecure SSL certs")]
        [IgnoreBrowser(Browser.Edge, "Browser does not support using insecure SSL certs")]
        public async Task ShouldBeAbleToAccessPagesWithAnInsecureSslCertificate()
        {
            String url = EnvironmentManager.Instance.UrlBuilder.WhereIsSecure("simpleTest.html");
            await driver.GoToUrl(url);

            // This should work
            Assert.AreEqual(await driver.Title(), "Hello WebDriver");
        }

        [Test]
        public async Task ShouldBeAbleToRefreshAPage()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.Navigate().Refresh();

            Assert.AreEqual(await driver.Title(), "XHTML Test Page");
        }

        /// <summary>
        /// see <a href="http://code.google.com/p/selenium/issues/detail?id=208">Issue 208</a>
        /// </summary>
        [Test]
        [IgnoreBrowser(Browser.IE, "Browser does, in fact, hang in this case.")]
        [IgnoreBrowser(Browser.Firefox, "Browser does, in fact, hang in this case.")]
        public async Task ShouldNotHangIfDocumentOpenCallIsNeverFollowedByDocumentCloseCall()
        {
            await driver.GoToUrl(documentWrite);

            // If this command succeeds, then all is well.
            await driver.FindElement(By.XPath("//body"));
        }

        [Test]
        [IgnoreBrowser(Browser.Safari)]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task PageLoadTimeoutCanBeChanged()
        {
            await TestPageLoadTimeoutIsEnforced(2);
            await TestPageLoadTimeoutIsEnforced(3);
        }

        [Test]
        [IgnoreBrowser(Browser.Safari)]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task CanHandleSequentialPageLoadTimeouts()
        {
            long pageLoadTimeout = 2;
            long pageLoadTimeBuffer = 10;
            string slowLoadingPageUrl = EnvironmentManager.Instance.UrlBuilder.WhereIs("sleep?time=" + (pageLoadTimeout + pageLoadTimeBuffer));
            await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(2));
            AssertPageLoadTimeoutIsEnforced(() => await driver.GoToUrl(slowLoadingPageUrl, pageLoadTimeout, pageLoadTimeBuffer));
            AssertPageLoadTimeoutIsEnforced(() => await driver.GoToUrl(slowLoadingPageUrl, pageLoadTimeout, pageLoadTimeBuffer));
        }

        [Test]
        [IgnoreBrowser(Browser.Opera, "Not implemented for browser")]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldTimeoutIfAPageTakesTooLongToLoad()
        {
            try {
                await TestPageLoadTimeoutIsEnforced(2);
            } finally {
                await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(300));
            }

            // Load another page after get() timed out but before test HTTP server served previous page.
            await driver.GoToUrl(xhtmlTestPage);
            WaitFor(TitleToBeEqualTo("XHTML Test Page"), "Title was not expected value");
        }

        [Test]
        [IgnoreBrowser(Browser.Opera, "Not implemented for browser")]
        [IgnoreBrowser(Browser.Edge, "Not implemented for browser")]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldTimeoutIfAPageTakesTooLongToLoadAfterClick()
        {
            await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(2));

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("page_with_link_to_slow_loading_page.html"));
            IWebElement link = WaitFor(() => driver.FindElement(By.Id("link-to-slow-loading-page")), "Could not find link");

            try {
                await AssertPageLoadTimeoutIsEnforced(() => link.Click(), 2, 3);
            } finally {
                await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(300));
            }

            // Load another page after get() timed out but before test HTTP server served previous page.
            await driver.GoToUrl(xhtmlTestPage);
            WaitFor(TitleToBeEqualTo("XHTML Test Page"), "Title was not expected value");
        }

        [Test]
        [IgnoreBrowser(Browser.Opera, "Not implemented for browser")]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldTimeoutIfAPageTakesTooLongToRefresh()
        {
            // Get the sleeping servlet with a pause of 5 seconds
            long pageLoadTimeout = 2;
            long pageLoadTimeBuffer = 0;
            string slowLoadingPageUrl = EnvironmentManager.Instance.UrlBuilder.WhereIs("sleep?time=" + (pageLoadTimeout + pageLoadTimeBuffer));
            await driver.GoToUrl(slowLoadingPageUrl);

            await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(2));

            try {
                await AssertPageLoadTimeoutIsEnforced(() => driver.Navigate().Refresh(), 2, 4);
            } finally {
                await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(300));
            }

            // Load another page after get() timed out but before test HTTP server served previous page.
            await driver.GoToUrl(xhtmlTestPage);
            WaitFor(TitleToBeEqualTo("XHTML Test Page"), "Title was not expected value");
        }

        [Test]
        [IgnoreBrowser(Browser.Edge, "Test hangs browser.")]
        [IgnoreBrowser(Browser.Chrome, "Chrome driver does, in fact, stop loading page after a timeout.")]
        [IgnoreBrowser(Browser.Opera, "Not implemented for browser")]
        [IgnoreBrowser(Browser.Safari, "Safari behaves correctly with page load timeout, but getting text does not propertly trim, leading to a test run time of over 30 seconds")]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldNotStopLoadingPageAfterTimeout()
        {
            try {
                await TestPageLoadTimeoutIsEnforced(1);
            } finally {
                await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(300));
            }

            WaitFor(() => {
                try {
                    string text = await driver.FindElement(By.TagName("body")).Text;
                    return text == "Slept for 11s";
                } catch (NoSuchElementException) {
                } catch (StaleElementReferenceException) {
                }
                return false;
            }, TimeSpan.FromSeconds(30), "Did not find expected text");
        }

        private Func<bool> TitleToBeEqualTo(string expectedTitle)
        {
            return () => { return driver.Title == expectedTitle; };
        }

        /**
         * Sets given pageLoadTimeout to the {@link #driver} and asserts that attempt to navigate to a
         * page that takes much longer (10 seconds longer) to load results in a TimeoutException.
         * <p>
         * Side effects: 1) {@link #driver} is configured to use given pageLoadTimeout,
         * 2) test HTTP server still didn't serve the page to browser (some browsers may still
         * be waiting for the page to load despite the fact that driver responded with the timeout).
         */
        private async Task TestPageLoadTimeoutIsEnforced(long webDriverPageLoadTimeoutInSeconds)
        {
            // Test page will load this many seconds longer than WD pageLoadTimeout.
            long pageLoadTimeBufferInSeconds = 10;
            string slowLoadingPageUrl = EnvironmentManager.Instance.UrlBuilder.WhereIs("sleep?time=" + (webDriverPageLoadTimeoutInSeconds + pageLoadTimeBufferInSeconds));
            await driver.Options().Timeouts.SetPageLoad(TimeSpan.FromSeconds(webDriverPageLoadTimeoutInSeconds));
            AssertPageLoadTimeoutIsEnforced(() => await driver.GoToUrl(slowLoadingPageUrl, webDriverPageLoadTimeoutInSeconds, pageLoadTimeBufferInSeconds));
        }

        private async Task AssertPageLoadTimeoutIsEnforced(TestDelegate delegateToTest, long webDriverPageLoadTimeoutInSeconds, long pageLoadTimeBufferInSeconds)
        {
            DateTime start = DateTime.Now;
            Assert.That(delegateToTest, Throws.InstanceOf<WebDriverTimeoutException>(), "I should have timed out after " + webDriverPageLoadTimeoutInSeconds + " seconds");
            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            Assert.That(duration.TotalSeconds, Is.GreaterThan(webDriverPageLoadTimeoutInSeconds));
            Assert.That(duration.TotalSeconds, Is.LessThan(webDriverPageLoadTimeoutInSeconds + pageLoadTimeBufferInSeconds));
        }

        private async Task InitLocalDriver(PageLoadStrategy strategy)
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            if (localDriver != null) {
                await localDriver.Quit();
            }

            PageLoadStrategyOptions options = new PageLoadStrategyOptions();
            options.PageLoadStrategy = strategy;
            localDriver = EnvironmentManager.Instance.CreateDriverInstance(options);
        }

        private class PageLoadStrategyOptions : DriverOptions
        {
            public override async Task AddAdditionalCapability(string capabilityName, object capabilityValue)
            {
            }

            public override ICapabilities ToCapabilities()
            {
                return null;
            }
        }
    }
}
