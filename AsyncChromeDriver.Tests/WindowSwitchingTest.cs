using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    [Explicit("NotImplemented")]
    public class WindowSwitchingTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldSwitchFocusToANewWindowWhenItIsOpenedAndNotStopFutureOperations()
        {
            await driver.GoToUrl(xhtmlTestPage);
            String current = await driver.CurrentWindowHandle();

            await driver.FindElement(By.LinkText("Open new window")).Click();
            Assert.AreEqual("XHTML Test Page", await driver.Title());

            await WaitFor(WindowCountToBe(2), "Window count was not 2");
            await WaitFor(WindowWithName("result"), "Could not find window with name 'result'");
            await WaitFor(async () => { return await driver.Title() == "We Arrive Here"; }, "Browser title was not 'We Arrive Here'");
            Assert.AreEqual("We Arrive Here", await driver.Title());

            await driver.GoToUrl(iframesPage);
            string handle = await driver.CurrentWindowHandle();
            await driver.FindElement(By.Id("iframe_page_heading"));
            await driver.SwitchTo().Frame("iframe1");
            Assert.AreEqual(await driver.CurrentWindowHandle(), handle);
            await driver.SwitchTo().DefaultContent();
            await driver.Close();

            await driver.SwitchTo().Window(current);
            //Assert.AreEqual("XHTML Test Page", driver.Title);
        }

        [Test]
        public async Task ShouldThrowNoSuchWindowException()
        {
            await driver.GoToUrl(xhtmlTestPage);
            String current = await driver.CurrentWindowHandle();
            try {
                await driver.SwitchTo().Window("invalid name");
            } catch (NoSuchWindowException) {
                // This is expected.
            }

            await driver.SwitchTo().Window(current);
        }

        [Test]
        public async Task ShouldThrowNoSuchWindowExceptionOnAnAttemptToGetItsHandle()
        {
            await driver.GoToUrl((xhtmlTestPage));
            String current = await driver.CurrentWindowHandle();
            var currentWindowHandles = await driver.WindowHandles().Count();

            await driver.FindElement(By.LinkText("Open new window")).Click();

            await WaitFor(WindowCountToBe(2), "Window count was not 2");
            Assert.AreEqual(2, await driver.WindowHandles().Count());

            await WaitFor(WindowWithName("result"), "Could not find window with name 'result'");
            await driver.SwitchTo().Window("result");
            await driver.Close();

            try {
                string currentHandle = await driver.CurrentWindowHandle();
                Assert.Fail("NoSuchWindowException expected");
            } catch (NoSuchWindowException) {
                // Expected.
            } finally {
                await driver.SwitchTo().Window(current);
            }
        }

        [Test]
        public async Task ShouldThrowNoSuchWindowExceptionOnAnyOperationIfAWindowIsClosed()
        {
            await driver.GoToUrl((xhtmlTestPage));
            String current = await driver.CurrentWindowHandle();
            int currentWindowHandles = await driver.WindowHandles().Count();

            await driver.FindElement(By.LinkText("Open new window")).Click();

            await WaitFor(WindowCountToBe(2), "Window count was not 2");
            Assert.AreEqual(2, await driver.WindowHandles().Count());

            await WaitFor(WindowWithName("result"), "Could not find window with name 'result'");
            await driver.SwitchTo().Window("result");
            await driver.Close();

            try {
                try {
                    string title = await driver.Title();
                    Assert.Fail("NoSuchWindowException expected");
                } catch (NoSuchWindowException) {
                    // Expected.
                }

                try {
                    await driver.FindElement(By.TagName("body"));
                    Assert.Fail("NoSuchWindowException expected");
                } catch (NoSuchWindowException) {
                    // Expected.
                }
            } finally {
                await driver.SwitchTo().Window(current);
            }
        }

        [Test]
        public async Task ShouldThrowNoSuchWindowExceptionOnAnyElementOperationIfAWindowIsClosed()
        {
            await driver.GoToUrl((xhtmlTestPage));
            String current = await driver.CurrentWindowHandle();
            int currentWindowHandles = await driver.WindowHandles().Count();

            await driver.FindElement(By.LinkText("Open new window")).Click();

            await WaitFor(WindowCountToBe(2), "Window count was not 2");
            Assert.AreEqual(2, await driver.WindowHandles().Count());

            await WaitFor(WindowWithName("result"), "Could not find window with name 'result'");
            await driver.SwitchTo().Window("result");
            IWebElement body = await driver.FindElement(By.TagName("body"));
            await driver.Close();

            try {
                string bodyText = await body.Text();
                Assert.Fail("NoSuchWindowException expected");
            } catch (NoSuchWindowException) {
                // Expected.
            } finally {
                await driver.SwitchTo().Window(current);
            }
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true, IsCreatedAfterTest = true)]
        public async Task ShouldBeAbleToIterateOverAllOpenWindows()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.FindElement(By.Name("windowOne")).Click();
            await WaitFor(WindowCountToBe(2), "Window count was not 2");
            await driver.FindElement(By.Name("windowTwo")).Click();
            await WaitFor(WindowCountToBe(3), "Window count was not 3");

            var allWindowHandles = await driver.WindowHandles();

            // There should be three windows. We should also see each of the window titles at least once.
            List<string> seenHandles = new List<string>();
            foreach (string handle in allWindowHandles) {
                Assert.That(seenHandles, Has.No.Member(handle));
                await driver.SwitchTo().Window(handle);
                seenHandles.Add(handle);
            }

            Assert.AreEqual(3, allWindowHandles.Count);
        }

        [Test]
        public async Task ClickingOnAButtonThatClosesAnOpenWindowDoesNotCauseTheBrowserToHang()
        {
            await driver.GoToUrl(xhtmlTestPage);

            String currentHandle = await driver.CurrentWindowHandle();

            await driver.FindElement(By.Name("windowThree")).Click();

            await driver.SwitchTo().Window("result");

            try {
                IWebElement closeElement = await WaitFor(() => driver.FindElement(By.Id("close")), "Could not find element with id 'close'");
                await closeElement.Click();
                // If we make it this far, we're all good.
            } finally {
                await driver.SwitchTo().Window(currentHandle);
                await driver.FindElement(By.Id("linkId"));
            }
        }

        [Test]
        public async Task CanCallGetWindowHandlesAfterClosingAWindow()
        {
            await driver.GoToUrl(xhtmlTestPage);

            String currentHandle = await driver.CurrentWindowHandle();

            await driver.FindElement(By.Name("windowThree")).Click();

            await driver.SwitchTo().Window("result");

            try {
                IWebElement closeElement = await WaitFor(() => driver.FindElement(By.Id("close")), "Could not find element with id 'close'");
                await closeElement.Click();
                var handles = await driver.WindowHandles();
                // If we make it this far, we're all good.
            } finally {
                await driver.SwitchTo().Window(currentHandle);
            }
        }

        [Test]
        public async Task CanObtainAWindowHandle()
        {
            await driver.GoToUrl(xhtmlTestPage);

            String currentHandle = await driver.CurrentWindowHandle();

            Assert.That(currentHandle, Is.Not.Null);
        }

        [Test]
        public async Task FailingToSwitchToAWindowLeavesTheCurrentWindowAsIs()
        {
            await driver.GoToUrl(xhtmlTestPage);
            String current = await driver.CurrentWindowHandle();

            try {
                await driver.SwitchTo().Window("i will never exist");
                Assert.Fail("Should not be ablt to change to a non-existant window");
            } catch (NoSuchWindowException) {
                // expected
            }

            String newHandle = await driver.CurrentWindowHandle();

            Assert.AreEqual(current, newHandle);
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true, IsCreatedAfterTest = true)]
        public async Task CanCloseWindowWhenMultipleWindowsAreOpen()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.FindElement(By.Name("windowOne")).Click();

            await WaitFor(WindowCountToBe(2), "Window count was not 2");

            var allWindowHandles = await driver.WindowHandles();

            // There should be two windows. We should also see each of the window titles at least once.
            Assert.AreEqual(2, allWindowHandles.Count);
            string handle1 = allWindowHandles[1];
            await driver.SwitchTo().Window(handle1);
            await driver.Close();

            await WaitFor(WindowCountToBe(1), "Window count was not 1");

            allWindowHandles = await driver.WindowHandles();
            Assert.AreEqual(1, allWindowHandles.Count);
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true, IsCreatedAfterTest = true)]
        public async Task CanCloseWindowAndSwitchBackToMainWindow()
        {
            await driver.GoToUrl(xhtmlTestPage);

            var currentWindowHandles = await driver.WindowHandles();
            string mainHandle = await driver.CurrentWindowHandle();

            await driver.FindElement(By.Name("windowOne")).Click();

            await WaitFor(WindowCountToBe(2), "Window count was not 2");

            var allWindowHandles = await driver.WindowHandles();

            // There should be two windows. We should also see each of the window titles at least once.
            Assert.AreEqual(2, allWindowHandles.Count);

            foreach (string handle in allWindowHandles) {
                if (handle != mainHandle) {
                    await driver.SwitchTo().Window(handle);
                    await driver.Close();
                }
            }

            await driver.SwitchTo().Window(mainHandle);

            string newHandle = await driver.CurrentWindowHandle();
            Assert.AreEqual(mainHandle, newHandle);

            Assert.AreEqual(1, await driver.WindowHandles().Count());
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true, IsCreatedAfterTest = true)]
        public async Task ClosingOnlyWindowShouldNotCauseTheBrowserToHang()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.Close();
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true, IsCreatedAfterTest = true)]
        public async Task ShouldFocusOnTheTopMostFrameAfterSwitchingToAWindow()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("window_switching_tests/page_with_frame.html"));

            var currentWindowHandles = await driver.WindowHandles();
            string mainWindow = await driver.CurrentWindowHandle();

            await driver.FindElement(By.Id("a-link-that-opens-a-new-window")).Click();
            await WaitFor(WindowCountToBe(2), "Window count was not 2");

            await driver.SwitchTo().Frame("myframe");

            await driver.SwitchTo().Window("newWindow");
            await driver.Close();
            await driver.SwitchTo().Window(mainWindow);

            await driver.FindElement(By.Name("myframe"));
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldGetBrowserHandles()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.FindElement(By.LinkText("Open new window")).Click();

            await WaitFor(WindowCountToBe(2), "Window count was not 2");

            string handle1, handle2;
            handle1 = await driver.CurrentWindowHandle();

            System.Threading.Thread.Sleep(1000);
            await driver.SwitchTo().Window("result");
            handle2 = await driver.CurrentWindowHandle();

            var handles = await driver.WindowHandles();

            // At least the two handles we want should be there.
            Assert.Contains(handle1, handles, "Should have contained current handle");
            Assert.Contains(handle2, handles, "Should have contained result handle");

            // Some (semi-)clean up..
            await driver.SwitchTo().Window(handle2);
            await driver.Close();
            await driver.SwitchTo().Window(handle1);
            await driver.GoToUrl(macbethPage);
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task CloseShouldCloseCurrentHandleOnly()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.FindElement(By.LinkText("Open new window")).Click();

            await WaitFor(WindowCountToBe(2), "Window count was not 2");

            string handle1, handle2;
            handle1 = await driver.CurrentWindowHandle();

            await driver.SwitchTo().Window("result");
            handle2 = await driver.CurrentWindowHandle();
            await driver.Close();
            SleepBecauseWindowsTakeTimeToOpen();

            var handles = await driver.WindowHandles();

            Assert.That(handles, Has.No.Member(handle2), "Invalid handle still in handle list");
            Assert.That(handles, Contains.Item(handle1), "Valid handle not in handle list");
        }

        private void SleepBecauseWindowsTakeTimeToOpen()
        {
            try {
                System.Threading.Thread.Sleep(1000);
            } catch (Exception) {
                Assert.Fail("Interrupted");
            }
        }

        private Func<Task<bool>> WindowCountToBe(int desiredCount)
        {
            return async () => await driver.WindowHandles().Count() == desiredCount;
        }

        private Func<Task<bool>> WindowWithName(string name)
        {
            return async () => {
                try {
                    await driver.SwitchTo().Window(name);
                    return true;
                } catch (NoSuchWindowException) {
                }

                return false;
            };
        }

        private Func<Task<IAlert>> AlertToBePresent()
        {
            return async () => {
                IAlert alert = null;
                try {
                    alert = await driver.SwitchTo().Alert();
                } catch (NoAlertPresentException) {
                }

                return alert;
            };
        }
    }
}
