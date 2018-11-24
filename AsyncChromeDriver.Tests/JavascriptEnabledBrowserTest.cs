using System;
using NUnit.Framework;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class JavascriptEnabledBrowserTest : DriverTestFixture
    {
        [Test]
        public async Task DocumentShouldReflectLatestTitle()
        {
            await driver.GoToUrl(javascriptPage);

            Assert.AreEqual("Testing Javascript", await driver.Title());
            await driver.FindElement(By.LinkText("Change the page title!")).Click();
            Assert.AreEqual("Changed", await driver.Title());
        }

        [Test]
        public async Task DocumentShouldReflectLatestDom()
        {
            await driver.GoToUrl(javascriptPage);
            String currentText = await driver.FindElement(By.XPath("//div[@id='dynamo']")).Text();
            Assert.AreEqual("What's for dinner?", currentText);

            IWebElement element = await driver.FindElement(By.LinkText("Update a div"));
            await element.Click();

            String newText = await driver.FindElement(By.XPath("//div[@id='dynamo']")).Text();
            Assert.AreEqual("Fish and chips!", newText);
        }

        [Test]
        public async Task ShouldWaitForLoadsToCompleteAfterJavascriptCausesANewPageToLoad()
        {
            await driver.GoToUrl(formsPage);

            await driver.FindElement(By.Id("changeme")).Click();
            await WaitFor(driver.Title(), "Page3", "Browser title was not 'Page3'");
            Assert.AreEqual("Page3", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToFindElementAfterJavascriptCausesANewPageToLoad()
        {
            await driver.GoToUrl(formsPage);

            await driver.FindElement(By.Id("changeme")).Click();

            await WaitFor(driver.Title(), "Page3", "Browser title was not 'Page3'");
            Assert.AreEqual("3", await driver.FindElement(By.Id("pageNumber")).Text());
        }

        [Test]
        public async Task ShouldFireOnChangeEventWhenSettingAnElementsValue()
        {
            await driver.GoToUrl(javascriptPage);
            await driver.FindElement(By.Id("change")).SendKeys("foo");
            String result = await driver.FindElement(By.Id("result")).Text();

            Assert.AreEqual("change", result);
        }

        [Test]
        public async Task ShouldBeAbleToSubmitFormsByCausingTheOnClickEventToFire()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("jsSubmitButton"));
            await element.Click();

            await WaitFor(driver.Title(), "We Arrive Here", "Browser title was not 'We Arrive Here'");
            Assert.AreEqual("We Arrive Here", await driver.Title());
        }

        [Test]
        public async Task ShouldBeAbleToClickOnSubmitButtons()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("submittingButton"));
            await element.Click();

            await WaitFor(driver.Title(), "We Arrive Here", "Browser title was not 'We Arrive Here'");
            Assert.AreEqual("We Arrive Here", await driver.Title());
        }

        [Test]
        public async Task Issue80ClickShouldGenerateClickEvent()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.Id("clickField"));
            Assert.AreEqual("Hello", element.GetAttribute("value"));

            await element.Click();

            Assert.AreEqual("Clicked", element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToSwitchToFocusedElement()
        {
            await driver.GoToUrl(javascriptPage);

            await driver.FindElement(By.Id("switchFocus")).Click();

            IWebElement element = await driver.SwitchTo().ActiveElement();
            Assert.AreEqual("theworks", element.GetAttribute("id"));
        }

        [Test]
        public async Task IfNoElementHasFocusTheActiveElementIsTheBody()
        {
            await driver.GoToUrl(simpleTestPage);

            IWebElement element = await driver.SwitchTo().ActiveElement();

            Assert.AreEqual("body", element.GetAttribute("name"));
        }

        [Test]
        public async Task ChangeEventIsFiredAppropriatelyWhenFocusIsLost()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement input = await driver.FindElement(By.Id("changeable"));
            await input.SendKeys("test");
            await driver.FindElement(By.Id("clickField")).Click(); // move focus
            EqualConstraint firstConstraint = new EqualConstraint("focus change blur");
            EqualConstraint secondConstraint = new EqualConstraint("focus change blur");


            Assert.That((await driver.FindElement(By.Id("result")).Text()).Trim(), firstConstraint | secondConstraint);

            await input.SendKeys(Keys.Backspace + "t");
            await driver.FindElement(By.Id("clickField")).Click();  // move focus

            firstConstraint = new EqualConstraint("focus change blur focus blur");
            secondConstraint = new EqualConstraint("focus blur change focus blur");
            EqualConstraint thirdConstraint = new EqualConstraint("focus blur change focus blur change");
            EqualConstraint fourthConstraint = new EqualConstraint("focus change blur focus change blur"); //What Chrome does
            // I weep.
            Assert.That((await driver.FindElement(By.Id("result")).Text()).Trim(),
                       firstConstraint | secondConstraint | thirdConstraint | fourthConstraint);
        }

        /**
         * If the click handler throws an exception, the firefox driver freezes. This is suboptimal.
         */
        [Test]
        public async Task ShouldBeAbleToClickIfEvenSomethingHorribleHappens()
        {
            await driver.GoToUrl(javascriptPage);

            await driver.FindElement(By.Id("error")).Click();

            // If we get this far then the test has passed, but let's do something basic to prove the point
            String text = await driver.FindElement(By.Id("error")).Text();

            Assert.That(text, Is.Not.Null);
        }

        [Test]
        public async Task ShouldBeAbleToGetTheLocationOfAnElement()
        {
            await driver.GoToUrl(javascriptPage);

            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await ((IJavaScriptExecutor)driver).ExecuteScript("window.focus();");
            IWebElement element = await driver.FindElement(By.Id("keyUp"));

            if (!(element is ILocatable)) {
                return;
            }

            WebPoint point = await ((ILocatable)element).LocationOnScreenOnceScrolledIntoView();

            Assert.That(point.X, Is.GreaterThan(1));
            Assert.That(point.Y, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldBeAbleToClickALinkThatClosesAWindow()
        {
            await driver.GoToUrl(javascriptPage);

            String handle = await driver.CurrentWindowHandle();
            await driver.FindElement(By.Id("new_window")).Click();
            await WaitFor(async () => { await driver.SwitchTo().Window("close_me"); return true; }, "Could not find window with name 'close_me'");

            IWebElement closeElement = await WaitFor<IWebElement>(() => {
                try {
                    return driver.FindElement(By.Id("close"));
                } catch (NoSuchElementException) {
                    return null;
                }
            }, "No element to close window found");
            await closeElement.Click();

            await driver.SwitchTo().Window(handle);
        }
    }
}
