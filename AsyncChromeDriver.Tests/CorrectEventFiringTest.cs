using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class CorrectEventFiringTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldFireFocusEventWhenClicking()
        {
            await driver.GoToUrl(javascriptPage);

            await ClickOnElementWhichRecordsEvents(driver);

            await AssertEventFired("focus", driver);
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true, IsCreatedAfterTest = true)]
        public async Task ShouldFireFocusEventInNonTopmostWindow()
        {
            IWebDriver driver2 = EnvironmentManager.Instance.CreateDriverInstance();
            try {
                // topmost
                await driver2.GoToUrl(javascriptPage);
                await ClickOnElementWhichRecordsEvents(driver2);
                await AssertEventFired("focus", driver2);

                // non-topmost
                await driver.GoToUrl(javascriptPage);
                await ClickOnElementWhichRecordsEvents(driver);
                await AssertEventFired("focus", driver);

            } finally {
                //await driver.GoToUrl(driver2.Quit());
            }
        }

        [Test]
        public async Task ShouldFireClickEventWhenClicking()
        {
            await driver.GoToUrl(javascriptPage);

            await ClickOnElementWhichRecordsEvents(driver);

            await AssertEventFired("click", driver);
        }

        [Test]
        public async Task ShouldFireMouseDownEventWhenClicking()
        {
            await driver.GoToUrl(javascriptPage);

            await ClickOnElementWhichRecordsEvents(driver);

            await AssertEventFired("mousedown", driver);
        }

        [Test]
        public async Task ShouldFireMouseUpEventWhenClicking()
        {
            await driver.GoToUrl(javascriptPage);

            await ClickOnElementWhichRecordsEvents(driver);

            await AssertEventFired("mouseup", driver);
        }

        [Test]
        public async Task ShouldFireMouseOverEventWhenClicking()
        {
            await driver.GoToUrl(javascriptPage);

            await ClickOnElementWhichRecordsEvents(driver);

            await AssertEventFired("mouseover", driver);
        }

        //[Test]
        //public async Task ShouldFireMouseMoveEventWhenClicking()
        //{
        //   await driver.GoToUrl(javascriptPage);

        //    // This bears some explanation. In certain cases, if the prior test
        //    // leaves the mouse cursor immediately over the wrong element, then
        //    // the mousemove event may not get fired, because the mouse does not
        //    // actually move. Prevent this situation by forcing the mouse to move
        //    // to the origin.
        //    new Actions(driver).MoveToElement(await driver.FindElement(By.TagName("body"))).Perform();

        //    await ClickOnElementWhichRecordsEvents(driver);

        //    await AssertEventFired("mousemove", driver);
        //}

        [Test]
        public async Task ShouldNotThrowIfEventHandlerThrows()
        {
            await driver.GoToUrl(javascriptPage);
            await driver.FindElement(By.Id("throwing-mouseover")).Click();
        }

        [Test]
        public async Task ShouldFireEventsInTheRightOrder()
        {
            await driver.GoToUrl(javascriptPage);

            await ClickOnElementWhichRecordsEvents(driver);

            string text = await driver.FindElement(By.Id("result")).Text();

            int lastIndex = -1;
            List<string> eventList = new List<string>() { "mousedown", "focus", "mouseup", "click" };
            foreach (string eventName in eventList) {
                int index = text.IndexOf(eventName);

                Assert.That(text, Does.Contain(eventName), eventName + " did not fire at all. Text is " + text);
                Assert.That(index, Is.GreaterThan(lastIndex), eventName + " did not fire in the correct order. Text is " + text);
                lastIndex = index;
            }
        }

        [Test]
        public async Task ShouldIssueMouseDownEvents()
        {
            await driver.GoToUrl(javascriptPage);
            await driver.FindElement(By.Id("mousedown")).Click();

            String result = await driver.FindElement(By.Id("result")).Text();
            Assert.AreEqual(result, "mouse down");
        }

        [Test]
        public async Task ShouldIssueClickEvents()
        {
            await driver.GoToUrl(javascriptPage);
            await driver.FindElement(By.Id("mouseclick")).Click();

            String result = await driver.FindElement(By.Id("result")).Text();
            Assert.AreEqual(result, "mouse click");
        }

        [Test]
        public async Task ShouldIssueMouseUpEvents()
        {
            await driver.GoToUrl(javascriptPage);
            await driver.FindElement(By.Id("mouseup")).Click();

            String result = await driver.FindElement(By.Id("result")).Text();
            Assert.AreEqual(result, "mouse up");
        }

        [Test]
        public async Task MouseEventsShouldBubbleUpToContainingElements()
        {
            await driver.GoToUrl(javascriptPage);
            await driver.FindElement(By.Id("child")).Click();

            String result = await driver.FindElement(By.Id("result")).Text();
            Assert.AreEqual(result, "mouse down");
        }

        [Test]
        public async Task ShouldEmitOnChangeEventsWhenSelectingElements()
        {
            await driver.GoToUrl(javascriptPage);
            //Intentionally not looking up the select tag.  See selenium r7937 for details.
            ReadOnlyCollection<IWebElement> allOptions = await driver.FindElements(By.XPath("//select[@id='selector']//option"));

            String initialTextValue = await driver.FindElement(By.Id("result")).Text();

            IWebElement foo = allOptions[0];
            IWebElement bar = allOptions[1];

            await foo.Click();
            Assert.AreEqual(await driver.FindElement(By.Id("result")).Text(), initialTextValue);
            await bar.Click();
            Assert.AreEqual(await driver.FindElement(By.Id("result")).Text(), "bar");
        }

        [Test]
        public async Task ShouldEmitOnClickEventsWhenSelectingElements()
        {
            await driver.GoToUrl(javascriptPage);
            // Intentionally not looking up the select tag. See selenium r7937 for details.
            ReadOnlyCollection<IWebElement> allOptions = await driver.FindElements(By.XPath("//select[@id='selector2']//option"));

            IWebElement foo = allOptions[0];
            IWebElement bar = allOptions[1];

            await foo.Click();
            Assert.AreEqual(await driver.FindElement(By.Id("result")).Text(), "foo");
            await bar.Click();
            Assert.AreEqual(await driver.FindElement(By.Id("result")).Text(), "bar");
        }

        [Test]
        public async Task ShouldEmitOnChangeEventsWhenChangingTheStateOfACheckbox()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement checkbox = await driver.FindElement(By.Id("checkbox"));

            await checkbox.Click();
            Assert.AreEqual(await driver.FindElement(By.Id("result")).Text(), "checkbox thing");
        }

        [Test]
        public async Task ShouldEmitClickEventWhenClickingOnATextInputElement()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement clicker = await driver.FindElement(By.Id("clickField"));
            await clicker.Click();

            Assert.AreEqual(await clicker.GetAttribute("value"), "Clicked");
        }

        //[Test]
        //public async Task ShouldFireTwoClickEventsWhenClickingOnALabel()
        //{
        //   await driver.GoToUrl(javascriptPage;

        //    await driver.FindElement(By.Id("labelForCheckbox")).Click();

        //    IWebElement result = await driver.FindElement(By.Id("result"));
        //    Assert.That(await WaitFor(async () => { return (await result.Text()).Contains("labelclick chboxclick"); }, "Did not find text: " + (await result.Text()), Is.True));
        //}


        [Test]
        public async Task ClearingAnElementShouldCauseTheOnChangeHandlerToFire()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("clearMe"));
            await element.Clear();

            IWebElement result = await driver.FindElement(By.Id("result"));
            Assert.AreEqual(await result.Text(), "Cleared");
        }

        [Test]
        public async Task SendingKeysToAnotherElementShouldCauseTheBlurEventToFire()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("theworks"));
            await element.SendKeys("foo");
            IWebElement element2 = await driver.FindElement(By.Id("changeable"));
            await element2.SendKeys("bar");
            await AssertEventFired("blur", driver);
        }

        [Test]
        public async Task SendingKeysToAnotherElementShouldCauseTheBlurEventToFireInNonTopmostWindow()
        {
            IWebElement element = null;
            IWebElement element2 = null;
            IWebDriver driver2 = EnvironmentManager.Instance.CreateDriverInstance();
            try {
                // topmost
                await driver2.GoToUrl(javascriptPage);
                element = await driver2.FindElement(By.Id("theworks"));
                await element.SendKeys("foo");
                element2 = await driver2.FindElement(By.Id("changeable"));
                await element2.SendKeys("bar");
                await AssertEventFired("blur", driver2);

                // non-topmost
                await driver.GoToUrl(javascriptPage);
                element = await driver.FindElement(By.Id("theworks"));
                await element.SendKeys("foo");
                element2 = await driver.FindElement(By.Id("changeable"));
                await element2.SendKeys("bar");
                await AssertEventFired("blur", driver);
            } finally {
                await driver2.Quit();
            }

            await driver.GoToUrl(javascriptPage);
            element = await driver.FindElement(By.Id("theworks"));
            await element.SendKeys("foo");
            element2 = await driver.FindElement(By.Id("changeable"));
            await element2.SendKeys("bar");
            await AssertEventFired("blur", driver);
        }

        [Test]
        public async Task SendingKeysToAnElementShouldCauseTheFocusEventToFire()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("theworks"));
            await element.SendKeys("foo");
            await AssertEventFired("focus", driver);
        }

        [Test]
        public async Task SendingKeysToAFocusedElementShouldNotBlurThatElement()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("theworks"));
            await element.Click();

            //Wait until focused
            bool focused = false;
            IWebElement result = await driver.FindElement(By.Id("result"));
            for (int i = 0; i < 5; ++i) {
                string fired = await result.Text();
                if (fired.Contains("focus")) {
                    focused = true;
                    break;
                }
                try {
                    System.Threading.Thread.Sleep(200);
                } catch (Exception e) {
                    throw e;
                }
            }
            if (!focused) {
                Assert.Fail("Clicking on element didn't focus it in time - can't proceed so failing");
            }

            await element.SendKeys("a");
            await AssertEventNotFired("blur");
        }

        [Test]
        public async Task ClickingAnUnfocusableChildShouldNotBlurTheParent()
        {

            await driver.GoToUrl(javascriptPage);
            // Click on parent, giving it the focus.
            IWebElement parent = await driver.FindElement(By.Id("hideOnBlur"));
            await parent.Click();
            await AssertEventNotFired("blur");
            // Click on child. It is not focusable, so focus should stay on the parent.
            await driver.FindElement(By.Id("hideOnBlurChild")).Click();
            System.Threading.Thread.Sleep(2000);
            Assert.That(await parent.Displayed(), Is.True, "#hideOnBlur should still be displayed after click");
            await AssertEventNotFired("blur");
            // Click elsewhere, and let the element disappear.
            await driver.FindElement(By.Id("result")).Click();
            await AssertEventFired("blur", driver);
        }

        [Test]
        public async Task SubmittingFormFromFormElementShouldFireOnSubmitForThatForm()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement formElement = await driver.FindElement(By.Id("submitListeningForm"));
            await formElement.Submit();
            await AssertEventFired("form-onsubmit", driver);
        }

        [Test]
        public async Task SubmittingFormFromFormInputSubmitElementShouldFireOnSubmitForThatForm()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement submit = await driver.FindElement(By.Id("submitListeningForm-submit"));
            await submit.Submit();
            await AssertEventFired("form-onsubmit", driver);
        }

        [Test]
        public async Task SubmittingFormFromFormInputTextElementShouldFireOnSubmitForThatFormAndNotClickOnThatInput()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement submit = await driver.FindElement(By.Id("submitListeningForm-submit"));
            await submit.Submit();
            await AssertEventFired("form-onsubmit", driver);
            await AssertEventNotFired("text-onclick");
        }

        [Test]
        public async Task UploadingFileShouldFireOnChangeEvent()
        {
            await driver.GoToUrl(formsPage);
            IWebElement uploadElement = await driver.FindElement(By.Id("upload"));
            IWebElement result = await driver.FindElement(By.Id("fileResults"));
            Assert.AreEqual(string.Empty, await result.Text());

            string filePath = System.IO.Path.Combine(EnvironmentManager.Instance.CurrentDirectory, "test.txt");
            System.IO.FileInfo inputFile = new System.IO.FileInfo(filePath);
            System.IO.StreamWriter inputFileWriter = inputFile.CreateText();
            inputFileWriter.WriteLine("Hello world");
            inputFileWriter.Close();

            await uploadElement.SendKeys(inputFile.FullName);
            // Shift focus to something else because send key doesn't make the focus leave
            await driver.FindElement(By.Id("id-name1")).Click();

            inputFile.Delete();
            Assert.AreEqual("changed", await result.Text());
        }

        //[Test]
        //public async Task ShouldReportTheXAndYCoordinatesWhenClicking()
        //{
        //   await driver.GoToUrl(clickEventPage;

        //    IWebElement element = await driver.FindElement(By.Id("eventish"));
        //    await element.Click();

        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        //    string clientX = await driver.FindElement(By.Id("clientX")).Text;
        //    string clientY = await driver.FindElement(By.Id("clientY")).Text;
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

        //    Assert.AreNotEqual("0", clientX);
        //    Assert.AreNotEqual("0", clientY);
        //}

        [Test]
        public async Task ClickEventsShouldBubble()
        {
            await driver.GoToUrl(clicksPage);
            await driver.FindElement(By.Id("bubblesFrom")).Click();
            bool eventBubbled = (bool)await ((IJavaScriptExecutor)driver).ExecuteScript("return !!window.bubbledClick;");
            Assert.That(eventBubbled, Is.True, "Event didn't bubble up");
        }

        [Test]
        public async Task ClickOverlappingElements()
        {

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/overlapping_elements.html"));
            Assert.That(async () => await driver.FindElement(By.Id("under")).Click(), Throws.InstanceOf<ElementClickInterceptedException>().Or.InstanceOf<WebDriverException>().With.Message.Contains("Other element would receive the click"));
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("under")).Click());
        }


        [Test]
        public async Task ClickAnElementThatDisappear()
        {

            StringBuilder expectedLogBuilder = new StringBuilder();
            expectedLogBuilder.AppendLine("Log:");
            expectedLogBuilder.AppendLine("mousedown in over (handled by over)");
            expectedLogBuilder.AppendLine("mousedown in over (handled by body)");
            expectedLogBuilder.AppendLine("mouseup in under (handled by under)");
            expectedLogBuilder.Append("mouseup in under (handled by body)");

            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/disappearing_element.html"));
            await driver.FindElement(By.Id("over")).Click();
            Assert.That((await driver.FindElement(By.Id("log")).Text()).StartsWith(expectedLogBuilder.ToString()));
        }

        private async Task AssertEventNotFired(string eventName)
        {
            IWebElement result = await driver.FindElement(By.Id("result"));
            string text = await result.Text();
            Assert.That(text, Does.Not.Contain(eventName));
        }

        private async Task ClickOnElementWhichRecordsEvents(IWebDriver focusedDriver)
        {
            await focusedDriver.FindElement(By.Id("plainButton")).Click();
        }

        private async Task AssertEventFired(string eventName, IWebDriver focusedDriver)
        {
            IWebElement result = await focusedDriver.FindElement(By.Id("result"));
            string text = await result.Text();
            Assert.That(text, Does.Contain(eventName));
        }
    }
}
