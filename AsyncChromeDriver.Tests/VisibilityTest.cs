using System;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.BrowserOptions;
using Size = Zu.WebBrowser.BasicTypes.WebSize;
namespace OpenQA.Selenium
{
    [TestFixture]
    public class VisibilityTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldAllowTheUserToTellIfAnElementIsDisplayedOrNot()
        {
            await driver.GoToUrl(javascriptPage);

            Assert.That(await driver.FindElement(By.Id("displayed")).Displayed(), Is.True, "Element with ID 'displayed' should be displayed");
            Assert.That(await driver.FindElement(By.Id("none")).Displayed(), Is.False, "Element with ID 'none' should not be displayed");
            Assert.That(await driver.FindElement(By.Id("suppressedParagraph")).Displayed(), Is.False, "Element with ID 'suppressedParagraph' should not be displayed");
            Assert.That(await driver.FindElement(By.Id("hidden")).Displayed(), Is.False, "Element with ID 'hidden' should not be displayed");
        }

        [Test]
        public async Task VisibilityShouldTakeIntoAccountParentVisibility()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement childDiv = await driver.FindElement(By.Id("hiddenchild"));
            IWebElement hiddenLink = await driver.FindElement(By.Id("hiddenlink"));

            Assert.That(await childDiv.Displayed(), Is.False, "Child div should not be displayed");
            Assert.That(await hiddenLink.Displayed(), Is.False, "Hidden link should not be displayed");
        }

        [Test]
        public async Task ShouldCountElementsAsVisibleIfStylePropertyHasBeenSet()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement shown = await driver.FindElement(By.Id("visibleSubElement"));

            Assert.That(await shown.Displayed(), Is.True);
        }

        [Test]
        public async Task ShouldModifyTheVisibilityOfAnElementDynamically()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("hideMe"));

            Assert.That(await element.Displayed(), Is.True);
            await element.Click();

            Assert.That(await element.Displayed(), Is.False);
        }

        [Test]
        public async Task HiddenInputElementsAreNeverVisible()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement shown = await driver.FindElement(By.Name("hidden"));

            Assert.That(await shown.Displayed(), Is.False);
        }

        [Test]
        public async Task ShouldNotBeAbleToClickOnAnElementThatIsNotDisplayed()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("unclickable"));
            //Assert.That(async () => await element.Click(), Throws.InstanceOf<ElementNotInteractableException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Click(),
                exception => Assert.AreEqual("ElementNotInteractableException", exception.Error));
        }

        [Test]
        public async Task ShouldNotBeAbleToTypeAnElementThatIsNotDisplayed()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("unclickable"));
            //Assert.That(async () => await element.SendKeys("You don't see me"), Throws.InstanceOf<ElementNotInteractableException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.SendKeys("You don't see me"),
                exception => Assert.AreEqual("ElementNotInteractableException", exception.Error));

            Assert.That(await element.GetAttribute("value"), Is.Not.EqualTo("You don't see me"));
        }

        [Test]
        public async Task ZeroSizedDivIsShownIfDescendantHasSize()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("zero"));
            Size size = await element.Size();

            Assert.AreEqual(0, size.Width, "Should have 0 width");
            Assert.AreEqual(0, size.Height, "Should have 0 height");
            Assert.That(await element.Displayed(), Is.True);
        }

        [Test]
        public async Task ParentNodeVisibleWhenAllChildrenAreAbsolutelyPositionedAndOverflowIsHidden()
        {
            String url = EnvironmentManager.Instance.UrlBuilder.WhereIs("visibility-css.html");
            await driver.GoToUrl(url);

            IWebElement element = await driver.FindElement(By.Id("suggest"));
            Assert.That(await element.Displayed(), Is.True);
        }

        [Test]
        public async Task ElementHiddenByOverflowXIsNotVisible()
        {
            string[] pages = new string[]{
                "overflow/x_hidden_y_hidden.html",
                "overflow/x_hidden_y_scroll.html",
                "overflow/x_hidden_y_auto.html",
            };
            foreach (string page in pages) {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs(page));
                IWebElement right = await driver.FindElement(By.Id("right"));
                Assert.That(await right.Displayed(), Is.False, "Failed for " + page);
                IWebElement bottomRight = await driver.FindElement(By.Id("bottom-right"));
                Assert.That(await bottomRight.Displayed(), Is.False, "Failed for " + page);
            }
        }

        [Test]
        public async Task ElementHiddenByOverflowYIsNotVisible()
        {
            string[] pages = new string[]{
                "overflow/x_hidden_y_hidden.html",
                "overflow/x_scroll_y_hidden.html",
                "overflow/x_auto_y_hidden.html",
            };
            foreach (string page in pages) {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs(page));
                IWebElement bottom = await driver.FindElement(By.Id("bottom"));
                Assert.That(await bottom.Displayed(), Is.False, "Failed for " + page);
                IWebElement bottomRight = await driver.FindElement(By.Id("bottom-right"));
                Assert.That(await bottomRight.Displayed(), Is.False, "Failed for " + page);
            }
        }

        [Test]
        public async Task ElementScrollableByOverflowXIsVisible()
        {
            string[] pages = new string[]{
                "overflow/x_scroll_y_hidden.html",
                "overflow/x_scroll_y_scroll.html",
                "overflow/x_scroll_y_auto.html",
                "overflow/x_auto_y_hidden.html",
                "overflow/x_auto_y_scroll.html",
                "overflow/x_auto_y_auto.html",
            };
            foreach (string page in pages) {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs(page));
                IWebElement right = await driver.FindElement(By.Id("right"));
                Assert.That(await right.Displayed(), Is.True, "Failed for " + page);
            }
        }

        [Test]
        public async Task ElementScrollableByOverflowYIsVisible()
        {
            string[] pages = new string[]{
                "overflow/x_hidden_y_scroll.html",
                "overflow/x_scroll_y_scroll.html",
                "overflow/x_auto_y_scroll.html",
                "overflow/x_hidden_y_auto.html",
                "overflow/x_scroll_y_auto.html",
                "overflow/x_auto_y_auto.html",
            };
            foreach (string page in pages) {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs(page));
                IWebElement bottom = await driver.FindElement(By.Id("bottom"));
                Assert.That(await bottom.Displayed(), Is.True, "Failed for " + page);
            }
        }

        [Test]
        public async Task ElementScrollableByOverflowXAndYIsVisible()
        {
            string[] pages = new string[]{
                "overflow/x_scroll_y_scroll.html",
                "overflow/x_scroll_y_auto.html",
                "overflow/x_auto_y_scroll.html",
                "overflow/x_auto_y_auto.html",
            };
            foreach (string page in pages) {
                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs(page));
                IWebElement bottomRight = await driver.FindElement(By.Id("bottom-right"));
                Assert.That(await bottomRight.Displayed(), Is.True, "Failed for " + page);
            }
        }

        [Test]
        public async Task TooSmallAWindowWithOverflowHiddenIsNotAProblem()
        {
            IWindow window = driver.Options().Window;
            Size originalSize = await window.GetSize();

            try {
                // Short in the Y dimension
                await window.SetSize(new Size(1024, 500));

                await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("overflow-body.html"));

                IWebElement element = await driver.FindElement(By.Name("resultsFrame"));
                Assert.That(await element.Displayed(), Is.True);
            } finally {
                await window.SetSize(originalSize);
            }
        }

        [Test]
        public async Task ShouldShowElementNotVisibleWithHiddenAttribute()
        {
            string url = EnvironmentManager.Instance.UrlBuilder.WhereIs("hidden.html");
            await driver.GoToUrl(url);
            IWebElement element = await driver.FindElement(By.Id("singleHidden"));
            Assert.That(await element.Displayed(), Is.False);
        }

        [Test]
        public async Task ShouldShowElementNotVisibleWhenParentElementHasHiddenAttribute()
        {
            string url = EnvironmentManager.Instance.UrlBuilder.WhereIs("hidden.html");
            await driver.GoToUrl(url);

            IWebElement element = await driver.FindElement(By.Id("child"));
            Assert.That(await element.Displayed(), Is.False);
        }

        [Test]
        public async Task ShouldBeAbleToClickOnElementsWithOpacityZero()
        {
            await driver.GoToUrl(clickJackerPage);
            IWebElement element = await driver.FindElement(By.Id("clickJacker"));
            Assert.AreEqual("0", await element.GetCssValue("opacity"), "Precondition failed: clickJacker should be transparent");
            await element.Click();
            Assert.AreEqual("1", await element.GetCssValue("opacity"));
        }

        [Test]
        public async Task ShouldBeAbleToSelectOptionsFromAnInvisibleSelect()
        {
            await driver.GoToUrl(formsPage);

            IWebElement select = await driver.FindElement(By.Id("invisi_select"));

            ReadOnlyCollection<IWebElement> options = await select.FindElements(By.TagName("option"));
            IWebElement apples = options[0];
            IWebElement oranges = options[1];

            Assert.That(await apples.Selected(), Is.True, "Apples should be selected");
            Assert.That(await oranges.Selected(), Is.False, "Oranges shoudl be selected");
            await oranges.Click();
            Assert.That(await apples.Selected(), Is.False, "Apples should not be selected");
            Assert.That(await oranges.Selected(), Is.True, "Oranges should be selected");
        }

        [Test]
        public async Task CorrectlyDetectMapElementsAreShown()
        {
            await driver.GoToUrl(mapVisibilityPage);

            IWebElement area = await driver.FindElement(By.Id("mtgt_unnamed_0"));

            bool isShown = await area.Displayed();
            Assert.That(isShown, Is.True, "The element and the enclosing map should be considered shown.");
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldNotBeAbleToSelectAnElementThatIsNotDisplayed()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("untogglable"));
            //Assert.That(async () => await element.Click(), Throws.InstanceOf<ElementNotInteractableException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Click(),
                exception => Assert.AreEqual("ElementNotInteractableException", exception.Error));
        }

        [Test]
        public async Task ElementsWithOpacityZeroShouldNotBeVisible()
        {
            await driver.GoToUrl(clickJackerPage);
            IWebElement element = await driver.FindElement(By.Id("clickJacker"));
            Assert.That(await element.Displayed(), Is.False);
        }
    }
}
