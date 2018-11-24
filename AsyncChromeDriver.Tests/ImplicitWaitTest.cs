using System;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ImplicitWaitTest : DriverTestFixture
    {
        [TearDown]
        public async Task ResetImplicitWaitTimeout()
        {
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(0));
        }

        [Test]
        public async Task ShouldImplicitlyWaitForASingleElement()
        {
            await driver.GoToUrl(dynamicPage);
            IWebElement add = await driver.FindElement(By.Id("adder"));

            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(3000));
            await add.Click();
            await driver.FindElement(By.Id("box0"));  // All is well if this doesn't throw.
        }

        [Test]
        public async Task ShouldStillFailToFindAnElementWhenImplicitWaitsAreEnabled()
        {
            await driver.GoToUrl(dynamicPage);
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(500));
            //Assert.That(async () => await driver.FindElement(By.Id("box0")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("box0")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        [NeedsFreshDriver]
        public async Task ShouldReturnAfterFirstAttemptToFindOneAfterDisablingImplicitWaits()
        {
            await driver.GoToUrl(dynamicPage);
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(3000));
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(0));
            //Assert.That(async () => await driver.FindElement(By.Id("box0")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("box0")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        [NeedsFreshDriver]
        public async Task ShouldImplicitlyWaitUntilAtLeastOneElementIsFoundWhenSearchingForMany()
        {
            await driver.GoToUrl(dynamicPage);
            IWebElement add = await driver.FindElement(By.Id("adder"));

            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(2000));
            await add.Click();
            await add.Click();

            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.ClassName("redbox"));
            Assert.That(elements, Has.Count.GreaterThanOrEqualTo(1));
        }

        [Test]
        [NeedsFreshDriver]
        public async Task ShouldStillFailToFindElementsWhenImplicitWaitsAreEnabled()
        {
            await driver.GoToUrl(dynamicPage);

            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(500));
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.ClassName("redbox"));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        [NeedsFreshDriver]
        public async Task ShouldReturnAfterFirstAttemptToFindManyAfterDisablingImplicitWaits()
        {
            await driver.GoToUrl(dynamicPage);
            IWebElement add = await driver.FindElement(By.Id("adder"));
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(1100));
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(0));
            await add.Click();
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.ClassName("redbox"));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public async Task ShouldImplicitlyWaitForAnElementToBeVisibleBeforeInteracting()
        {
            await driver.GoToUrl(dynamicPage);

            IWebElement reveal = await driver.FindElement(By.Id("reveal"));
            IWebElement revealed = await driver.FindElement(By.Id("revealed"));
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromMilliseconds(5000));

            Assert.That(await revealed.Displayed(), Is.False, "revealed should not be visible");
            await reveal.Click();

            try {
                await revealed.SendKeys("hello world");
                // This is what we want
            } catch (ElementNotVisibleException) {
                Assert.Fail("Element should have been visible");
            }
        }

        [Test]
        public async Task ShouldRetainImplicitlyWaitFromTheReturnedWebDriverOfWindowSwitchTo()
        {
            await driver.Options().Timeouts.SetImplicitWait(TimeSpan.FromSeconds(1));
            await driver.GoToUrl(xhtmlTestPage);
            await driver.FindElement(By.Name("windowOne")).Click();

            string originalHandle = await driver.CurrentWindowHandle();
            await WaitFor(async () => await driver.WindowHandles().Count() == 2, "Window handle count was not 2");
            List<string> handles = new List<string>(await driver.WindowHandles());
            handles.Remove(originalHandle);

            IWebDriver newWindow = await driver.SwitchTo().Window(handles[0]);

            DateTime start = DateTime.Now;
            await newWindow.FindElements(By.Id("this-crazy-thing-does-not-exist"));
            DateTime end = DateTime.Now;
            TimeSpan time = end - start;
            await driver.Close();
            await driver.SwitchTo().Window(originalHandle);
            Assert.That(time.TotalMilliseconds, Is.GreaterThanOrEqualTo(1000));
        }
    }
}
