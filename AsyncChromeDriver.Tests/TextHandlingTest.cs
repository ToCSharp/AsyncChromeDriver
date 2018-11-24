using System;
using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class TextHandlingTest : DriverTestFixture
    {
        private readonly string NewLine = "\r\n";

        [Test]
        public async Task ShouldReturnTheTextContentOfASingleElementWithNoChildren()
        {
            await driver.GoToUrl(simpleTestPage);
            string selectText = await driver.FindElement(By.Id("oneline")).Text();
            Assert.AreEqual(selectText, "A single line of text");

            string getText = await driver.FindElement(By.Id("oneline")).Text();
            Assert.AreEqual(getText, "A single line of text");
        }

        [Test]
        public async Task ShouldReturnTheEntireTextContentOfChildElements()
        {
            await driver.GoToUrl((simpleTestPage));
            string text = await driver.FindElement(By.Id("multiline")).Text();

            Assert.That(text, Does.Contain("A div containing"));
            Assert.That(text, Does.Contain("More than one line of text"));
            Assert.That(text, Does.Contain("and block level elements"));
        }

        [Test]
        public async Task ShouldIgnoreScriptElements()
        {
            await driver.GoToUrl(javascriptEnhancedForm);
            IWebElement labelForUsername = await driver.FindElement(By.Id("labelforusername"));
            string text = await labelForUsername.Text();

            Assert.AreEqual((await labelForUsername.FindElements(By.TagName("script"))).Count, 1);
            Assert.That(text, Does.Not.Contain("document.getElementById"));
            Assert.AreEqual(text, "Username:");
        }

        [Test]
        public async Task ShouldRepresentABlockLevelElementAsANewline()
        {
            await driver.GoToUrl(simpleTestPage);
            string text = await driver.FindElement(By.Id("multiline")).Text();

            Assert.That(text, Does.StartWith("A div containing" + NewLine));
            Assert.That(text, Does.Contain("More than one line of text" + NewLine));
            Assert.That(text, Does.EndWith("and block level elements"));
        }

        [Test]
        public async Task ShouldCollapseMultipleWhitespaceCharactersIntoASingleSpace()
        {
            await driver.GoToUrl((simpleTestPage));
            string text = await driver.FindElement(By.Id("lotsofspaces")).Text();

            Assert.AreEqual(text, "This line has lots of spaces.");
        }

        [Test]
        public async Task ShouldTrimText()
        {
            await driver.GoToUrl((simpleTestPage));
            string text = await driver.FindElement(By.Id("multiline")).Text();

            Assert.That(text, Does.StartWith("A div containing"));
            Assert.That(text, Does.EndWith("block level elements"));
        }

        [Test]
        public async Task ShouldConvertANonBreakingSpaceIntoANormalSpaceCharacter()
        {
            await driver.GoToUrl((simpleTestPage));
            string text = await driver.FindElement(By.Id("nbsp")).Text();

            Assert.AreEqual(text, "This line has a non-breaking space");
        }

        [Test]
        public async Task ShouldNotCollapseANonBreakingSpaces()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.Id("nbspandspaces"));
            string text = await element.Text();

            Assert.AreEqual(text, "This line has a   non-breaking space and spaces");
        }

        [Test]
        public async Task ShouldNotTrimNonBreakingSpacesAtTheEndOfALineInTheMiddleOfText()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.Id("multilinenbsp"));
            string text = await element.Text();
            string expectedStart = "These lines  " + System.Environment.NewLine;
            Assert.That(text, Does.StartWith(expectedStart));
        }

        [Test]
        public async Task ShouldNotTrimNonBreakingSpacesAtTheStartOfALineInTheMiddleOfText()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.Id("multilinenbsp"));
            string text = await element.Text();
            string expectedContent = System.Environment.NewLine + "  have";
            Assert.That(text, Does.Contain(expectedContent));
        }

        [Test]
        public async Task ShouldNotTrimTrailingNonBreakingSpacesInMultilineText()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.Id("multilinenbsp"));
            string text = await element.Text();
            string expectedEnd = "trailing NBSPs  ";
            Assert.That(text, Does.EndWith(expectedEnd));
        }

        [Test]
        public async Task HavingInlineElementsShouldNotAffectHowTextIsReturned()
        {
            await driver.GoToUrl((simpleTestPage));
            string text = await driver.FindElement(By.Id("inline")).Text();
            Assert.AreEqual(text, "This line has text within elements that are meant to be displayed inline");
        }

        [Test]
        public async Task ShouldReturnTheEntireTextOfInlineElements()
        {
            await driver.GoToUrl((simpleTestPage));
            string text = await driver.FindElement(By.Id("span")).Text();

            Assert.AreEqual(text, "An inline element");
        }

        [Test]
        public async Task ShouldRetainTheFormatingOfTextWithinAPreElement()
        {
            await driver.GoToUrl(simpleTestPage);
            string text = await driver.FindElement(By.Id("preformatted")).Text();

            Assert.That(text, Is.EqualTo("   This section has a preformatted" + System.Environment.NewLine +
                "    text block    " + System.Environment.NewLine +
                "  split in four lines" + System.Environment.NewLine +
                "         "));
        }

        [Test]
        public async Task ShouldRetainTheFormatingOfTextWithinAPreElementThatIsWithinARegularBlock()
        {
            await driver.GoToUrl(simpleTestPage);
            string text = await driver.FindElement(By.Id("div-with-pre")).Text();
            Assert.That(text, Is.EqualTo("before pre" + System.Environment.NewLine +
                "   This section has a preformatted" + System.Environment.NewLine +
                "    text block    " + System.Environment.NewLine +
                "  split in four lines" + System.Environment.NewLine +
                "         " + System.Environment.NewLine +
                "after pre"));
        }

        [Test]
        public async Task ShouldBeAbleToSetMoreThanOneLineOfTextInATextArea()
        {
            await driver.GoToUrl(formsPage);
            IWebElement textarea = await driver.FindElement(By.Id("withText"));
            await textarea.Clear();
            string expectedText = "I like cheese" + NewLine + NewLine + "It's really nice";
            await textarea.SendKeys(expectedText);

            string seenText = await textarea.GetAttribute("value");
            Assert.AreEqual(seenText, expectedText);
        }

        [Test]
        public async Task ShouldBeAbleToEnterDatesAfterFillingInOtherValuesFirst()
        {
            await driver.GoToUrl(formsPage);
            IWebElement input = await driver.FindElement(By.Id("working"));
            string expectedValue = "10/03/2007 to 30/07/1993";
            await input.SendKeys(expectedValue);
            string seenValue = await input.GetAttribute("value");

            Assert.AreEqual(seenValue, expectedValue);
        }

        [Test]
        public async Task ShouldReturnEmptyStringWhenTextIsOnlySpaces()
        {
            await driver.GoToUrl((xhtmlTestPage));

            string text = await driver.FindElement(By.Id("spaces")).Text();
            Assert.AreEqual(text, string.Empty);
        }

        [Test]
        public async Task ShouldReturnEmptyStringWhenTextIsEmpty()
        {
            await driver.GoToUrl((xhtmlTestPage));

            string text = await driver.FindElement(By.Id("empty")).Text();
            Assert.AreEqual(text, string.Empty);
        }

        [Test]
        public async Task ShouldReturnEmptyStringWhenTagIsSelfClosing()
        {
            await driver.GoToUrl((xhtmlFormPage));

            string text = await driver.FindElement(By.Id("self-closed")).Text();
            Assert.AreEqual(text, string.Empty);
        }

        [Test]
        public async Task ShouldNotTrimSpacesWhenLineWraps()
        {
            await driver.GoToUrl(simpleTestPage);

            string text = await driver.FindElement(By.XPath("//table/tbody/tr[1]/td[1]")).Text();
            Assert.AreEqual("beforeSpace afterSpace", text);
        }

        [Test]
        public async Task ShouldHandleSiblingBlockLevelElements()
        {
            await driver.GoToUrl(simpleTestPage);

            string text = await driver.FindElement(By.Id("twoblocks")).Text();

            Assert.AreEqual(text, "Some text" + NewLine + "Some more text");
        }

        [Test]
        public async Task ShouldHandleNestedBlockLevelElements()
        {
            await driver.GoToUrl((simpleTestPage));

            string text = await driver.FindElement(By.Id("nestedblocks")).Text();

            Assert.AreEqual("Cheese" + NewLine + "Some text" + NewLine + "Some more text" + NewLine
                                + "and also" + NewLine + "Brie", text);
        }

        [Test]
        public async Task ShouldHandleWhitespaceInInlineElements()
        {
            await driver.GoToUrl((simpleTestPage));

            string text = await driver.FindElement(By.Id("inlinespan")).Text();

            Assert.AreEqual(text, "line has text");
        }

        [Test]
        public async Task ReadALargeAmountOfData()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("macbeth.html"));
            string source = await driver.PageSource().Trim().ToLower();

            Assert.That(source, Does.EndWith("</html>"));
        }

        [Test]
        public async Task GetTextWithLineBreakForInlineElement()
        {
            await driver.GoToUrl(simpleTestPage);

            IWebElement label = await driver.FindElement(By.Id("label1"));
            string labelText = await label.Text();

            Assert.That(new Regex("foo[\\n\\r]+bar").IsMatch(labelText), "Label text '" + labelText + "' did not match regular expression 'foo[\\n\\r]+bar'");
        }

        [Test]
        public async Task ShouldOnlyIncludeVisibleText()
        {
            await driver.GoToUrl(javascriptPage);

            string empty = await driver.FindElement(By.Id("suppressedParagraph")).Text();
            string explicitText = await driver.FindElement(By.Id("outer")).Text();

            Assert.AreEqual(string.Empty, empty);
            Assert.AreEqual("sub-element that is explicitly visible", explicitText);
        }

        [Test]
        public async Task ShouldGetTextFromTableCells()
        {
            await driver.GoToUrl(tables);

            IWebElement tr = await driver.FindElement(By.Id("hidden_text"));
            String text = await tr.Text();

            Assert.That(text, Does.Contain("some text"));
            Assert.That(text, Does.Not.Contain("some more text"));
        }

        [Test]
        public async Task TextOfAnInputFieldShouldBeEmpty()
        {
            await driver.GoToUrl(formsPage);
            IWebElement input = await driver.FindElement(By.Id("inputWithText"));
            Assert.AreEqual(string.Empty, await input.Text());
        }

        [Test]
        public async Task TextOfATextAreaShouldBeEqualToItsDefaultText()
        {
            await driver.GoToUrl(formsPage);
            IWebElement area = await driver.FindElement(By.Id("withText"));
            Assert.AreEqual("Example text", await area.Text());
        }

        [Test]
        public async Task TextOfATextAreaShouldBeEqualToItsDefaultTextEvenAfterTyping()
        {
            await driver.GoToUrl(formsPage);
            IWebElement area = await driver.FindElement(By.Id("withText"));
            string oldText = await area.Text();
            await area.SendKeys("New Text");
            Assert.AreEqual(oldText, await area.Text());
        }

        [Test]
        public async Task TextOfATextAreaShouldBeEqualToItsDefaultTextEvenAfterChangingTheValue()
        {
            await driver.GoToUrl(formsPage);
            IWebElement area = await driver.FindElement(By.Id("withText"));
            string oldText = await area.GetAttribute("value");
            await ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value = arguments[1]", new CancellationToken(), area, "New Text");
            Assert.AreEqual(oldText, await area.Text());
        }

        [Test]
        public async Task ShouldGetTextWhichIsAValidJSONObject()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.Id("simpleJsonText"));
            Assert.AreEqual("{a=\"b\", c=1, d=true}", await element.Text());
            //assertEquals("{a=\"b\", \"c\"=d, e=true, f=\\123\\\\g\\\\\"\"\"\\\'}", element.getText());
        }

        [Test]
        public async Task ShouldGetTextWhichIsAValidComplexJSONObject()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.Id("complexJsonText"));
            Assert.AreEqual("{a=\"\\\\b\\\\\\\"\'\\\'\"}", await element.Text());
        }

        [Test]
        public async Task CanHandleTextThatLooksLikeANumber()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(
                 new InlinePage().WithBody("<div id='point'>12.345</div>",
                                           "<div id='comma'>12,345</div>",
                                           "<div id='space'>12 345</div>")));

            Assert.That(await driver.FindElement(By.Id("point")).Text(), Is.EqualTo("12.345"));
            Assert.That(await driver.FindElement(By.Id("comma")).Text(), Is.EqualTo("12,345"));
            Assert.That(await driver.FindElement(By.Id("space")).Text(), Is.EqualTo("12 345"));
        }

        [Test]
        public async Task CanHandleTextTransformProperty()
        {
            await driver.GoToUrl(simpleTestPage);
            Assert.That(await driver.FindElement(By.Id("capitalized")).Text(), Is.EqualTo("Hello, World! Bla-Bla-BLA").Or.EqualTo("Hello, World! Bla-bla-BLA"));
            Assert.That(await driver.FindElement(By.Id("lowercased")).Text(), Is.EqualTo("hello, world! bla-bla-bla"));
            Assert.That(await driver.FindElement(By.Id("uppercased")).Text(), Is.EqualTo("HELLO, WORLD! BLA-BLA-BLA"));
        }
    }
}
