using System;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class ClickScrollingTest : DriverTestFixture
    {
        [Test]
        public async Task ClickingOnAnchorScrollsPage()
        {
            string scrollScript = "var pageY;";
            scrollScript += "if (typeof(window.pageYOffset) == 'number') {";
            scrollScript += "pageY = window.pageYOffset;";
            scrollScript += "} else {";
            scrollScript += "pageY = document.documentElement.scrollTop;";
            scrollScript += "}";
            scrollScript += "return pageY;";

            await driver.GoToUrl(macbethPage);

            await driver.FindElement(By.PartialLinkText("last speech")).Click();

            long yOffset = (long)await ((IJavaScriptExecutor)driver).ExecuteScript(scrollScript);

            //Focusing on to click, but not actually following,
            //the link will scroll it in to view, which is a few pixels further than 0 
            Assert.That(yOffset, Is.GreaterThan(300), "Did not scroll");
        }

        [Test]
        public async Task ShouldScrollToClickOnAnElementHiddenByOverflow()
        {
            string url = EnvironmentManager.Instance.UrlBuilder.WhereIs("click_out_of_bounds_overflow.html");
            await driver.GoToUrl(url);

            IWebElement link = await driver.FindElement(By.Id("link"));
            await link.Click();
        }

        [Test]
        public async Task ShouldBeAbleToClickOnAnElementHiddenByOverflow()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scroll.html"));

            IWebElement link = await driver.FindElement(By.Id("line8"));
            // This used to throw a MoveTargetOutOfBoundsException - we don't expect it to
            await link.Click();
            Assert.AreEqual("line8", await driver.FindElement(By.Id("clicked")).Text());
        }

        [Test]
        public async Task ShouldBeAbleToClickOnAnElementHiddenByDoubleOverflow()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_double_overflow_auto.html"));

            await driver.FindElement(By.Id("link")).Click();
            await WaitFor(TitleToBe("Clicked Successfully!"), "Browser title was not 'Clicked Successfully'");
        }

        [Test]
        public async Task ShouldBeAbleToClickOnAnElementHiddenByYOverflow()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_y_overflow_auto.html"));

            await driver.FindElement(By.Id("link")).Click();
            await WaitFor(TitleToBe("Clicked Successfully!"), "Browser title was not 'Clicked Successfully'");
        }

        [Test]
        public async Task ShouldBeAbleToClickOnAnElementPartiallyHiddenByOverflow()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_partially_hidden_element.html"));

            await driver.FindElement(By.Id("btn")).Click();
            await WaitFor(TitleToBe("Clicked Successfully!"), "Browser title was not 'Clicked Successfully'");
        }

        [Test]
        public async Task ShouldNotScrollOverflowElementsWhichAreVisible()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scroll2.html"));
            IWebElement list = await driver.FindElement(By.TagName("ul"));
            IWebElement item = await list.FindElement(By.Id("desired"));
            await item.Click();
            long yOffset = (long)await ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].scrollTop;", new CancellationToken(), list);
            Assert.AreEqual(0, yOffset, "Should not have scrolled");
        }

        [Test]
        public async Task ShouldBeAbleToClickRadioButtonScrolledIntoView()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scroll4.html"));
            await driver.FindElement(By.Id("radio")).Click();
            // If we don't throw, we're good
        }

        [Test]
        public async Task ShouldScrollOverflowElementsIfClickPointIsOutOfViewButElementIsInView()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scroll5.html"));
            await driver.FindElement(By.Id("inner")).Click();
            Assert.AreEqual("clicked", await driver.FindElement(By.Id("clicked")).Text());
        }

        [Test]
        public async Task ShouldBeAbleToClickElementInAFrameThatIsOutOfView()
        {
            try {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_frame_out_of_view.html"));
                await driver.SwitchTo().Frame("frame");
                IWebElement element = await driver.FindElement(By.Name("checkbox"));
                await element.Click();
                Assert.That(await element.Selected(), "Element is not selected");
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldBeAbleToClickElementThatIsOutOfViewInAFrame()
        {
            try {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_scrolling_frame.html"));
                await driver.SwitchTo().Frame("scrolling_frame");
                IWebElement element = await driver.FindElement(By.Name("scroll_checkbox")).Click();
                Assert.That(await element.Selected(), "Element is not selected");
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        [Ignore("All tested browses scroll non-scrollable frames")]
        public async Task ShouldNotBeAbleToClickElementThatIsOutOfViewInANonScrollableFrame()
        {
            try {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_non_scrolling_frame.html"));
                await driver.SwitchTo().Frame("scrolling_frame");
                IWebElement element = await driver.FindElement(By.Name("scroll_checkbox"));
                //Assert.That(async () => await element.Click(), Throws.InstanceOf<WebDriverException>());
                await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Click(),
                    exception => Assert.AreEqual("WebDriverException", exception.Error));
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldBeAbleToClickElementThatIsOutOfViewInAFrameThatIsOutOfView()
        {
            try {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_scrolling_frame_out_of_view.html"));
                await driver.SwitchTo().Frame("scrolling_frame");
                IWebElement element = await driver.FindElement(By.Name("scroll_checkbox"));
                await element.Click();
                Assert.That(await element.Selected(), "Element is not selected");
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldBeAbleToClickElementThatIsOutOfViewInANestedFrame()
        {
            try {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_nested_scrolling_frames.html"));
                await driver.SwitchTo().Frame("scrolling_frame");
                await driver.SwitchTo().Frame("nested_scrolling_frame");
                IWebElement element = await driver.FindElement(By.Name("scroll_checkbox"));
                await element.Click();
                Assert.That(await element.Selected(), "Element is not selected");
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldBeAbleToClickElementThatIsOutOfViewInANestedFrameThatIsOutOfView()
        {
            try {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_nested_scrolling_frames_out_of_view.html"));
                await driver.SwitchTo().Frame("scrolling_frame");
                await driver.SwitchTo().Frame("nested_scrolling_frame");
                IWebElement element = await driver.FindElement(By.Name("scroll_checkbox"));
                await element.Click();
                Assert.That(await element.Selected(), "Element is not selected");
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldNotScrollWhenGettingElementSize()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scroll3.html"));
            long scrollTop = await GetScrollTop();
            var ignoredSize = await driver.FindElement(By.Id("button1")).Size();
            Assert.AreEqual(scrollTop, await GetScrollTop());
        }

        [Test]
        public async Task ShouldBeAbleToClickElementInATallFrame()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("scrolling_tests/page_with_tall_frame.html"));
            await driver.SwitchTo().Frame("tall_frame");
            IWebElement element = await driver.FindElement(By.Name("checkbox"));
            await element.Click();
            Assert.That(await element.Selected(), "Element is not selected");
        }

        [Test]
        public async Task ShouldBeAbleToClickInlineTextElementWithChildElementAfterScrolling()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                 .WithBody(
                     "<div style='height: 2000px;'>Force scroll needed</div><label id='wrapper'>wraps a checkbox <input id='check' type='checkbox' checked='checked'/></label>")));
            IWebElement label = await driver.FindElement(By.Id("wrapper"));
            await label.Click();
            IWebElement checkbox = await driver.FindElement(By.Id("check"));
            Assert.IsFalse(await checkbox.Selected(), "Checkbox should not be selected after click");
        }

        private async Task<long> GetScrollTop()
        {
            return (long)await ((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollTop;");
        }

        private Func<Task<bool>> TitleToBe(string desiredTitle)
        {
            return async () => await driver.Title() == desiredTitle;
        }
    }
}
