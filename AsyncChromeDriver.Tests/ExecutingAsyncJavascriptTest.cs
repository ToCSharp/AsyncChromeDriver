using System;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class ExecutingAsyncJavascriptTest : DriverTestFixture
    {
        private IJavaScriptExecutor executor;
        private TimeSpan originalTimeout = TimeSpan.MinValue;

        [SetUp]
        public async Task SetUpEnvironment()
        {
            if (driver is IJavaScriptExecutor scriptExecutor) {
                executor = scriptExecutor;
            }

            try {
                originalTimeout = await driver.Options().Timeouts.GetAsynchronousJavaScript();
            } catch (NotImplementedException) {
                // For driver implementations that do not support getting timeouts,
                // just set a default 30-second timeout.
                originalTimeout = TimeSpan.FromSeconds(30);
            }

            await driver.Options().Timeouts.SetAsynchronousJavaScript(TimeSpan.FromSeconds(1));
        }

        [TearDown]
        public async Task TearDownEnvironment()
        {
            await driver.Options().Timeouts.SetAsynchronousJavaScript(originalTimeout);
        }

        [Test]
        public async Task ShouldNotTimeoutIfCallbackInvokedImmediately()
        {
            await driver.GoToUrl(ajaxyPage);
            object result = await executor.ExecuteAsyncScript("arguments[arguments.length - 1](123);");
            Assert.That(result, Is.InstanceOf<long>());
            Assert.That((long)result, Is.EqualTo(123));
        }

        [Test]
        public async Task ShouldBeAbleToReturnJavascriptPrimitivesFromAsyncScripts_NeitherNullNorUndefined()
        {
            await driver.GoToUrl(ajaxyPage);
            Assert.That((long)await executor.ExecuteAsyncScript("arguments[arguments.length - 1](123);"), Is.EqualTo(123));
            await driver.GoToUrl(ajaxyPage);
            Assert.That((await executor.ExecuteAsyncScript("arguments[arguments.length - 1]('abc');")).ToString(), Is.EqualTo("abc"));
            await driver.GoToUrl(ajaxyPage);
            Assert.That((bool)await executor.ExecuteAsyncScript("arguments[arguments.length - 1](false);"), Is.False);
            await driver.GoToUrl(ajaxyPage);
            Assert.That((bool)await executor.ExecuteAsyncScript("arguments[arguments.length - 1](true);"), Is.True);
        }

        [Test]
        public async Task ShouldBeAbleToReturnJavascriptPrimitivesFromAsyncScripts_NullAndUndefined()
        {
            await driver.GoToUrl(ajaxyPage);
            Assert.That(await executor.ExecuteAsyncScript("arguments[arguments.length - 1](null);"), Is.Null);
            Assert.That(await executor.ExecuteAsyncScript("arguments[arguments.length - 1]();"), Is.Null);
        }

        [Test]
        public async Task ShouldBeAbleToReturnAnArrayLiteralFromAnAsyncScript()
        {
            await driver.GoToUrl(ajaxyPage);

            object result = await executor.ExecuteAsyncScript("arguments[arguments.length - 1]([]);");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReadOnlyCollection<object>>());
            Assert.That((ReadOnlyCollection<object>)result, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task ShouldBeAbleToReturnAnArrayObjectFromAnAsyncScript()
        {
            await driver.GoToUrl(ajaxyPage);

            object result = await executor.ExecuteAsyncScript("arguments[arguments.length - 1](new Array());");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReadOnlyCollection<object>>());
            Assert.That((ReadOnlyCollection<object>)result, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task ShouldBeAbleToReturnArraysOfPrimitivesFromAsyncScripts()
        {
            await driver.GoToUrl(ajaxyPage);

            object result = await executor.ExecuteAsyncScript("arguments[arguments.length - 1]([null, 123, 'abc', true, false]);");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReadOnlyCollection<object>>());
            ReadOnlyCollection<object> resultList = result as ReadOnlyCollection<object>;
            Assert.That(resultList.Count, Is.EqualTo(5));
            Assert.That(resultList[0], Is.Null);
            Assert.That((long)resultList[1], Is.EqualTo(123));
            Assert.That(resultList[2].ToString(), Is.EqualTo("abc"));
            Assert.That((bool)resultList[3], Is.True);
            Assert.That((bool)resultList[4], Is.False);
        }

        [Test]
        public async Task ShouldBeAbleToReturnWebElementsFromAsyncScripts()
        {
            await driver.GoToUrl(ajaxyPage);

            object result = await executor.ExecuteAsyncScript("arguments[arguments.length - 1](document.body);");
            Assert.That(result, Is.InstanceOf<IWebElement>());
            Assert.That((await ((IWebElement)result).TagName()).ToLower(), Is.EqualTo("body"));
        }

        [Test]
        public async Task ShouldBeAbleToReturnArraysOfWebElementsFromAsyncScripts()
        {
            await driver.GoToUrl(ajaxyPage);

            object result = await executor.ExecuteAsyncScript("arguments[arguments.length - 1]([document.body, document.body]);");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ReadOnlyCollection<IWebElement>>());
            ReadOnlyCollection<IWebElement> resultsList = (ReadOnlyCollection<IWebElement>)result;
            Assert.That(resultsList, Has.Count.EqualTo(2));
            Assert.That(resultsList[0], Is.InstanceOf<IWebElement>());
            Assert.That(resultsList[1], Is.InstanceOf<IWebElement>());
            Assert.That((await ((IWebElement)resultsList[0]).TagName()).ToLower(), Is.EqualTo("body"));
            Assert.That(((IWebElement)resultsList[0]), Is.EqualTo((IWebElement)resultsList[1]));
        }

        [Test]
        public async Task ShouldTimeoutIfScriptDoesNotInvokeCallback()
        {
            await driver.GoToUrl(ajaxyPage);
            //Assert.That(async () => await executor.ExecuteAsyncScript("return 1 + 2;"), Throws.InstanceOf<WebDriverTimeoutException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await executor.ExecuteAsyncScript("return 1 + 2;"),
                exception => Assert.AreEqual("WebDriverTimeoutException", exception.Error));
        }

        [Test]
        public async Task ShouldTimeoutIfScriptDoesNotInvokeCallbackWithAZeroTimeout()
        {
            await driver.GoToUrl(ajaxyPage);
            //Assert.That(async () => await executor.ExecuteAsyncScript("window.setTimeout(function() {}, 0);"), Throws.InstanceOf<WebDriverTimeoutException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await executor.ExecuteAsyncScript("window.setTimeout(function() {}, 0);"),
                exception => Assert.AreEqual("WebDriverTimeoutException", exception.Error));
        }

        [Test]
        public async Task ShouldNotTimeoutIfScriptCallsbackInsideAZeroTimeout()
        {
            await driver.GoToUrl(ajaxyPage);
            await executor.ExecuteAsyncScript(
                "var callback = arguments[arguments.length - 1];" +
                "window.setTimeout(function() { callback(123); }, 0)");
        }

        [Test]
        public async Task ShouldTimeoutIfScriptDoesNotInvokeCallbackWithLongTimeout()
        {
            await driver.Options().Timeouts.SetAsynchronousJavaScript(TimeSpan.FromMilliseconds(500));
            await driver.GoToUrl(ajaxyPage);
            //Assert.That(async () => await executor.ExecuteAsyncScript(
            //    "var callback = arguments[arguments.length - 1];" +
            //    "window.setTimeout(callback, 1500);"), Throws.InstanceOf<WebDriverTimeoutException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await executor.ExecuteAsyncScript(
                    "var callback = arguments[arguments.length - 1];" +
                    "window.setTimeout(callback, 1500);"),
                exception => Assert.AreEqual("WebDriverTimeoutException", exception.Error));
        }

        [Test]
        public async Task ShouldDetectPageLoadsWhileWaitingOnAnAsyncScriptAndReturnAnError()
        {
            await driver.GoToUrl(ajaxyPage);
            //Assert.That(async () => await executor.ExecuteAsyncScript("window.location = '" + dynamicPage + "';"), Throws.InstanceOf<WebDriverException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await executor.ExecuteAsyncScript("window.location = '" + dynamicPage + "';"),
                exception => Assert.AreEqual("WebDriverException", exception.Error));
        }

        [Test]
        public async Task ShouldCatchErrorsWhenExecutingInitialScript()
        {
            await driver.GoToUrl(ajaxyPage);
            //Assert.That(async () => await executor.ExecuteAsyncScript("throw Error('you should catch this!');"), Throws.InstanceOf<WebDriverException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await executor.ExecuteAsyncScript("throw Error('you should catch this!');"),
                exception => Assert.AreEqual("WebDriverException", exception.Error));
        }

        [Test]
        public async Task ShouldNotTimeoutWithMultipleCallsTheFirstOneBeingSynchronous()
        {
            await driver.GoToUrl(ajaxyPage);
            await driver.Options().Timeouts.SetAsynchronousJavaScript(TimeSpan.FromMilliseconds(1000));
            Assert.That((bool)await executor.ExecuteAsyncScript("arguments[arguments.length - 1](true);"), Is.True);
            Assert.That((bool)await executor.ExecuteAsyncScript("var cb = arguments[arguments.length - 1]; window.setTimeout(function(){cb(true);}, 9);"), Is.True);
        }

        [Test]
        public async Task ShouldBeAbleToExecuteAsynchronousScripts()
        {
            // Reset the timeout to the 30-second default instead of zero.
            await driver.Options().Timeouts.SetAsynchronousJavaScript(TimeSpan.FromSeconds(30));
            await driver.GoToUrl(ajaxyPage);

            IWebElement typer = await driver.FindElement(By.Name("typer"));
            await typer.SendKeys("bob");
            Assert.AreEqual("bob", typer.GetAttribute("value"));

            await driver.FindElement(By.Id("red")).Click();
            await driver.FindElement(By.Name("submit")).Click();

            Assert.AreEqual(1, await GetNumberOfDivElements(), "There should only be 1 DIV at this point, which is used for the butter message");

            await driver.Options().Timeouts.SetAsynchronousJavaScript(TimeSpan.FromSeconds(10));
            string text = (string)await executor.ExecuteAsyncScript(
                "var callback = arguments[arguments.length - 1];"
                + "window.registerListener(arguments[arguments.length - 1]);");
            Assert.AreEqual("bob", text);
            Assert.AreEqual("", await typer.GetAttribute("value"));

            Assert.AreEqual(2, await GetNumberOfDivElements(), "There should be 1 DIV (for the butter message) + 1 DIV (for the new label)");
        }

        [Test]
        public async Task ShouldBeAbleToPassMultipleArgumentsToAsyncScripts()
        {
            await driver.GoToUrl(ajaxyPage);
            long result = (long)await executor.ExecuteAsyncScript("arguments[arguments.length - 1](arguments[0] + arguments[1]);", new CancellationToken(), 1, 2);
            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task ShouldBeAbleToMakeXMLHttpRequestsAndWaitForTheResponse()
        {
            string script =
                "var url = arguments[0];" +
                "var callback = arguments[arguments.length - 1];" +
                // Adapted from http://www.quirksmode.org/js/xmlhttp.html
                "var XMLHttpFactories = [" +
                "  function () {return new XMLHttpRequest()}," +
                "  function () {return new ActiveXObject('Msxml2.XMLHTTP')}," +
                "  function () {return new ActiveXObject('Msxml3.XMLHTTP')}," +
                "  function () {return new ActiveXObject('Microsoft.XMLHTTP')}" +
                "];" +
                "var xhr = false;" +
                "while (!xhr && XMLHttpFactories.length) {" +
                "  try {" +
                "    xhr = XMLHttpFactories.shift().call();" +
                "  } catch (e) {}" +
                "}" +
                "if (!xhr) throw Error('unable to create XHR object');" +
                "xhr.open('GET', url, true);" +
                "xhr.onreadystatechange = function() {" +
                "  if (xhr.readyState == 4) callback(xhr.responseText);" +
                "};" +
                "xhr.send();";

            await driver.GoToUrl(ajaxyPage);
            await driver.Options().Timeouts.SetAsynchronousJavaScript(TimeSpan.FromSeconds(3));
            string response = (string)await executor.ExecuteAsyncScript(script, new CancellationToken(), sleepingPage + "?time=2");
            Assert.AreEqual("<html><head><title>Done</title></head><body>Slept for 2s</body></html>", response.Trim());
        }

        private async Task<long> GetNumberOfDivElements()
        {
            IJavaScriptExecutor jsExecutor = driver as IJavaScriptExecutor;
            // Selenium does not support "findElements" yet, so we have to do this through a script.
            return (long)await jsExecutor.ExecuteScript("return document.getElementsByTagName('div').length;");
        }
    }
}
