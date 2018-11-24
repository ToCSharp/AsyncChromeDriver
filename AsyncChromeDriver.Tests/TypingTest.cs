using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class TypingTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldFireKeyPressEvents()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("a");

            IWebElement result = await driver.FindElement(By.Id("result"));
            string text = await result.Text();
            Assert.That(text, Does.Contain("press:"));
        }

        [Test]
        public async Task ShouldFireKeyDownEvents()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("I");

            IWebElement result = await driver.FindElement(By.Id("result"));
            string text = await result.Text();
            Assert.That(text, Does.Contain("down:"));
        }

        [Test]
        public async Task ShouldFireKeyUpEvents()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("a");

            IWebElement result = await driver.FindElement(By.Id("result"));
            string text = await result.Text();
            Assert.That(text, Does.Contain("up:"));
        }

        [Test]
        public async Task ShouldTypeLowerCaseLetters()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("abc def");

            Assert.AreEqual("abc def", await keyReporter.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToTypeCapitalLetters()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("ABC DEF");

            Assert.AreEqual("ABC DEF", await keyReporter.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToTypeQuoteMarks()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("\"");

            Assert.AreEqual("\"", await keyReporter.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToTypeTheAtCharacter()
        {
            // simon: I tend to use a US/UK or AUS keyboard layout with English
            // as my primary language. There are consistent reports that we're
            // not handling i18nised keyboards properly. This test exposes this
            // in a lightweight manner when my keyboard is set to the DE mapping
            // and we're using IE.

            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("@");

            Assert.AreEqual("@", await keyReporter.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToMixUpperAndLowerCaseLetters()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("me@eXample.com");

            Assert.AreEqual("me@eXample.com", await keyReporter.GetAttribute("value"));
        }

        [Test]
        public async Task ArrowKeysShouldNotBePrintable()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys(Keys.ArrowLeft);

            Assert.AreEqual(string.Empty, await keyReporter.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToUseArrowKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement keyReporter = await driver.FindElement(By.Id("keyReporter"));
            await keyReporter.SendKeys("Tet" + Keys.ArrowLeft + "s");

            Assert.AreEqual("Test", await keyReporter.GetAttribute("value"));
        }

        [Test]
        public async Task WillSimulateAKeyUpWhenEnteringTextIntoInputElements()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyUp"));
            await element.SendKeys("I like cheese");

            IWebElement result = await driver.FindElement(By.Id("result"));
            Assert.AreEqual("I like cheese", await result.Text());
        }

        [Test]
        public async Task WillSimulateAKeyDownWhenEnteringTextIntoInputElements()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyDown"));
            await element.SendKeys("I like cheese");

            IWebElement result = await driver.FindElement(By.Id("result"));
            // Because the key down gets the result before the input element is
            // filled, we're a letter short here
            Assert.AreEqual("I like chees", await result.Text());
        }

        [Test]
        public async Task WillSimulateAKeyPressWhenEnteringTextIntoInputElements()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyPress"));
            await element.SendKeys("I like cheese");

            IWebElement result = await driver.FindElement(By.Id("result"));
            // Because the key down gets the result before the input element is
            // filled, we're a letter short here
            Assert.AreEqual("I like chees", await result.Text());
        }

        [Test]
        public async Task WillSimulateAKeyUpWhenEnteringTextIntoTextAreas()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyUpArea"));
            await element.SendKeys("I like cheese");

            IWebElement result = await driver.FindElement(By.Id("result"));
            Assert.AreEqual("I like cheese", await result.Text());
        }

        [Test]
        public async Task WillSimulateAKeyDownWhenEnteringTextIntoTextAreas()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyDownArea"));
            await element.SendKeys("I like cheese");

            IWebElement result = await driver.FindElement(By.Id("result"));
            // Because the key down gets the result before the input element is
            // filled, we're a letter short here
            Assert.AreEqual("I like chees", await result.Text());
        }

        [Test]
        public async Task WillSimulateAKeyPressWhenEnteringTextIntoTextAreas()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyPressArea"));
            await element.SendKeys("I like cheese");

            IWebElement result = await driver.FindElement(By.Id("result"));
            // Because the key down gets the result before the input element is
            // filled, we're a letter short here
            Assert.AreEqual("I like chees", await result.Text());
        }

        [Test]
        public async Task ShouldFireFocusKeyEventsInTheRightOrder()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("theworks"));
            await element.SendKeys("a");
            Assert.AreEqual("focus keydown keypress keyup", await result.Text().Trim());
        }

        [Test]
        public async Task ShouldReportKeyCodeOfArrowKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys(Keys.ArrowDown);
            await CheckRecordedKeySequence(result, 40);
            await element.SendKeys(Keys.ArrowUp);
            await CheckRecordedKeySequence(result, 38);
            await element.SendKeys(Keys.ArrowLeft);
            await CheckRecordedKeySequence(result, 37);
            await element.SendKeys(Keys.ArrowRight);
            await CheckRecordedKeySequence(result, 39);

            // And leave no rubbish/printable keys in the "keyReporter"
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldReportKeyCodeOfArrowKeysUpDownEvents()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys(Keys.ArrowDown);
            string text = await result.Text().Trim();
            Assert.That(text, Does.Contain("down: 40"));
            Assert.That(text, Does.Contain("up: 40"));
            await element.SendKeys(Keys.ArrowUp);
            text = await result.Text().Trim();
            Assert.That(text, Does.Contain("down: 38"));
            Assert.That(text, Does.Contain("up: 38"));
            await element.SendKeys(Keys.ArrowLeft);
            text = await result.Text().Trim();
            Assert.That(text, Does.Contain("down: 37"));
            Assert.That(text, Does.Contain("up: 37"));
            await element.SendKeys(Keys.ArrowRight);
            text = await result.Text().Trim();
            Assert.That(text, Does.Contain("down: 39"));
            Assert.That(text, Does.Contain("up: 39"));

            // And leave no rubbish/printable keys in the "keyReporter"
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
        }

        [Test]
        public async Task NumericNonShiftKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));

            string numericLineCharsNonShifted = "`1234567890-=[]\\;,.'/42";
            await element.SendKeys(numericLineCharsNonShifted);

            Assert.AreEqual(numericLineCharsNonShifted, await element.GetAttribute("value"));
        }

        [Test]
        public async Task NumericShiftKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("keyReporter"));

            string numericShiftsEtc = "~!@#$%^&*()_+{}:\"<>?|END~";
            await element.SendKeys(numericShiftsEtc);

            Assert.AreEqual(numericShiftsEtc, await element.GetAttribute("value"));
            string text = await result.Text().Trim();
            Assert.That(text, Does.Contain(" up: 16"));
        }

        [Test]
        public async Task LowerCaseAlphaKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));

            String lowerAlphas = "abcdefghijklmnopqrstuvwxyz";
            await element.SendKeys(lowerAlphas);

            Assert.AreEqual(lowerAlphas, await element.GetAttribute("value"));
        }

        [Test]
        public async Task UppercaseAlphaKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("keyReporter"));

            String upperAlphas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            await element.SendKeys(upperAlphas);

            Assert.AreEqual(upperAlphas, await element.GetAttribute("value"));
            string text = await result.Text().Trim();
            Assert.That(text, Does.Contain(" up: 16"));
        }

        [Test]
        public async Task AllPrintableKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("keyReporter"));

            String allPrintable =
                "!\"#$%&'()*+,-./0123456789:;<=>?@ ABCDEFGHIJKLMNO" +
                "PQRSTUVWXYZ [\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            await element.SendKeys(allPrintable);

            Assert.AreEqual(allPrintable, element.GetAttribute("value"));
            string text = await result.Text().Trim();
            Assert.That(text, Does.Contain(" up: 16"));
        }

        [Test]
        public async Task ArrowKeysAndPageUpAndDown()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("a" + Keys.Left + "b" + Keys.Right +
                                        Keys.Up + Keys.Down + Keys.PageUp + Keys.PageDown + "1");
            Assert.AreEqual("ba1", await element.GetAttribute("value"));
        }

        [Test]
        public async Task HomeAndEndAndPageUpAndPageDownKeys()
        {
            // FIXME: macs don't have HOME keys, would PGUP work?
            if (System.Environment.OSVersion.Platform == PlatformID.MacOSX) {
                return;
            }

            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("abc" + Keys.Home + "0" + Keys.Left + Keys.Right +
                                        Keys.PageUp + Keys.PageDown + Keys.End + "1" + Keys.Home +
                                        "0" + Keys.PageUp + Keys.End + "111" + Keys.Home + "00");
            Assert.AreEqual("0000abc1111", await element.GetAttribute("value"));
        }

        [Test]
        public async Task DeleteAndBackspaceKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("abcdefghi");
            Assert.AreEqual("abcdefghi", await element.GetAttribute("value"));
            await element.SendKeys(Keys.Left + Keys.Left + Keys.Delete);
            Assert.AreEqual("abcdefgi", await element.GetAttribute("value"));
            await element.SendKeys(Keys.Left + Keys.Left + Keys.Backspace);
            Assert.AreEqual("abcdfgi", await element.GetAttribute("value"));
        }

        [Test]
        public async Task SpecialSpaceKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("abcd" + Keys.Space + "fgh" + Keys.Space + "ij");
            Assert.AreEqual("abcd fgh ij", await element.GetAttribute("value"));
        }

        [Test]
        public async Task NumberpadKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("abcd" + Keys.Multiply + Keys.Subtract + Keys.Add +
                                        Keys.Decimal + Keys.Separator + Keys.NumberPad0 + Keys.NumberPad9 +
                                        Keys.Add + Keys.Semicolon + Keys.Equal + Keys.Divide +
                                        Keys.NumberPad3 + "abcd");
            Assert.AreEqual("abcd*-+.,09+;=/3abcd", await element.GetAttribute("value"));
        }

        [Test]
        public async Task FunctionKeys()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("FUNCTION" + Keys.F8 + "-KEYS" + Keys.F8);
            await element.SendKeys("" + Keys.F8 + "-TOO" + Keys.F8);
            Assert.AreEqual("FUNCTION-KEYS-TOO", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShiftSelectionDeletes()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("abcd efgh");
            Assert.AreEqual(await element.GetAttribute("value"), "abcd efgh");
            await //Could be chord problem
                       element.SendKeys(Keys.Shift + Keys.Left + Keys.Left + Keys.Left);
            await element.SendKeys(Keys.Delete);
            Assert.AreEqual("abcd e", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ChordControlHomeShiftEndDelete()
        {
            // FIXME: macs don't have HOME keys, would PGUP work?
            if (System.Environment.OSVersion.Platform == PlatformID.MacOSX) {
                return;
            }

            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("!\"#$%&'()*+,-./0123456789:;<=>?@ ABCDEFG");
            await element.SendKeys(Keys.Home);
            await element.SendKeys("" + Keys.Shift + Keys.End + Keys.Delete);

            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
            string text = await result.Text().Trim();
            Assert.That(text, Does.Contain(" up: 16"));
        }

        [Test]
        public async Task ChordReveseShiftHomeSelectionDeletes()
        {
            // FIXME: macs don't have HOME keys, would PGUP work?
            if (System.Environment.OSVersion.Platform == PlatformID.MacOSX) {
                return;
            }
            await driver.GoToUrl(javascriptPage);

            IWebElement result = await driver.FindElement(By.Id("result"));
            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            await element.SendKeys("done" + Keys.Home);
            Assert.AreEqual("done", await element.GetAttribute("value"));
            await //Sending chords
                       element.SendKeys("" + Keys.Shift + "ALL " + Keys.Home);
            Assert.AreEqual("ALL done", await element.GetAttribute("value"));
            await element.SendKeys(Keys.Delete);
            Assert.AreEqual("done", await element.GetAttribute("value"), "done");
            await element.SendKeys("" + Keys.End + Keys.Shift + Keys.Home);
            Assert.AreEqual("done", await element.GetAttribute("value"));
            // Note: trailing SHIFT up here
            string text = await result.Text().Trim();
            Assert.That(text, Does.Contain(" up: 16"), "Text should contain ' up: 16'. Actual text: {0}", text);
            await element.SendKeys("" + Keys.Delete);
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
        }

        // control-x control-v here for cut & paste tests, these work on windows
        // and linux, but not on the MAC.

        [Test]
        public async Task ChordControlCutAndPaste()
        {
            // FIXME: macs don't have HOME keys, would PGUP work?
            if (System.Environment.OSVersion.Platform == PlatformID.MacOSX) {
                return;
            }

            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.Id("keyReporter"));
            IWebElement result = await driver.FindElement(By.Id("result"));

            String paste = "!\"#$%&'()*+,-./0123456789:;<=>?@ ABCDEFG";
            await element.SendKeys(paste);
            Assert.AreEqual(paste, await element.GetAttribute("value"));
            await //Chords
                       element.SendKeys("" + Keys.Home + Keys.Shift + Keys.End);
            string text = await result.Text().Trim();
            Assert.That(text, Does.Contain(" up: 16"));
            await element.SendKeys(Keys.Control + "x");
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
            await element.SendKeys(Keys.Control + "v");
            Assert.AreEqual(paste, await element.GetAttribute("value"));
            await element.SendKeys("" + Keys.Left + Keys.Left + Keys.Left +
                                        Keys.Shift + Keys.End);
            await element.SendKeys(Keys.Control + "x" + "v");
            Assert.AreEqual(paste, await element.GetAttribute("value"));
            await element.SendKeys(Keys.Home);
            await element.SendKeys(Keys.Control + "v");
            await element.SendKeys(Keys.Control + "v" + "v");
            await element.SendKeys(Keys.Control + "v" + "v" + "v");
            Assert.AreEqual("EFGEFGEFGEFGEFGEFG" + paste, await element.GetAttribute("value"));
            await element.SendKeys("" + Keys.End + Keys.Shift + Keys.Home +
                                        Keys.Null + Keys.Delete);
            Assert.AreEqual(await element.GetAttribute("value"), string.Empty);
        }

        [Test]
        public async Task ShouldTypeIntoInputElementsThatHaveNoTypeAttribute()
        {
            await driver.GoToUrl(formsPage);

            IWebElement element = await driver.FindElement(By.Id("no-type"));
            await element.SendKeys("Should Say Cheese");
            Assert.AreEqual("Should Say Cheese", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldNotTypeIntoElementsThatPreventKeyDownEvents()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement silent = await driver.FindElement(By.Name("suppress"));
            await silent.SendKeys("s");
            Assert.AreEqual(string.Empty, await silent.GetAttribute("value"));
        }

        [Test]
        public async Task GenerateKeyPressEventEvenWhenElementPreventsDefault()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement silent = await driver.FindElement(By.Name("suppress"));
            IWebElement result = await driver.FindElement(By.Id("result"));
            await silent.SendKeys("s");
            string text = await result.Text();
        }

        [Test]
        public async Task ShouldBeAbleToTypeOnAnEmailInputField()
        {
            await driver.GoToUrl(formsPage);
            IWebElement email = await driver.FindElement(By.Id("email"));
            await email.SendKeys("foobar");
            Assert.AreEqual("foobar", await email.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToTypeOnANumberInputField()
        {
            await driver.GoToUrl(formsPage);
            IWebElement numberElement = await driver.FindElement(By.Id("age"));
            await numberElement.SendKeys("33");
            Assert.AreEqual("33", await numberElement.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldThrowIllegalArgumentException()
        {
            await driver.GoToUrl(formsPage);
            IWebElement email = await driver.FindElement(By.Id("age"));
            //Assert.That(async () => await email.SendKeys(null), Throws.InstanceOf<ArgumentNullException>());
            await AssertEx.ThrowsAsync<ArgumentNullException>(async () => await email.SendKeys(null));
        }

        [Test]
        public async Task CanSafelyTypeOnElementThatIsRemovedFromTheDomOnKeyPress()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("key_tests/remove_on_keypress.html"));

            IWebElement input = await driver.FindElement(By.Id("target"));
            IWebElement log = await driver.FindElement(By.Id("log"));

            Assert.AreEqual("", await log.GetAttribute("value"));
            await input.SendKeys("b");
            string expected = "keydown (target)\nkeyup (target)\nkeyup (body)";
            Assert.AreEqual(expected, await GetValueText(log));
            await input.SendKeys("a");

            // Some drivers (IE, Firefox) do not always generate the final keyup event since the element
            // is removed from the DOM in response to the keypress (note, this is a product of how events
            // are generated and does not match actual user behavior).
            expected += "\nkeydown (target)\na pressed; removing";
            Assert.That(await GetValueText(log), Is.EqualTo(expected).Or.EqualTo(expected + "\nkeyup (body)"));
        }

        [Test]
        public async Task CanClearNumberInputAfterTypingInvalidInput()
        {
            await driver.GoToUrl(formsPage);
            IWebElement input = await driver.FindElement(By.Id("age"));
            await input.SendKeys("e");
            await input.Clear();
            await input.SendKeys("3");
            Assert.AreEqual("3", await input.GetAttribute("value"));
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task TypingIntoAnIFrameWithContentEditableOrDesignModeSet()
        {
            await driver.GoToUrl(richTextPage);

            await driver.SwitchTo().Frame("editFrame");
            IWebElement element = await driver.SwitchTo().ActiveElement();
            await element.SendKeys("Fishy");

            await driver.SwitchTo().DefaultContent();
            IWebElement trusted = await driver.FindElement(By.Id("istrusted"));
            IWebElement id = await driver.FindElement(By.Id("tagId"));

            Assert.That(await trusted.Text(), Is.EqualTo("[true]").Or.EqualTo("[n/a]").Or.EqualTo("[]"));
            Assert.That(await id.Text(), Is.EqualTo("[frameHtml]").Or.EqualTo("[theBody]"));
        }

        [Test]
        public async Task ShouldBeAbleToTypeIntoEmptyContentEditableElement()
        {
            await driver.GoToUrl(readOnlyPage);
            IWebElement editable = await driver.FindElement(By.Id("content-editable"));
            await editable.Clear();
            await editable.SendKeys("cheese"); // requires focus on OS X

            Assert.AreEqual("cheese", await editable.Text());
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldBeAbleToTypeIntoTinyMCE()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("tinymce.html"));
            await driver.SwitchTo().Frame("mce_0_ifr");

            IWebElement editable = await driver.FindElement(By.Id("tinymce"));
            await editable.Clear();
            await editable.SendKeys("cheese"); // requires focus on OS X

            Assert.AreEqual("cheese", await editable.Text());
        }

        private async Task<string> GetValueText(IWebElement el)
        {
            // Standardize on \n and strip any trailing whitespace.
            return await el.GetAttribute("value").Replace("\r\n", "\n").Trim();
        }

        private async Task CheckRecordedKeySequence(IWebElement element, int expectedKeyCode)
        {
            string withKeyPress = string.Format("down: {0} press: {0} up: {0}", expectedKeyCode);
            string withoutKeyPress = string.Format("down: {0} up: {0}", expectedKeyCode);
            Assert.That((await element.Text()).Trim(), Is.AnyOf(withKeyPress, withoutKeyPress));
        }
    }
}
