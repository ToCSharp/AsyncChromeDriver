using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class AlertsTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldBeAbleToOverrideTheWindowAlertMethod()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));

            await ((IJavaScriptExecutor)driver).ExecuteScript(
                "window.alert = function(msg) { document.getElementById('text').innerHTML = msg; }");
            await driver.FindElement(By.Id("alert")).Click();
        }

        [Test]
        public async Task ShouldAllowUsersToAcceptAnAlertManually()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));

            await driver.FindElement(By.Id("alert")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Accept();

            // If we can perform any action, we're good to go
            Assert.AreEqual("Testing Alerts", await driver.Title());
        }

        [Test]
        public async Task ShouldThrowArgumentNullExceptionWhenKeysNull()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));

            await driver.FindElement(By.Id("alert")).Click();
            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            try {
                //Assert.That(async () => await alert.SendKeys(null), Throws.ArgumentNullException);
                await AssertEx.ThrowsAsync<ArgumentNullException>(async () => await alert.SendKeys(null));
            } finally {
                await alert.Accept();
            }
        }

        [Test]
        public async Task ShouldAllowUsersToAcceptAnAlertWithNoTextManually()
        {
            await driver.GoToUrl(CreateAlertPage(""));

            await driver.FindElement(By.Id("alert")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Accept();

            // If we can perform any action, we're good to go
            Assert.AreEqual("Testing Alerts", await driver.Title());
        }

        [Test]
        public async Task ShouldAllowUsersToDismissAnAlertManually()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));

            await driver.FindElement(By.Id("alert")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Dismiss();

            // If we can perform any action, we're good to go
            Assert.AreEqual("Testing Alerts", await driver.Title());
        }

        [Test]
        public async Task ShouldAllowAUserToAcceptAPrompt()
        {
            await driver.GoToUrl(CreatePromptPage(null));

            await driver.FindElement(By.Id("prompt")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Accept();

            // If we can perform any action, we're good to go
            Assert.AreEqual("Testing Prompt", await driver.Title());
        }

        [Test]
        public async Task ShouldAllowAUserToDismissAPrompt()
        {
            await driver.GoToUrl(CreatePromptPage(null));

            await driver.FindElement(By.Id("prompt")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Dismiss();

            // If we can perform any action, we're good to go
            Assert.AreEqual("Testing Prompt", await driver.Title());
        }

        [Test]
        public async Task ShouldAllowAUserToSetTheValueOfAPrompt()
        {
            await driver.GoToUrl(CreatePromptPage(null));

            await driver.FindElement(By.Id("prompt")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.SendKeys("cheese");
            await alert.Accept();

            string result = await driver.FindElement(By.Id("text")).Text();
            Assert.AreEqual("cheese", result);
        }

        [Test]
        public async Task ShouldAllowTheUserToGetTheTextOfAnAlert()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));

            await driver.FindElement(By.Id("alert")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            string value = await alert.Text();
            await alert.Accept();

            Assert.AreEqual("cheese", value);
        }

        [Test]
        public async Task ShouldAllowTheUserToGetTheTextOfAPrompt()
        {
            await driver.GoToUrl(CreatePromptPage(null));

            await driver.FindElement(By.Id("prompt")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            string value = await alert.Text();
            await alert.Accept();

            Assert.AreEqual("Enter something", value);
        }

        [Test]
        public async Task AlertShouldNotAllowAdditionalCommandsIfDimissed()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));

            await driver.FindElement(By.Id("alert")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Dismiss();
            string text;
            //Assert.That(async () => text = await alert.Text(), Throws.InstanceOf<NoAlertPresentException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => text = await alert.Text(),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldAllowUsersToAcceptAnAlertInAFrame()
        {
            string iframe = EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithBody("<a href='#' id='alertInFrame' onclick='alert(\"framed cheese\");'>click me</a>"));
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithTitle("Testing Alerts")
                .WithBody($"<iframe src='{iframe}' name='iframeWithAlert'></iframe>")));

            await driver.SwitchTo().Frame("iframeWithAlert");

            await driver.FindElement(By.Id("alertInFrame")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Accept();

            // If we can perform any action, we're good to go
            Assert.AreEqual("Testing Alerts", await driver.Title());
        }

        [Test]
        public async Task ShouldAllowUsersToAcceptAnAlertInANestedFrame()
        {
            string iframe = EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithBody("<a href='#' id='alertInFrame' onclick='alert(\"framed cheese\");'>click me</a>"));
            string iframe2 = EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithBody($"<iframe src='{iframe}' name='iframeWithAlert'></iframe>"));
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithTitle("Testing Alerts")
                .WithBody($"<iframe src='{iframe2}' name='iframeWithIframe'></iframe>")));

            await driver.SwitchTo().Frame("iframeWithIframe").SwitchTo().Frame("iframeWithAlert");

            await driver.FindElement(By.Id("alertInFrame")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Accept();

            // If we can perform any action, we're good to go
            Assert.AreEqual("Testing Alerts", await driver.Title());
        }

        [Test]
        public async Task SwitchingToMissingAlertThrows()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));

            //Assert.That(async () => await AlertToBePresent(), Throws.InstanceOf<NoAlertPresentException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await AlertToBePresent(),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task SwitchingToMissingAlertInAClosedWindowThrows()
        {
            string blank = EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage());
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithBody($"<a id='open-new-window' href='{blank}' target='newwindow'>open new window</a>")));

            string mainWindow = await driver.CurrentWindowHandle();
            try {
                await driver.FindElement(By.Id("open-new-window")).Click();
                await WaitFor(WindowHandleCountToBe(2), "Window count was not 2");
                await WaitFor(WindowWithName("newwindow"), "Could not find window with name 'newwindow'");
                await driver.Close();
                await WaitFor(WindowHandleCountToBe(1), "Window count was not 1");

                try {
                   await  (await AlertToBePresent()).Accept();
                    Assert.Fail("Expected exception");
                } catch (NoSuchWindowException) {
                    // Expected
                }

            } finally {
                await driver.SwitchTo().Window(mainWindow);
                await WaitFor(ElementTextToEqual(await driver.FindElement(By.Id("open-new-window")), "open new window"), "Could not find element with text 'open new window'");
            }
        }

        [Test]
        public async Task PromptShouldUseDefaultValueIfNoKeysSent()
        {
            await driver.GoToUrl(CreatePromptPage("This is a default value"));
            await driver.FindElement(By.Id("prompt")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Accept();

            IWebElement element = await driver.FindElement(By.Id("text"));
            await WaitFor(ElementTextToEqual(element, "This is a default value"), "Element text was not 'This is a default value'");
            Assert.AreEqual("This is a default value", await element.Text());
        }

        [Test]
        public async Task PromptShouldHaveNullValueIfDismissed()
        {
            await driver.GoToUrl(CreatePromptPage("This is a default value"));
            await driver.FindElement(By.Id("prompt")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Dismiss();
            IWebElement element = await driver.FindElement(By.Id("text"));
            await WaitFor(ElementTextToEqual(element, "null"), "Element text was not 'null'");
            Assert.AreEqual("null", await element.Text());
        }

        [Test]
        public async Task HandlesTwoAlertsFromOneInteraction()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithScripts(
                    "function setInnerText(id, value) {",
                    "  document.getElementById(id).innerHTML = '<p>' + value + '</p>';",
                    "}",
                    "function displayTwoPrompts() {",
                    "  setInnerText('text1', prompt('First'));",
                    "  setInnerText('text2', prompt('Second'));",
                    "}")
                .WithBody(
                    "<a href='#' id='double-prompt' onclick='displayTwoPrompts();'>click me</a>",
                    "<div id='text1'></div>",
                    "<div id='text2'></div>")));

            await driver.FindElement(By.Id("double-prompt")).Click();

            IAlert alert1 = await WaitFor(AlertToBePresent(), "No alert found");
            await alert1.SendKeys("brie");
            await alert1.Accept();

            IAlert alert2 = await WaitFor(AlertToBePresent(), "No alert found");
            await alert2.SendKeys("cheddar");
            await alert2.Accept();

            IWebElement element1 = await driver.FindElement(By.Id("text1"));
            await WaitFor(ElementTextToEqual(element1, "brie"), "Element text was not 'brie'");
            Assert.AreEqual("brie", await element1.Text());
            IWebElement element2 = await driver.FindElement(By.Id("text2"));
            await WaitFor(ElementTextToEqual(element2, "cheddar"), "Element text was not 'cheddar'");
            Assert.AreEqual("cheddar", await element2.Text());
        }

        [Test]
        public async Task ShouldHandleAlertOnPageLoad()
        {
            string pageWithOnLoad = EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithOnLoad("javascript:alert(\"onload\")")
                .WithBody("<p>Page with onload event handler</p>"));
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithBody(string.Format("<a id='open-page-with-onload-alert' href='{0}'>open new page</a>", pageWithOnLoad))));

            await driver.FindElement(By.Id("open-page-with-onload-alert")).Click();

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            string value = await alert.Text();
            await alert.Accept();

            Assert.AreEqual("onload", value);
            IWebElement element = await driver.FindElement(By.TagName("p"));
            await WaitFor(ElementTextToEqual(element, "Page with onload event handler"), "Element text was not 'Page with onload event handler'");
        }

        [Test]
        public async Task ShouldHandleAlertOnPageLoadUsingGet()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithOnLoad("javascript:alert(\"onload\")")
                .WithBody("<p>Page with onload event handler</p>")));

            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            string value = await alert.Text();
            await alert.Accept();

            Assert.AreEqual("onload", value);
            await WaitFor(ElementTextToEqual(await driver.FindElement(By.TagName("p")), "Page with onload event handler"), "Could not find element with text 'Page with onload event handler'");
        }

        [Test]
        public async Task CanQuitWhenAnAlertIsPresent()
        {
            await driver.GoToUrl(CreateAlertPage("cheese"));
            await driver.FindElement(By.Id("alert")).Click();
            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            EnvironmentManager.Instance.CloseCurrentDriver();
        }

        [Test]
        public async Task ShouldHandleAlertOnFormSubmit()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithTitle("Testing Alerts").
                WithBody("<form id='theForm' action='javascript:alert(\"Tasty cheese\");'>",
                    "<input id='unused' type='submit' value='Submit'>",
                    "</form>")));

            IWebElement element = await driver.FindElement(By.Id("theForm"));
            await element.Submit();
            IAlert alert = await driver.SwitchTo().Alert();
            string text = await alert.Text();
            await alert.Accept();

            Assert.AreEqual("Tasty cheese", text);
            Assert.AreEqual("Testing Alerts", await driver.Title());
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldHandleAlertOnPageBeforeUnload()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("pageWithOnBeforeUnloadMessage.html"));
            IWebElement element = await driver.FindElement(By.Id("navigate"));
            await element.Click();
            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Dismiss();
            Assert.That(await driver.GetUrl(), Does.Contain("pageWithOnBeforeUnloadMessage.html"));

            // Can't move forward or even quit the driver
            // until the alert is accepted.
            await element.Click();
            alert = await WaitFor(AlertToBePresent(), "No alert found");
            await alert.Accept();
            await WaitFor(async () => await driver.GetUrl().Contains(alertsPage), "Browser URL does not contain " + alertsPage);
            Assert.That(await driver.GetUrl(), Does.Contain(alertsPage));
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldHandleAlertOnPageBeforeUnloadAlertAtQuit()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("pageWithOnBeforeUnloadMessage.html"));
            IWebElement element = await driver.FindElement(By.Id("navigate"));
            await element.Click();
            IAlert alert = await WaitFor(AlertToBePresent(), "No alert found");
            await driver.Quit();
            driver = null;
        }


        private Task<IAlert> AlertToBePresent()
        {
            return driver.SwitchTo().Alert();
        }

        private string CreateAlertPage(string alertText)
        {
            return EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithTitle("Testing Alerts")
                .WithBody("<a href='#' id='alert' onclick='alert(\"" + alertText + "\");'>click me</a>"));
        }

        private string CreatePromptPage(string defaultText)
        {
            return EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithTitle("Testing Prompt")
                .WithScripts(
                    "function setInnerText(id, value) {",
                    "  document.getElementById(id).innerHTML = '<p>' + value + '</p>';",
                    "}",
                    defaultText == null
                      ? "function displayPrompt() { setInnerText('text', prompt('Enter something')); }"
                      : "function displayPrompt() { setInnerText('text', prompt('Enter something', '" + defaultText + "')); }")

                .WithBody(
                    "<a href='#' id='prompt' onclick='displayPrompt();'>click me</a>",
                    "<div id='text'>acceptor</div>"));
        }

        private async Task SetSimpleOnBeforeUnload(string returnText)
        {
            await ((IJavaScriptExecutor)driver).ExecuteScript(
                "var returnText = arguments[0]; window.onbeforeunload = function() { return returnText; }", new CancellationToken(),
                returnText);
        }

        private Func<Task<IWebElement>> ElementToBePresent(By locator)
        {
            return async () => {
                IWebElement foundElement = null;
                try {
                    foundElement = await driver.FindElement(By.Id("open-page-with-onunload-alert"));
                } catch (NoSuchElementException) {
                }

                return foundElement;
            };
        }

        private Func<Task<bool>> ElementTextToEqual(IWebElement element, string text)
        {
            return async () => await element.Text() == text;
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

        private Func<Task<bool>> WindowHandleCountToBe(int count)
        {
            return async () => await driver.WindowHandles().Count() == count;
        }

    }
}
