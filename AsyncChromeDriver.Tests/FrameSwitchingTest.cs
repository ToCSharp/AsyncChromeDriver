using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class FrameSwitchingTest : DriverTestFixture
    {
        // ----------------------------------------------------------------------------------------------
        //
        // Tests that WebDriver doesn't do anything fishy when it navigates to a page with frames.
        //
        // ----------------------------------------------------------------------------------------------

        [Test]
        public async Task ShouldAlwaysFocusOnTheTopMostFrameAfterANavigationEvent()
        {
            await driver.GoToUrl(framesetPage);
            IWebElement element = await driver.FindElement(By.TagName("frameset"));
            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public async Task ShouldNotAutomaticallySwitchFocusToAnIFrameWhenAPageContainingThemIsLoaded()
        {
            await driver.GoToUrl(iframePage);
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromSeconds(1));
            IWebElement element = await driver.FindElement(By.Id("iframe_page_heading"));
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromSeconds(0));
            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public async Task ShouldOpenPageWithBrokenFrameset()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("framesetPage3.html"));

            IWebElement frame1 = await driver.FindElement(By.Id("first"));
            await driver.SwitchTo().Frame(frame1);

            await driver.SwitchTo().DefaultContent();

            IWebElement frame2 = await driver.FindElement(By.Id("second"));

            try {
                await driver.SwitchTo().Frame(frame2);
            } catch (WebDriverException) {
                // IE9 can not switch to this broken frame - it has no window.
            }
        }

        // ----------------------------------------------------------------------------------------------
        //
        // Tests that WebDriver can switch to frames as expected.
        //
        // ----------------------------------------------------------------------------------------------

        [Test]
        public async Task ShouldBeAbleToSwitchToAFrameByItsIndex()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame(1);

            Assert.AreEqual("2", await driver.FindElement(By.Id("pageNumber")).Text());
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToAnIframeByItsIndex()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame(0);

            Assert.AreEqual("name", await driver.FindElement(By.Name("id-name1")).GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToAFrameByItsName()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("fourth");
            Assert.AreEqual("child1", await driver.FindElement(By.TagName("frame")).GetAttribute("name"));

        }

        [Test]
        public async Task ShouldBeAbleToSwitchToAnIframeByItsName()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame("iframe1-name");
            Assert.AreEqual("name", await driver.FindElement(By.Name("id-name1")).GetAttribute("value"));

        }

        [Test]
        public async Task ShouldBeAbleToSwitchToAFrameByItsID()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("fifth");
            Assert.AreEqual("Open new window", await driver.FindElement(By.Name("windowOne")).Text());

        }

        [Test]
        public async Task ShouldBeAbleToSwitchToAnIframeByItsID()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame("iframe1");
            Assert.AreEqual("name", await driver.FindElement(By.Name("id-name1")).GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToFrameWithNameContainingDot()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("sixth.iframe1");
            Assert.That(await driver.FindElement(By.TagName("body")).Text(), Does.Contain("Page number 3"));
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToAFrameUsingAPreviouslyLocatedWebElement()
        {
            await driver.GoToUrl(framesetPage);
            IWebElement frame = await driver.FindElement(By.TagName("frame"));
            await driver.SwitchTo().Frame(frame);
            Assert.AreEqual("1", await driver.FindElement(By.Id("pageNumber")).Text());
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToAnIFrameUsingAPreviouslyLocatedWebElement()
        {
            await driver.GoToUrl(iframePage);
            IWebElement frame = await driver.FindElement(By.TagName("iframe"));
            await driver.SwitchTo().Frame(frame);
            Assert.AreEqual("name", await driver.FindElement(By.Name("id-name1")).GetAttribute("value"));

        }

        [Test]
        public async Task ShouldEnsureElementIsAFrameBeforeSwitching()
        {
            await driver.GoToUrl(framesetPage);
            IWebElement frame = await driver.FindElement(By.TagName("frameset"));
            //Assert.That(async () => await driver.SwitchTo().Frame(frame), Throws.InstanceOf<NoSuchFrameException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.SwitchTo().Frame(frame),
                exception => Assert.AreEqual("NoSuchFrameException", exception.Error));
        }

        [Test]
        public async Task FrameSearchesShouldBeRelativeToTheCurrentlySelectedFrame()
        {
            await driver.GoToUrl(framesetPage);

            IWebElement frameElement = await WaitFor(async () => await driver.FindElement(By.Name("second")), "did not find frame");
            await driver.SwitchTo().Frame(frameElement);
            Assert.AreEqual("2", await driver.FindElement(By.Id("pageNumber")).Text());

            try {
                await driver.SwitchTo().Frame("third");
                Assert.Fail();
            } catch (NoSuchFrameException) {
                // Do nothing
            }

            await driver.SwitchTo().DefaultContent();
            await driver.SwitchTo().Frame("third");

            try {
                await driver.SwitchTo().Frame("second");
                Assert.Fail();
            } catch (NoSuchFrameException) {
                // Do nothing
            }

            await driver.SwitchTo().DefaultContent();
            await driver.SwitchTo().Frame("second");
            Assert.AreEqual("2", await driver.FindElement(By.Id("pageNumber")).Text());
        }

        [Test]
        public async Task ShouldSelectChildFramesByChainedCalls()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("fourth").SwitchTo().Frame("child2");
            Assert.AreEqual("11", await driver.FindElement(By.Id("pageNumber")).Text());
        }

        [Test]
        public async Task ShouldThrowFrameNotFoundExceptionLookingUpSubFramesWithSuperFrameNames()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("fourth");
            //Assert.That(async () => await driver.SwitchTo().Frame("second"), Throws.InstanceOf<NoSuchFrameException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.SwitchTo().Frame("second"),
                exception => Assert.AreEqual("NoSuchFrameException", exception.Error));

        }

        [Test]
        public async Task ShouldThrowAnExceptionWhenAFrameCannotBeFound()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.SwitchTo().Frame("Nothing here"), Throws.InstanceOf<NoSuchFrameException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.SwitchTo().Frame("Nothing here"),
                exception => Assert.AreEqual("NoSuchFrameException", exception.Error));
        }

        [Test]
        public async Task ShouldThrowAnExceptionWhenAFrameCannotBeFoundByIndex()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.SwitchTo().Frame(27), Throws.InstanceOf<NoSuchFrameException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.SwitchTo().Frame(27),
                exception => Assert.AreEqual("NoSuchFrameException", exception.Error));
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToParentFrame()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("fourth").SwitchTo().ParentFrame().SwitchTo().Frame("first");
            Assert.AreEqual("1", await driver.FindElement(By.Id("pageNumber")).Text());
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToParentFrameFromASecondLevelFrame()
        {
            await driver.GoToUrl(framesetPage);

            await driver.SwitchTo().Frame("fourth").SwitchTo().Frame("child1").SwitchTo().ParentFrame().SwitchTo().Frame("child2");
            Assert.AreEqual("11", await driver.FindElement(By.Id("pageNumber")).Text());
        }

        [Test]
        public async Task SwitchingToParentFrameFromDefaultContextIsNoOp()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.SwitchTo().ParentFrame();
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToParentFromAnIframe()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame(0);

            await driver.SwitchTo().ParentFrame();
            await driver.FindElement(By.Id("iframe_page_heading"));
        }

        // ----------------------------------------------------------------------------------------------
        //
        // General frame handling behavior tests
        //
        // ----------------------------------------------------------------------------------------------
        [Test]
        public async Task ShouldContinueToReferToTheSameFrameOnceItHasBeenSelected()
        {
            await driver.GoToUrl(framesetPage);

            await driver.SwitchTo().Frame(2);
            IWebElement checkbox = await driver.FindElement(By.XPath("//input[@name='checky']"));
            await checkbox.Click();
            await checkbox.Submit();

            Assert.AreEqual("Success!", await driver.FindElement(By.XPath("//p")).Text());
        }

        [Test]
        public async Task ShouldFocusOnTheReplacementWhenAFrameFollowsALinkToA_TopTargettedPage()
        {
            await driver.GoToUrl(framesetPage);

            await driver.SwitchTo().Frame(0);
            await driver.FindElement(By.LinkText("top")).Click();

            // TODO(simon): Avoid going too fast when native events are there.
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual("XHTML Test Page", await driver.Title());
        }

        [Test]
        public async Task ShouldAllowAUserToSwitchFromAnIframeBackToTheMainContentOfThePage()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame(0);

            await driver.SwitchTo().DefaultContent();
            await driver.FindElement(By.Id("iframe_page_heading"));
        }


        [Test]
        public async Task ShouldAllowTheUserToSwitchToAnIFrameAndRemainFocusedOnIt()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame(0);

            await driver.FindElement(By.Id("submitButton")).Click();

            string hello = await GetTextOfGreetingElement();
            Assert.AreEqual(hello, "Success!");
        }

        [Test]
        public async Task ShouldBeAbleToClickInAFrame()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("third");

            // This should replace frame "third" ...
            await driver.FindElement(By.Id("submitButton")).Click();

            // driver should still be focused on frame "third" ...
            Assert.AreEqual("Success!", await GetTextOfGreetingElement());

            // Make sure it was really frame "third" which was replaced ...
            await driver.SwitchTo().DefaultContent().SwitchTo().Frame("third");
            Assert.AreEqual("Success!", await GetTextOfGreetingElement());
        }

        [Test]
        public async Task ShouldBeAbleToClickInAFrameThatRewritesTopWindowLocation()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/issue5237.html"));
            await driver.SwitchTo().Frame("search");
            await driver.FindElement(By.Id("submit")).Click();
            await driver.SwitchTo().DefaultContent();
            await WaitFor(driver.Title(), "Target page for issue 5237", "Browser title was not 'Target page for issue 5237'");
        }

        [Test]
        public async Task ShouldBeAbleToClickInASubFrame()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("sixth").SwitchTo().Frame("iframe1");

            // This should replaxe frame "iframe1" inside frame "sixth" ...
            await driver.FindElement(By.Id("submitButton")).Click();

            // driver should still be focused on frame "iframe1" inside frame "sixth" ...
            Assert.AreEqual("Success!", await GetTextOfGreetingElement());

            // Make sure it was really frame "iframe1" inside frame "sixth" which was replaced ...
            await driver.SwitchTo().DefaultContent().SwitchTo().Frame("sixth").SwitchTo().Frame("iframe1");
            Assert.AreEqual("Success!", await driver.FindElement(By.Id("greeting")).Text());
        }

        [Test]
        public async Task ShouldBeAbleToFindElementsInIframesByXPath()
        {
            await driver.GoToUrl(iframePage);

            await driver.SwitchTo().Frame("iframe1");

            IWebElement element = await driver.FindElement(By.XPath("//*[@id = 'changeme']"));

            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public async Task GetCurrentUrlShouldReturnTopLevelBrowsingContextUrl()
        {
            await driver.GoToUrl(framesetPage);
            Assert.AreEqual(framesetPage, await driver.GetUrl());

            await driver.SwitchTo().Frame("second");
            Assert.AreEqual(framesetPage, await driver.GetUrl());
        }

        [Test]
        public async Task GetCurrentUrlShouldReturnTopLevelBrowsingContextUrlForIframes()
        {
            await driver.GoToUrl(iframePage);
            Assert.AreEqual(iframePage, await driver.GetUrl());


            await driver.SwitchTo().Frame("iframe1");
            Assert.AreEqual(iframePage, await driver.GetUrl());
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToTheTopIfTheFrameIsDeletedFromUnderUs()
        {
            await driver.GoToUrl(deletingFrame);
            await driver.SwitchTo().Frame("iframe1");

            IWebElement killIframe = await driver.FindElement(By.Id("killIframe"));
            await killIframe.Click();
            await driver.SwitchTo().DefaultContent();
            await AssertFrameNotPresent("iframe1");

            IWebElement addIFrame = await driver.FindElement(By.Id("addBackFrame"));
            await addIFrame.Click();
            await WaitFor(() => driver.FindElement(By.Id("iframe1")), "Did not find frame element");

            await driver.SwitchTo().Frame("iframe1");
            await WaitFor(() => driver.FindElement(By.Id("success")), "Did not find element in frame");
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToTheTopIfTheFrameIsDeletedFromUnderUsWithFrameIndex()
        {
            await driver.GoToUrl(deletingFrame);
            int iframe = 0;
            await WaitFor(() => FrameExistsAndSwitchedTo(iframe), "Did not switch to frame");

            // we should be in the frame now
            IWebElement killIframe = await driver.FindElement(By.Id("killIframe"));
            await killIframe.Click();
            await driver.SwitchTo().DefaultContent();

            IWebElement addIFrame = await driver.FindElement(By.Id("addBackFrame"));
            await addIFrame.Click();
            await WaitFor(() => FrameExistsAndSwitchedTo(iframe), "Did not switch to frame");
            await WaitFor(() => driver.FindElement(By.Id("success")), "Did not find element in frame");
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToTheTopIfTheFrameIsDeletedFromUnderUsWithWebelement()
        {
            await driver.GoToUrl(deletingFrame);
            IWebElement iframe = await driver.FindElement(By.Id("iframe1"));
            await WaitFor(() => FrameExistsAndSwitchedTo(iframe), "Did not switch to frame");

            // we should be in the frame now
            IWebElement killIframe = await driver.FindElement(By.Id("killIframe"));
            await killIframe.Click();
            await driver.SwitchTo().DefaultContent();

            IWebElement addIFrame = await driver.FindElement(By.Id("addBackFrame"));
            await addIFrame.Click();

            iframe = await driver.FindElement(By.Id("iframe1"));
            await WaitFor(() => FrameExistsAndSwitchedTo(iframe), "Did not switch to frame");
            await WaitFor(() => driver.FindElement(By.Id("success")), "Did not find element in frame");
        }

        [Test]
        public async Task ShouldReturnWindowTitleInAFrameset()
        {
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("third");
            Assert.AreEqual("Unique title", await driver.Title());
        }

        [Test]
        public async Task JavaScriptShouldExecuteInTheContextOfTheCurrentFrame()
        {
            IJavaScriptExecutor executor = driver as IJavaScriptExecutor;

            await driver.GoToUrl(framesetPage);
            Assert.That((bool)await executor.ExecuteScript("return window == window.top"), Is.True);

            await driver.SwitchTo().Frame("third");
            Assert.That((bool)await executor.ExecuteScript("return window != window.top"), Is.True);
        }

        [Test]
        public async Task ShouldNotSwitchMagicallyToTheTopWindow()
        {
            string baseUrl = EnvironmentManager.Instance.UrlBuilder.WhereIs("frame_switching_tests/");
            await driver.GoToUrl(baseUrl + "bug4876.html");
            await driver.SwitchTo().Frame(0);
            await WaitFor(() => driver.FindElement(By.Id("inputText")), "Could not find element");

            for (int i = 0; i < 20; i++) {
                try {
                    IWebElement input = await WaitFor(async () => await driver.FindElement(By.Id("inputText")), "Did not find element");
                    IWebElement submit = await WaitFor(async () => await driver.FindElement(By.Id("submitButton")), "Did not find input element");
                    await input.Clear();
                    await input.SendKeys("rand" + new Random().Next());
                    await submit.Click();
                } finally {
                    string url = (string)await ((IJavaScriptExecutor)driver).ExecuteScript("return window.location.href");
                    // IE6 and Chrome add "?"-symbol to the end of the URL
                    if (url.EndsWith("?")) {
                        url = url.Substring(0, url.Length - 1);
                    }
                    Assert.AreEqual(baseUrl + "bug4876_iframe.html", url);
                }
            }
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task GetShouldSwitchToDefaultContext()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame(await driver.FindElement(By.Id("iframe1")));
            await driver.FindElement(By.Id("cheese")); // Found on formPage.html but not on iframes.html.

            await driver.GoToUrl(iframePage); // This must effectively switchTo().defaultContent(), too.
            await driver.FindElement(By.Id("iframe1"));
        }

        // ----------------------------------------------------------------------------------------------
        //
        // Frame handling behavior tests not included in Java tests
        //
        // ----------------------------------------------------------------------------------------------

        [Test]
        public async Task ShouldBeAbleToFlipToAFrameIdentifiedByItsId()
        {
            await driver.GoToUrl(framesetPage);

            await driver.SwitchTo().Frame("fifth");
            await driver.FindElement(By.Id("username"));
        }

        [Test]
        public async Task ShouldBeAbleToSelectAFrameByName()
        {
            await driver.GoToUrl(framesetPage);

            await driver.SwitchTo().Frame("second");
            Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "2");

            await driver.SwitchTo().DefaultContent().SwitchTo().Frame("third");
            await driver.FindElement(By.Id("changeme")).Click();

            await driver.SwitchTo().DefaultContent().SwitchTo().Frame("second");
            Assert.AreEqual(await driver.FindElement(By.Id("pageNumber")).Text(), "2");
        }

        [Test]
        public async Task ShouldBeAbleToFindElementsInIframesByName()
        {
            await driver.GoToUrl(iframePage);

            await driver.SwitchTo().Frame("iframe1");
            IWebElement element = await driver.FindElement(By.Name("id-name1"));

            Assert.That(element, Is.Not.Null);
        }

        private async Task<string> GetTextOfGreetingElement()
        {
            string text = string.Empty;
            DateTime end = DateTime.Now.Add(TimeSpan.FromMilliseconds(3000));
            while (DateTime.Now < end) {
                try {
                    IWebElement element = await driver.FindElement(By.Id("greeting"));
                    text = await element.Text();
                    break;
                } catch (NoSuchElementException) {
                }
            }

            return text;
        }

        private async Task AssertFrameNotPresent(string locator)
        {
            await driver.SwitchTo().DefaultContent();
            await WaitFor(async () => !(await FrameExistsAndSwitchedTo(locator)), "Frame still present after timeout");
            await driver.SwitchTo().DefaultContent();
        }

        private async Task<bool> FrameExistsAndSwitchedTo(string locator)
        {
            try {
                await driver.SwitchTo().Frame(locator);
                return true;
            } catch (NoSuchFrameException) {
            }

            return false;
        }

        private async Task<bool> FrameExistsAndSwitchedTo(int index)
        {
            try {
                await driver.SwitchTo().Frame(index);
                return true;
            } catch (NoSuchFrameException) {
            }

            return false;
        }

        private async Task<bool> FrameExistsAndSwitchedTo(IWebElement frameElement)
        {
            try {
                await driver.SwitchTo().Frame(frameElement);
                return true;
            } catch (NoSuchFrameException) {
            }

            return false;
        }
    }
}
