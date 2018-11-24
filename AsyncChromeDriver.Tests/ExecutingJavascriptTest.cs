using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ExecutingJavascriptTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnAString()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(xhtmlTestPage);

            object result = await ExecuteScript("return document.title;");

            Assert.That(result, Is.InstanceOf<string>());
            Assert.That(result, Is.EqualTo("XHTML Test Page"));
        }

        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnALong()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(xhtmlTestPage);

            object result = await ExecuteScript("return document.title.length;");

            Assert.That(result, Is.InstanceOf<long>());
            Assert.That((long)result, Is.EqualTo((long)"XHTML Test Page".Length));
        }

        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnAWebElement()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(xhtmlTestPage);

            object result = await ExecuteScript("return document.getElementById('id1');");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IWebElement>());
        }

        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnABoolean()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(xhtmlTestPage);

            object result = await ExecuteScript("return true;");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<bool>());
            Assert.That((bool)result, Is.True);
        }

        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnAStringArray()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            List<object> expectedResult = new List<object>();
            expectedResult.Add("zero");
            expectedResult.Add("one");
            expectedResult.Add("two");
            object result = await ExecuteScript("return ['zero', 'one', 'two'];");
            Assert.That(result, Is.InstanceOf<ReadOnlyCollection<object>>());
            ReadOnlyCollection<object> list = (ReadOnlyCollection<object>)result;
            Assert.That(list, Is.EqualTo(expectedResult.AsReadOnly()));
        }

        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnAnArray()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            List<object> expectedResult = new List<object>();
            expectedResult.Add("zero");
            List<object> subList = new List<object>();
            subList.Add(true);
            subList.Add(false);
            expectedResult.Add(subList.AsReadOnly());
            object result = await ExecuteScript("return ['zero', [true, false]];");
            Assert.That(result, Is.InstanceOf<ReadOnlyCollection<object>>());
            ReadOnlyCollection<object> list = (ReadOnlyCollection<object>)result;
            Assert.That(result, Is.EqualTo(expectedResult.AsReadOnly()));
        }

        [Test]
        public async Task ShouldBeAbleToExecuteJavascriptAndReturnABasicObjectLiteral()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);

            object result = await ExecuteScript("return {abc: '123', tired: false};");
            Assert.That(result, Is.InstanceOf<Dictionary<string, object>>());
            Dictionary<string, object> map = (Dictionary<string, object>)result;

            Dictionary<string, object> expected = new Dictionary<string, object>();
            expected.Add("abc", "123");
            expected.Add("tired", false);

            Assert.AreEqual(expected.Count, map.Count, "Expected:<" + expected.Count + ">, but was:<" + map.Count + ">");
            foreach (string expectedKey in expected.Keys) {
                Assert.That(map, Does.ContainKey(expectedKey));
                Assert.That(map[expectedKey], Is.EqualTo(expected[expectedKey]));
            }
        }

        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnAnObjectLiteral()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);

            Dictionary<string, object> expectedPerson = new Dictionary<string, object>();
            expectedPerson.Add("first", "John");
            expectedPerson.Add("last", "Doe");
            Dictionary<string, object> expectedResult = new Dictionary<string, object>();
            expectedResult.Add("foo", "bar");
            List<object> subList = new List<object>() { "a", "b", "c" };
            expectedResult.Add("baz", subList.AsReadOnly());
            expectedResult.Add("person", expectedPerson);

            object result = await ExecuteScript(
                "return {foo:'bar', baz: ['a', 'b', 'c'], " +
                    "person: {first: 'John',last: 'Doe'}};");
            Assert.That(result, Is.InstanceOf<Dictionary<string, object>>());

            Dictionary<string, object> map = (Dictionary<string, object>)result;
            Assert.That(map, Has.Count.EqualTo(3));
            foreach (string expectedKey in expectedResult.Keys) {
                Assert.That(map, Does.ContainKey(expectedKey));
            }

            Assert.That(map["foo"], Is.EqualTo("bar"));
            Assert.That((ReadOnlyCollection<object>)map["baz"], Is.EqualTo((ReadOnlyCollection<object>)expectedResult["baz"]));

            Dictionary<string, object> person = (Dictionary<string, object>)map["person"];
            Assert.That(person, Has.Count.EqualTo(2));
            Assert.That(person["first"], Is.EqualTo("John"));
            Assert.That(person["last"], Is.EqualTo("Doe"));
        }

        [Test]
        public async Task ShouldBeAbleToExecuteSimpleJavascriptAndReturnAComplexObject()
        {
            await driver.GoToUrl(javascriptPage);

            object result = await ExecuteScript("return window.location;");

            Assert.That(result, Is.InstanceOf<Dictionary<string, object>>());
            Dictionary<string, object> map = (Dictionary<string, object>)result;
            Assert.AreEqual("http:", map["protocol"]);
            Assert.AreEqual(javascriptPage, map["href"]);
        }

        [Test]
        public async Task PassingAndReturningALongShouldReturnAWholeNumber()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            long expectedResult = 1L;
            object result = await ExecuteScript("return arguments[0];", expectedResult);
            Assert.That(result, Is.InstanceOf<int>().Or.InstanceOf<long>());
            Assert.That(result, Is.EqualTo((long)expectedResult));
        }

        [Test]
        public async Task PassingAndReturningADoubleShouldReturnADecimal()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            double expectedResult = 1.2;
            object result = await ExecuteScript("return arguments[0];", expectedResult);
            Assert.That(result, Is.InstanceOf<float>().Or.InstanceOf<double>());
            Assert.That(result, Is.EqualTo((double)expectedResult));
        }

        [Test]
        public async Task ShouldThrowAnExceptionWhenTheJavascriptIsBad()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await ExecuteScript("return squiggle();"), Throws.InstanceOf<WebDriverException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await ExecuteScript("return squiggle();"),
                exception => Assert.AreEqual("invalid operation", exception.Error));
        }

        [Test]
        public async Task ShouldBeAbleToCallFunctionsDefinedOnThePage()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);
            await ExecuteScript("displayMessage('I like cheese');");
            string text = await driver.FindElement(By.Id("result")).Text();

            Assert.AreEqual("I like cheese", text.Trim());
        }

        [Test]
        public async Task ShouldBeAbleToPassAStringAsAnArgument()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            string text = (string)await ExecuteScript("return arguments[0] == 'Hello!' ? 'Hello!' : 'Goodbye!';", "Hello!");
            Assert.AreEqual("Hello!", text);
        }

        [Test]
        public async Task ShouldBeAbleToPassABooleanAsAnArgument()
        {

            string function = "return arguments[0] == true ? true : false;";

            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            bool result = (bool)await ExecuteScript(function, true);
            Assert.That(result, Is.True);

            result = (bool)await ExecuteScript(function, false);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ShouldBeAbleToPassANumberAsAnArgument()
        {
            string functionTemplate = "return arguments[0] == {0} ? {0} : 0;";

            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            string function = string.Format(functionTemplate, 3);
            long result = (long)await ExecuteScript(function, 3);
            Assert.AreEqual(3, result);

            function = string.Format(functionTemplate, -3);
            result = (long)await ExecuteScript(function, -3);
            Assert.AreEqual(-3, result);

            function = string.Format(functionTemplate, 2147483647);
            result = (long)await ExecuteScript(function, 2147483647);
            Assert.AreEqual(2147483647, result);

            function = string.Format(functionTemplate, -2147483647);
            result = (long)await ExecuteScript(function, -2147483647);
            Assert.AreEqual(-2147483647, result);
        }

        [Test]

        public async Task ShouldBeAbleToPassAWebElementAsArgument()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);
            IWebElement button = await driver.FindElement(By.Id("plainButton"));
            string value = (string)await ExecuteScript("arguments[0]['flibble'] = arguments[0].getAttribute('id'); return arguments[0]['flibble'];", button);

            Assert.AreEqual("plainButton", value);
        }

        [Test]
        public async Task PassingArrayAsOnlyArgumentShouldFlattenArray()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            object[] array = new object[] { "zero", 1, true, 3.14159 };
            long length = (long)await ExecuteScript("return arguments[0].length", array);
            Assert.AreEqual(array.Length, length);
        }

        [Test]
        public async Task ShouldBeAbleToPassAnArrayAsAdditionalArgument()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            object[] array = new object[] { "zero", 1, true, 3.14159, false };
            long length = (long)await ExecuteScript("return arguments[1].length", "string", array);
            Assert.AreEqual(array.Length, length);
        }

        [Test]
        public async Task ShouldBeAbleToPassACollectionAsArgument()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            List<object> collection = new List<object> { "Cheddar", "Brie", 7 };
            long length = (long)await ExecuteScript("return arguments[0].length", collection);
            Assert.AreEqual(collection.Count, length);
        }


        public async Task ShouldThrowAnExceptionIfAnArgumentIsNotValid()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);
            //Assert.That(async () => await ExecuteScript("return arguments[0];", driver), Throws.InstanceOf<ArgumentException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await ExecuteScript("return arguments[0];", driver),
                exception => Assert.AreEqual("ArgumentException", exception.Error));
        }

        [Test]
        public async Task ShouldBeAbleToPassInMoreThanOneArgument()
        {
            if (!(driver is IJavaScriptExecutor)) {
                return;
            }

            await driver.GoToUrl(javascriptPage);
            string result = (string)await ExecuteScript("return arguments[0] + arguments[1];", "one", "two");

            Assert.AreEqual("onetwo", result);
        }

        [Test]
        public async Task ShouldBeAbleToGrabTheBodyOfFrameOnceSwitchedTo()
        {
            await driver.GoToUrl(richTextPage);

            await driver.SwitchTo().Frame("editFrame");
            IWebElement body = (IWebElement)await ((IJavaScriptExecutor)driver).ExecuteScript("return document.body");

            Assert.AreEqual("", await body.Text());
        }

        // This is a duplicate test of ShouldBeAbleToExecuteScriptAndReturnElementsList.
        // It's here and commented only to make comparison with the Java language bindings
        // tests easier.
        //[Test]
        //public async Task testShouldBeAbleToReturnAnArrayOfWebElements()
        //{
        //   await driver.GoToUrl(formsPage;

        //    ReadOnlyCollection<IWebElement> items = (ReadOnlyCollection<IWebElement>)((IJavaScriptExecutor)driver)
        //        .ExecuteScript("return document.getElementsByName('snack');");

        //    Assert.That(items.Count, Is.Not.EqualTo(0));
        //}

        [Test]
        public async Task JavascriptStringHandlingShouldWorkAsExpected()
        {
            await driver.GoToUrl(javascriptPage);

            string value = (string)await ExecuteScript("return '';");
            Assert.AreEqual("", value);

            value = (string)await ExecuteScript("return undefined;");
            Assert.That(value, Is.Null);

            value = (string)await ExecuteScript("return ' '");
            Assert.AreEqual(" ", value);
        }

        [Test]
        public async Task ShouldBeAbleToExecuteABigChunkOfJavascriptCode()
        {
            await driver.GoToUrl(javascriptPage);
            string path = System.IO.Path.Combine(Environment.EnvironmentManager.Instance.CurrentDirectory, ".." + System.IO.Path.DirectorySeparatorChar + "..");
            string[] fileList = System.IO.Directory.GetFiles(path, "jquery-1.2.6.min.js", System.IO.SearchOption.AllDirectories);
            if (fileList.Length > 0) {
                string jquery = System.IO.File.ReadAllText(fileList[0]);
                Assert.That(jquery.Length, Is.GreaterThan(50000));
                await ExecuteScript(jquery, null);
            }
        }

        [Test]
        public async Task ShouldBeAbleToExecuteScriptAndReturnElementsList()
        {
            await driver.GoToUrl(formsPage);
            String scriptToExec = "return document.getElementsByName('snack');";

            object resultObject = await ((IJavaScriptExecutor)driver).ExecuteScript(scriptToExec);

            ReadOnlyCollection<IWebElement> resultsList = (ReadOnlyCollection<IWebElement>)resultObject;

            Assert.That(resultsList.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task ShouldBeAbleToCreateAPersistentValue()
        {
            await driver.GoToUrl(formsPage);

            await ExecuteScript("document.alerts = []");
            await ExecuteScript("document.alerts.push('hello world');");
            string text = (string)await ExecuteScript("return document.alerts.shift()");

            Assert.AreEqual("hello world", text);
        }

        [Test]
        public async Task ShouldBeAbleToHandleAnArrayOfElementsAsAnObjectArray()
        {
            await driver.GoToUrl(formsPage);

            ReadOnlyCollection<IWebElement> forms = await driver.FindElements(By.TagName("form"));
            object[] args = new object[] { forms };

            string name = (string)await ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0][0].tagName", new CancellationToken(), args);

            Assert.AreEqual("form", name.ToLower());
        }

        [Test]
        public async Task ShouldBeAbleToPassADictionaryAsAParameter()
        {
            await driver.GoToUrl(simpleTestPage);

            List<int> nums = new List<int>() { 1, 2 };
            Dictionary<string, object> args = new Dictionary<string, object>();
            args["bar"] = "test";
            args["foo"] = nums;

            object res = await ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0]['foo'][1]", new CancellationToken(), args);

            Assert.AreEqual(2, (long)res);
        }

        [Test]
        public async Task ShouldThrowAnExceptionWhenArgumentsWithStaleElementPassed()
        {
            IJavaScriptExecutor executor = driver as IJavaScriptExecutor;
            if (executor == null) {
                return;
            }

            await driver.GoToUrl(simpleTestPage);

            IWebElement el = await driver.FindElement(By.Id("oneline"));

            await driver.GoToUrl(simpleTestPage);

            Dictionary<string, object> args = new Dictionary<string, object>();
            args["key"] = new object[] { "a", new object[] { "zero", 1, true, 3.14159, false, el }, "c" };
            //Assert.That(async () => await executor.ExecuteScript("return undefined;", new CancellationToken(), args), Throws.InstanceOf<StaleElementReferenceException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await executor.ExecuteScript("return undefined;", new CancellationToken(), args),
                exception => Assert.AreEqual("stale element reference", exception.Error));
        }

        [Test]
        public async Task ShouldHandleObjectThatThatHaveToJSONMethod()
        {
            await driver.GoToUrl(simpleTestPage);

            object value = await ExecuteScript("return window.performance.timing");

            Assert.That(value, Is.InstanceOf<Dictionary<string, object>>());
        }

        [Test]
        public async Task ShouldHandleRecursiveStructures()
        {
            await driver.GoToUrl(simpleTestPage);

            //Assert.That(async () => await ExecuteScript("var obj1 = {}; var obj2 = {}; obj1['obj2'] = obj2; obj2['obj1'] = obj1; return obj1"), Throws.InstanceOf<WebDriverException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await ExecuteScript("var obj1 = {}; var obj2 = {}; obj1['obj2'] = obj2; obj2['obj1'] = obj1; return obj1"),
                exception => Assert.AreEqual("WebDriverException", exception.Error));
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true, IsCreatedAfterTest = true)]
        [Ignore("Reason for ignore: Failure indicates hang condition, which would break the test suite. Really needs a timeout set.")]
        public async Task ShouldThrowExceptionIfExecutingOnNoPage()
        {
            bool exceptionCaught = false;
            try {
                await ((IJavaScriptExecutor)driver).ExecuteScript("return 1;");
            } catch (WebDriverException) {
                exceptionCaught = true;
            }

            if (!exceptionCaught) {
                Assert.Fail("Expected an exception to be caught");
            }
        }

        [Test]
        public async Task ExecutingLargeJavaScript()
        {
            string script = "// stolen from injectableSelenium.js in WebDriver\nvar browserbot = {\n\n    triggerEvent: function(element, eventType, canBubble, controlKeyDown, altKeyDown, shiftKeyDown, metaKeyDown) {\n        canBubble = (typeof(canBubble) == undefined) ? true: canBubble;\n        if (element.fireEvent && element.ownerDocument && element.ownerDocument.createEventObject) {\n            // IE\n            var evt = this.createEventObject(element, controlKeyDown, altKeyDown, shiftKeyDown, metaKeyDown);\n            element.fireEvent('on' + eventType,evt);\n        } else {\n            var evt = document.createEvent('HTMLEvents');\n\n            try {\n                evt.shiftKey = shiftKeyDown;\n       evt.metaKey = metaKeyDown;\n                evt.altKey = altKeyDown;\n             evt.ctrlKey = controlKeyDown;\n            } catch(e) {\n      // Nothing sane to do\n                }\n\n            evt.initEvent(eventType, canBubble, true);\n            return element.dispatchEvent(evt);\n  }\n    },\n\n    getVisibleText: function() {\n        var selection = getSelection();\n        var range = document.createRange();\n        range.selectNodeContents(document.documentElement);\n        selection.addRange(range);\nvar string = selection.toString();\n        selection.removeAllRanges();\n\n    return string;\n    },\n\n    getOuterHTML: function(element) {\n        if(element.outerHTML) {\n            return element.outerHTML;\n        } else if(typeof(XMLSerializer) != undefined) {\n            return new XMLSerializer().serializeToString(element);\n        } else {\n            throw \"can't get outerHTML in this browser\";\n        }\n    }\n\n\n};return browserbot.getOuterHTML.apply(browserbot, arguments);";
            await driver.GoToUrl(javascriptPage);
            IWebElement element = await driver.FindElement(By.TagName("body"));
            object x = await ExecuteScript(script, element);
        }

        [Test]

        public async Task ShouldBeAbleToPassMoreThanOneStringAsArguments()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);
            string text = (string)await ExecuteScript("return arguments[0] + arguments[1] + arguments[2] + arguments[3];", "Hello,", " ", "world", "!");

            Assert.AreEqual("Hello, world!", text);
        }

        [Test]
        public async Task ShouldBeAbleToPassMoreThanOneBooleanAsArguments()
        {

            string function = "return (arguments[0] ? 'True' : 'False') + (arguments[1] ? 'True' : 'False');";

            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            string text = (string)await ExecuteScript(function, true, true);
            Assert.AreEqual("TrueTrue", text);

            text = (string)await ExecuteScript(function, false, true);
            Assert.AreEqual("FalseTrue", text);

            text = (string)await ExecuteScript(function, true, false);
            Assert.AreEqual("TrueFalse", text);

            text = (string)await ExecuteScript(function, false, false);
            Assert.AreEqual("FalseFalse", text);
        }

        [Test]
        public async Task ShouldBeAbleToPassMoreThanOneNumberAsArguments()
        {
            string function = "return arguments[0]+arguments[1];";

            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            long result = (long)await ExecuteScript(function, 30, 12);
            Assert.AreEqual(42, result);

            result = (long)await ExecuteScript(function, -30, -12);
            Assert.AreEqual(-42, result);

            result = (long)await ExecuteScript(function, 2147483646, 1);
            Assert.AreEqual(2147483647, result);

            result = (long)await ExecuteScript(function, -2147483646, -1);
            Assert.AreEqual(-2147483647, result);

        }

        [Test]
        public async Task ShouldBeAbleToPassADoubleAsAnArgument()
        {
            string function = "return arguments[0];";

            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            double result = (double)await ExecuteScript(function, (double)4.2);
            Assert.AreEqual(4.2, result);

            result = (double)await ExecuteScript(function, (double)-4.2);
            Assert.AreEqual(-4.2, result);

            result = (double)await ExecuteScript(function, (float)4.2);
            Assert.AreEqual(4.2, result);

            result = (double)await ExecuteScript(function, (float)-4.2);
            Assert.AreEqual(-4.2, result);

            result = (long)await ExecuteScript(function, (double)4.0);
            Assert.AreEqual(4, result);

            result = (long)await ExecuteScript(function, (double)-4.0);
            Assert.AreEqual(-4, result);
        }

        [Test]
        public async Task ShouldBeAbleToPassMoreThanOneDoubleAsArguments()
        {
            String function = "return arguments[0]+arguments[1];";

            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            double result = (double)await ExecuteScript(function, 30.1, 12.1);
            Assert.AreEqual(42.2, result);

            result = (double)await ExecuteScript(function, -30.1, -12.1);
            Assert.AreEqual(-42.2, result);

            result = (double)await ExecuteScript(function, 2147483646.1, 1.0);
            Assert.AreEqual(2147483647.1, result);

            result = (double)await ExecuteScript(function, -2147483646.1, -1.0);
            Assert.AreEqual(-2147483647.1, result);

        }

        [Test]
        public async Task ShouldBeAbleToPassMoreThanOneWebElementAsArguments()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);
            IWebElement button = await driver.FindElement(By.Id("plainButton"));
            IWebElement dynamo = await driver.FindElement(By.Id("dynamo"));
            string value = (string)await ExecuteScript("arguments[0]['flibble'] = arguments[0].getAttribute('id'); return arguments[0]['flibble'] + arguments[1].innerHTML;", button, dynamo);

            Assert.AreEqual("plainButtonWhat's for dinner?", value);
        }

        [Test]
        public async Task ShouldBeAbleToPassInMixedArguments()
        {
            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            IWebElement dynamo = await driver.FindElement(By.Id("dynamo"));
            string result = (string)await ExecuteScript("return arguments[0].innerHTML + arguments[1].toString() + arguments[2].toString() + arguments[3] + arguments[4]",
                dynamo,
                42,
                4.2,
                "Hello, World!",
                true);

            Assert.AreEqual("What's for dinner?424.2Hello, World!true", result);

        }

        [Test]
        public async Task ShouldBeAbleToPassInAndRetrieveDates()
        {
            string function = "displayMessage(arguments[0]);";

            if (!(driver is IJavaScriptExecutor))
                return;

            await driver.GoToUrl(javascriptPage);

            await ExecuteScript(function, "2014-05-20T20:00:00+08:00");
            IWebElement element = await driver.FindElement(By.Id("result"));
            string text = await element.Text();
            Assert.AreEqual("2014-05-20T20:00:00+08:00", text);
        }

        private async Task<object> ExecuteScript(String script, params Object[] args)
        {
            object toReturn = await ((IJavaScriptExecutor)driver).ExecuteScript(script, new CancellationToken(), args);
            return toReturn;
        }
    }
}
