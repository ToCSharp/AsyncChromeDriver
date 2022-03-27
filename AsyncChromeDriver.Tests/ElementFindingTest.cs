using System;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Internal;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ElementFindingTest : DriverTestFixture
    {
        // By.id positive

        [Test]
        public async Task ShouldBeAbleToFindASingleElementById()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.Id("linkId"));
            Assert.AreEqual("linkId", await element.GetAttribute("id"));
        }

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByNumericId()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Id("2"));
            Assert.That(await element.GetAttribute("id"), Is.EqualTo("2"));
        }

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByIdWithNonAlphanumericCharacters()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Id("white space"));
            Assert.That(await element.Text(), Is.EqualTo("space"));
            IWebElement element2 = await driver.FindElement(By.Id("css#.chars"));
            Assert.That(await element2.Text(), Is.EqualTo("css escapes"));
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsById()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Id("2"));
            Assert.AreEqual(8, elements.Count);
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByNumericId()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Id("2"));
            Assert.That(elements.Count, Is.EqualTo(8));
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByIdWithNonAlphanumericCharacters()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Id("white space"));
            Assert.That(elements.Count, Is.EqualTo(2));
            ReadOnlyCollection<IWebElement> elements2 = await driver.FindElements(By.Id("css#.chars"));
            Assert.That(elements2.Count, Is.EqualTo(2));
        }

        // By.id negative

        [Test]
        public async Task ShouldNotBeAbleToLocateByIdASingleElementThatDoesNotExist()
        {
            await driver.GoToUrl(formsPage);
            Assert.That(() => driver.FindElement(By.Id("nonExistentButton")), Throws.InstanceOf<NoSuchElementException>());
        }

        [Test]
        public async Task ShouldNotBeAbleToLocateByIdMultipleElementsThatDoNotExist()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Id("nonExistentButton"));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public async Task FindingASingleElementByEmptyIdShouldThrow()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.Id("")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByEmptyIdShouldReturnEmptyList()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Id(""));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public async Task FindingASingleElementByIdWithSpaceShouldThrow()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.Id("nonexistent button")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("nonexistent button")),
                exception => Assert.AreEqual(exception.Error, "no such element"));
        }

        [Test]
        public async Task FindingMultipleElementsByIdWithSpaceShouldReturnEmptyList()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Id("nonexistent button"));
            Assert.AreEqual(0, elements.Count);
        }

        // By.Name positive

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByName()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Name("checky"));
            Assert.AreEqual("furrfu", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByName()
        {
            await driver.GoToUrl(nestedPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Name("checky"));
            Assert.That(elements, Has.Count.GreaterThan(1));
        }

        [Test]
        public async Task ShouldBeAbleToFindAnElementThatDoesNotSupportTheNameProperty()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("div1"));
            Assert.AreEqual("div1", await element.GetAttribute("name"));
        }

        // By.Name negative

        [Test]
        public async Task ShouldNotBeAbleToLocateByNameASingleElementThatDoesNotExist()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.Name("nonExistentButton")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Name("nonExistentButton")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldNotBeAbleToLocateByNameMultipleElementsThatDoNotExist()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Name("nonExistentButton"));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public async Task FindingASingleElementByEmptyNameShouldThrow()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.Name("")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Name("")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByEmptyNameShouldReturnEmptyList()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Name(""));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public async Task FindingASingleElementByNameWithSpaceShouldThrow()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.Name("nonexistent button")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Name("nonexistent button")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByNameWithSpaceShouldReturnEmptyList()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.Name("nonexistent button"));
            Assert.AreEqual(0, elements.Count);
        }

        // By.tagName positive

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByTagName()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.TagName("input"));
            Assert.AreEqual("input", await element.TagName().ToLower());
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByTagName()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.TagName("input"));
            Assert.That(elements, Has.Count.GreaterThan(1));
        }

        // By.tagName negative

        [Test]
        public async Task ShouldNotBeAbleToLocateByTagNameASingleElementThatDoesNotExist()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.TagName("nonExistentButton")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.TagName("nonExistentButton")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldNotBeAbleToLocateByTagNameMultipleElementsThatDoNotExist()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.TagName("nonExistentButton"));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public async Task FindingASingleElementByEmptyTagNameShouldThrow()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.TagName("")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.TagName("")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByEmptyTagNameShouldThrow()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElements(By.TagName("")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElements(By.TagName("")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingASingleElementByTagNameWithSpaceShouldThrow()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.TagName("nonexistent button")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.TagName("nonexistent button")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByTagNameWithSpaceShouldReturnEmptyList()
        {
            await driver.GoToUrl(formsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.TagName("nonexistent button"));
            Assert.AreEqual(0, elements.Count);
        }

        // By.ClassName positive

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByClass()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.ClassName("extraDiv"));
            Assert.That(await element.Text(), Does.StartWith("Another div starts here."));
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByClassName()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.ClassName("nameC"));
            Assert.That(elements.Count, Is.GreaterThan(1));
        }

        [Test]
        public async Task ShouldFindElementByClassWhenItIsTheFirstNameAmongMany()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.ClassName("nameA"));
            Assert.AreEqual("An H2 title", await element.Text());
        }

        [Test]
        public async Task ShouldFindElementByClassWhenItIsTheLastNameAmongMany()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.ClassName("nameC"));
            Assert.AreEqual("An H2 title", await element.Text());
        }

        [Test]
        public async Task ShouldFindElementByClassWhenItIsInTheMiddleAmongMany()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.ClassName("nameBnoise"));
            Assert.AreEqual("An H2 title", await element.Text());
        }

        [Test]
        public async Task ShouldFindElementByClassWhenItsNameIsSurroundedByWhitespace()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.ClassName("spaceAround"));
            Assert.AreEqual("Spaced out", await element.Text());
        }

        [Test]
        public async Task ShouldFindElementsByClassWhenItsNameIsSurroundedByWhitespace()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.ClassName("spaceAround"));
            Assert.AreEqual(1, elements.Count);
            Assert.AreEqual("Spaced out", await elements[0].Text());
        }

        // By.ClassName negative

        [Test]
        public async Task ShouldNotFindElementByClassWhenTheNameQueriedIsShorterThanCandidateName()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.ClassName("nameB")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.ClassName("nameB")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task FindingASingleElementByEmptyClassNameShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.ClassName("")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.ClassName("")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByEmptyClassNameShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElements(By.ClassName("")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElements(By.ClassName("")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingASingleElementByCompoundClassNameShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.ClassName("a b")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.ClassName("a b")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByCompoundClassNameShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElements(By.ClassName("a b")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElements(By.ClassName("a b")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingASingleElementByInvalidClassNameShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.ClassName("!@#$%^&*")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.ClassName("!@#$%^&*")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        // By.XPath positive

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByXPath()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.XPath("//h1"));
            Assert.AreEqual("XHTML Might Be The Future", await element.Text());
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByXPath()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.XPath("//div"));
            Assert.AreEqual(13, elements.Count);
        }

        [Test]
        public async Task ShouldBeAbleToFindManyElementsRepeatedlyByXPath()
        {
            await driver.GoToUrl(xhtmlTestPage);
            String xpathString = "//node()[contains(@id,'id')]";
            //Assert.AreEqual(3, await driver.FindElements(By.XPath(xpathString)).Count());
            Assert.AreEqual(3, await driver.FindElementsByXPath(xpathString, 1000).Count());

            xpathString = "//node()[contains(@id,'nope')]";
            Assert.AreEqual(0, await driver.FindElements(By.XPath(xpathString)).Count());
        }

        [Test]
        public async Task ShouldBeAbleToIdentifyElementsByClass()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement header = await driver.FindElement(By.XPath("//h1[@class='header']"));
            Assert.AreEqual("XHTML Might Be The Future", await header.Text());
        }

        [Test]
        public async Task ShouldBeAbleToFindAnElementByXPathWithMultipleAttributes()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(
                By.XPath("//form[@name='optional']/input[@type='submit' and @value='Click!']"));
            Assert.AreEqual("input", await element.TagName().ToLower());
            Assert.AreEqual("Click!", await element.GetAttribute("value"));
        }

        [Test]
        public async Task FindingALinkByXpathShouldLocateAnElementWithTheGivenText()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.XPath("//a[text()='click me']"));
            Assert.AreEqual("click me", await element.Text());
        }

        [Test]
        public async Task FindingALinkByXpathUsingContainsKeywordShouldWork()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.XPath("//a[contains(.,'hello world')]"));
            Assert.That(await element.Text(), Does.Contain("hello world"));
        }

        [Test]
        public async Task ShouldBeAbleToFindElementByXPathWithNamespace()
        {
            await driver.GoToUrl(svgPage);
            IWebElement element = await driver.FindElement(By.XPath("//svg:svg//svg:text"));
            Assert.That(await element.Text(), Is.EqualTo("Test Chart"));
        }


        // By.XPath negative

        [Test]
        public async Task ShouldThrowAnExceptionWhenThereIsNoLinkToClick()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.XPath("//a[@id='Not here']")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.XPath("//a[@id='Not here']")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathIsSyntacticallyInvalidInDriverFindElement()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.XPath("this][isnot][valid")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.XPath("this][isnot][valid")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathIsSyntacticallyInvalidInDriverFindElements()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElements(By.XPath("this][isnot][valid")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElements(By.XPath("this][isnot][valid")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathIsSyntacticallyInvalidInElementFindElement()
        {
            await driver.GoToUrl(formsPage);
            IWebElement body = await driver.FindElement(By.TagName("body"));
            //Assert.That(async () => await body.FindElement(By.XPath("this][isnot][valid")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await body.FindElement(By.XPath("this][isnot][valid")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathIsSyntacticallyInvalidInElementFindElements()
        {
            await driver.GoToUrl(formsPage);
            IWebElement body = await driver.FindElement(By.TagName("body"));
            //Assert.That(async () => await body.FindElements(By.XPath("this][isnot][valid")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await body.FindElements(By.XPath("this][isnot][valid")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathReturnsWrongTypeInDriverFindElement()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.XPath("count(//input)")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.XPath("count(//input)")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathReturnsWrongTypeInDriverFindElements()
        {

            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElements(By.XPath("count(//input)")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElements(By.XPath("count(//input)")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathReturnsWrongTypeInElementFindElement()
        {
            await driver.GoToUrl(formsPage);

            IWebElement body = await driver.FindElement(By.TagName("body"));
            //Assert.That(async () => await body.FindElement(By.XPath("count(//input)")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await body.FindElement(By.XPath("count(//input)")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task ShouldThrowInvalidSelectorExceptionWhenXPathReturnsWrongTypeInElementFindElements()
        {

            await driver.GoToUrl(formsPage);
            IWebElement body = await driver.FindElement(By.TagName("body"));
            //Assert.That(async () => await body.FindElements(By.XPath("count(//input)")), Throws.InstanceOf<InvalidSelectorException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await body.FindElements(By.XPath("count(//input)")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        // By.CssSelector positive

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByCssSelector()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.CssSelector("div.content"));
            Assert.AreEqual("div", await element.TagName().ToLower());
            Assert.AreEqual("content", await element.GetAttribute("class"));
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByCssSelector()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.CssSelector("p"));
            Assert.That(elements, Has.Count.GreaterThan(1));
        }

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByCompoundCssSelector()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.CssSelector("div.extraDiv, div.content"));
            Assert.AreEqual("div", await element.TagName().ToLower());
            Assert.AreEqual("content", await element.GetAttribute("class"));
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByCompoundCssSelector()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElementsByCssSelector("div.extraDiv, div.content", 1000);
            //ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.CssSelector("div.extraDiv, div.content"), );
            Assert.That(elements, Has.Count.GreaterThan(1));
            Assert.That(await elements[0].GetAttribute("class"), Is.EqualTo("content"));
            Assert.That(await elements[1].GetAttribute("class"), Is.EqualTo("extraDiv"));
        }

        [Test]
        public async Task ShouldBeAbleToFindAnElementByBooleanAttributeUsingCssSelector()
        {
            await driver.GoToUrl((EnvironmentManager.Instance.UrlBuilder.WhereIs("locators_tests/boolean_attribute_selected.html")));
            IWebElement element = await driver.FindElement(By.CssSelector("option[selected='selected']"));
            Assert.AreEqual("two", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToFindAnElementByBooleanAttributeUsingShortCssSelector()
        {
            await driver.GoToUrl((EnvironmentManager.Instance.UrlBuilder.WhereIs("locators_tests/boolean_attribute_selected.html")));
            IWebElement element = await driver.FindElement(By.CssSelector("option[selected]"));
            Assert.AreEqual("two", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToFindAnElementByBooleanAttributeUsingShortCssSelectorOnHtml4Page()
        {
            await driver.GoToUrl((EnvironmentManager.Instance.UrlBuilder.WhereIs("locators_tests/boolean_attribute_selected_html4.html")));
            IWebElement element = await driver.FindElement(By.CssSelector("option[selected]"));
            Assert.AreEqual("two", await element.GetAttribute("value"));
        }

        // By.CssSelector negative

        [Test]
        public async Task ShouldNotFindElementByCssSelectorWhenThereIsNoSuchElement()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.CssSelector(".there-is-no-such-class")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.CssSelector(".there-is-no-such-class")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldNotFindElementsByCssSelectorWhenThereIsNoSuchElement()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.CssSelector(".there-is-no-such-class"));
            Assert.AreEqual(0, elements.Count);
        }

        [Test]
        public async Task FindingASingleElementByEmptyCssSelectorShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.CssSelector("")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.CssSelector("")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByEmptyCssSelectorShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElements(By.CssSelector("")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElements(By.CssSelector("")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingASingleElementByInvalidCssSelectorShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.CssSelector("//a/b/c[@id='1']")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.CssSelector("//a/b/c[@id='1']")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        [Test]
        public async Task FindingMultipleElementsByInvalidCssSelectorShouldThrow()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElements(By.CssSelector("//a/b/c[@id='1']")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElements(By.CssSelector("//a/b/c[@id='1']")),
                exception => Assert.AreEqual("invalid selector", exception.Error));
        }

        // By.linkText positive

        [Test]
        public async Task ShouldBeAbleToFindALinkByText()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement link = await driver.FindElement(By.LinkText("click me"));
            Assert.AreEqual("click me", await link.Text());
        }

        [Test]
        public async Task ShouldBeAbleToFindMultipleLinksByText()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.LinkText("click me"));
            Assert.AreEqual(2, elements.Count, "Expected 2 links, got " + elements.Count);
        }

        [Test]
        public async Task ShouldFindElementByLinkTextContainingEqualsSign()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.LinkText("Link=equalssign"));
            Assert.AreEqual("linkWithEqualsSign", await element.GetAttribute("id"));
        }

        [Test]
        public async Task ShouldFindMultipleElementsByLinkTextContainingEqualsSign()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.LinkText("Link=equalssign"));
            Assert.AreEqual(1, elements.Count);
            Assert.AreEqual("linkWithEqualsSign", await elements[0].GetAttribute("id"));
        }

        [Test]
        public async Task FindsByLinkTextOnXhtmlPage()
        {
            await driver.GoToUrl((EnvironmentManager.Instance.UrlBuilder.WhereIs("actualXhtmlPage.xhtml")));
            string linkText = "Foo";
            IWebElement element = await driver.FindElement(By.LinkText(linkText));
            Assert.AreEqual(linkText, await element.Text());
        }

        [Test]
        public async Task LinkWithFormattingTags()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement res = await elem.FindElement(By.PartialLinkText("link with formatting tags"));
            Assert.AreEqual("link with formatting tags", await res.Text());
        }

        [Test]
        public async Task DriverCanGetLinkByLinkTestIgnoringTrailingWhitespace()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement link = await driver.FindElement(By.LinkText("link with trailing space"));
            Assert.AreEqual("linkWithTrailingSpace", await link.GetAttribute("id"));
            Assert.AreEqual("link with trailing space", await link.Text());
        }

        // By.linkText negative

        [Test]
        public async Task ShouldNotBeAbleToLocateByLinkTextASingleElementThatDoesNotExist()
        {
            await driver.GoToUrl(xhtmlTestPage);
            //Assert.That(async () => await driver.FindElement(By.LinkText("Not here either")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.LinkText("Not here either")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldNotBeAbleToLocateByLinkTextMultipleElementsThatDoNotExist()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.LinkText("Not here either"));
            Assert.AreEqual(0, elements.Count);
        }

        // By.partialLinkText positive

        [Test]
        public async Task ShouldBeAbleToFindMultipleElementsByPartialLinkText()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.PartialLinkText("ick me"));
            Assert.AreEqual(2, elements.Count);
        }

        [Test]
        public async Task ShouldBeAbleToFindASingleElementByPartialLinkText()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.PartialLinkText("anon"));
            Assert.That(await element.Text(), Does.Contain("anon"));
        }

        [Test]
        public async Task ShouldFindElementByPartialLinkTextContainingEqualsSign()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.PartialLinkText("Link="));
            Assert.AreEqual("linkWithEqualsSign", await element.GetAttribute("id"));
        }

        [Test]
        public async Task ShouldFindMultipleElementsByPartialLinkTextContainingEqualsSign()
        {
            await driver.GoToUrl(xhtmlTestPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.PartialLinkText("Link="));
            Assert.AreEqual(1, elements.Count);
            Assert.AreEqual("linkWithEqualsSign", await elements[0].GetAttribute("id"));
        }

        // Misc tests

        [Test]
        public async Task DriverShouldBeAbleToFindElementsAfterLoadingMoreThanOnePageAtATime()
        {
            await driver.GoToUrl(formsPage);
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement link = await driver.FindElement(By.LinkText("click me"));
            Assert.AreEqual("click me", await link.Text());
        }

        // You don't want to ask why this is here
        [Test]
        public async Task WhenFindingByNameShouldNotReturnById()
        {
            await driver.GoToUrl(formsPage);

            IWebElement element = await driver.FindElement(By.Name("id-name1"));
            Assert.AreEqual("name", await element.GetAttribute("value"));

            element = await driver.FindElement(By.Id("id-name1"));
            Assert.AreEqual("id", await element.GetAttribute("value"));

            element = await driver.FindElement(By.Name("id-name2"));
            Assert.AreEqual("name", await element.GetAttribute("value"));

            element = await driver.FindElement(By.Id("id-name2"));
            Assert.AreEqual("id", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldBeAbleToFindAHiddenElementsByName()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Name("hidden"));
            Assert.AreEqual("hidden", await element.GetAttribute("name"));
        }

        [Test]
        public async Task ShouldNotBeAbleToFindAnElementOnABlankPage()
        {
            await driver.GoToUrl("about:blank");
            //Assert.That(async () => await driver.FindElement(By.TagName("a")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.TagName("a")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldNotBeAbleToLocateASingleElementOnABlankPage()
        {
            // Note we're on the default start page for the browser at this point.
            //Assert.That(async () => await driver.FindElement(By.Id("nonExistantButton")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Id("nonExistantButton")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task AnElementFoundInADifferentFrameIsStale()
        {
            await driver.GoToUrl(missedJsReferencePage);
            await driver.SwitchTo().Frame("inner");
            IWebElement element = await driver.FindElement(By.Id("oneline"));
            await driver.SwitchTo().DefaultContent();
            //Assert.That(async () => { string foo = await element.Text(); }, Throws.InstanceOf<StaleElementReferenceException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.Text(),
                exception => Assert.AreEqual("stale element reference", exception.Error));
        }

        [Test]
        public async Task AnElementFoundInADifferentFrameViaJsCanBeUsed()
        {
            await driver.GoToUrl(missedJsReferencePage);

            try {
                await driver.SwitchTo().Frame("inner");
                IWebElement first = await driver.FindElement(By.Id("oneline"));

                await driver.SwitchTo().DefaultContent();
                IWebElement element = (IWebElement)await ((IJavaScriptExecutor)driver).ExecuteScript(
                    "return frames[0].document.getElementById('oneline');");


                await driver.SwitchTo().Frame("inner");

                IWebElement second = await driver.FindElement(By.Id("oneline"));

                Assert.AreEqual(first, element);
                Assert.AreEqual(second, element);
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        /////////////////////////////////////////////////
        // Tests unique to the .NET bindings
        /////////////////////////////////////////////////
        [Test]
        public async Task ShouldReturnTitleOfPageIfSet()
        {
            await driver.GoToUrl(xhtmlTestPage);
            Assert.AreEqual(await driver.Title(), "XHTML Test Page");

            await driver.GoToUrl(simpleTestPage);
            Assert.AreEqual(await driver.Title(), "Hello WebDriver");
        }

        [Test]
        public async Task ShouldBeAbleToClickOnLinkIdentifiedByText()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.FindElement(By.LinkText("click me")).Click();
            await WaitFor(async () => await driver.Title() == "We Arrive Here", "Browser title is not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldBeAbleToClickOnLinkIdentifiedById()
        {
            await driver.GoToUrl(xhtmlTestPage);
            await driver.FindElement(By.Id("linkId")).Click();
            await WaitFor(async () => await driver.Title() == "We Arrive Here", "Browser title is not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldFindAnElementBasedOnId()
        {
            await driver.GoToUrl(formsPage);

            IWebElement element = await driver.FindElement(By.Id("checky"));

            Assert.That(await element.Selected(), Is.False);
        }

        [Test]
        public async Task ShouldBeAbleToFindChildrenOfANode()
        {
            await driver.GoToUrl(selectableItemsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.XPath("/html/head"));
            IWebElement head = elements[0];
            ReadOnlyCollection<IWebElement> importedScripts = await head.FindElements(By.TagName("script"));
            Assert.AreEqual(importedScripts.Count, 3);
        }

        [Test]
        public async Task ReturnAnEmptyListWhenThereAreNoChildrenOfANode()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement table = await driver.FindElement(By.Id("table"));
            ReadOnlyCollection<IWebElement> rows = await table.FindElements(By.TagName("tr"));

            Assert.AreEqual(rows.Count, 0);
        }

        [Test]
        public async Task ShouldFindElementsByName()
        {
            await driver.GoToUrl(formsPage);

            IWebElement element = await driver.FindElement(By.Name("checky"));

            Assert.AreEqual(await element.GetAttribute("value"), "furrfu");
        }

        [Test]
        public async Task ShouldFindElementsByClassWhenItIsTheFirstNameAmongMany()
        {
            await driver.GoToUrl(xhtmlTestPage);

            IWebElement element = await driver.FindElement(By.ClassName("nameA"));
            Assert.AreEqual(await element.Text(), "An H2 title");
        }

        [Test]
        public async Task ShouldFindElementsByClassWhenItIsTheLastNameAmongMany()
        {
            await driver.GoToUrl(xhtmlTestPage);

            IWebElement element = await driver.FindElement(By.ClassName("nameC"));
            Assert.AreEqual(await element.Text(), "An H2 title");
        }

        [Test]
        public async Task ShouldFindElementsByClassWhenItIsInTheMiddleAmongMany()
        {
            await driver.GoToUrl(xhtmlTestPage);

            IWebElement element = await driver.FindElement(By.ClassName("nameBnoise"));
            Assert.AreEqual(await element.Text(), "An H2 title");
        }

        [Test]
        public async Task ShouldFindGrandChildren()
        {
            await driver.GoToUrl(formsPage);
            IWebElement form = await driver.FindElement(By.Id("nested_form"));
            await form.FindElement(By.Name("x"));
        }

        [Test]
        public async Task ShouldNotFindElementOutSideTree()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Name("login"));
            //Assert.That(async () => await element.FindElement(By.Name("x")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.FindElement(By.Name("x")),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldReturnElementsThatDoNotSupportTheNameProperty()
        {
            await driver.GoToUrl(nestedPage);

            await driver.FindElement(By.Name("div1"));
            // If this works, we're all good
        }

        [Test]
        public async Task ShouldBeAbleToClickOnLinksWithNoHrefAttribute()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement element = await driver.FindElement(By.LinkText("No href"));
            await element.Click();

            // if any exception is thrown, we won't get this far. Sanity check
            Assert.AreEqual("Changed", await driver.Title());
        }

        [Test]
        public async Task RemovingAnElementDynamicallyFromTheDomShouldCauseAStaleRefException()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement toBeDeleted = await driver.FindElement(By.Id("deleted"));
            Assert.That(await toBeDeleted.Displayed(), "Element is not displayed");

            await driver.FindElement(By.Id("delete")).Click();
            //Assert.That(async () => { bool displayedAfterDelete = await toBeDeleted.Displayed(); }, Throws.InstanceOf<StaleElementReferenceException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await toBeDeleted.Displayed(),
                exception => Assert.AreEqual("stale element reference", exception.Error));
        }

        [Test]
        public async Task FindingByTagNameShouldNotIncludeParentElementIfSameTagType()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement parent = await driver.FindElement(By.Id("my_span"));

            Assert.AreEqual(2, await parent.FindElements(By.TagName("div")).Count());
            Assert.AreEqual(2, await parent.FindElements(By.TagName("span")).Count());
        }

        [Test]
        public async Task FindingByCssShouldNotIncludeParentElementIfSameTagType()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement parent = await driver.FindElement(By.CssSelector("div#parent"));
            IWebElement child = await parent.FindElement(By.CssSelector("div"));

            Assert.AreEqual("child", await child.GetAttribute("id"));
        }

        [Test]
        public async Task FindingByXPathShouldNotIncludeParentElementIfSameTagType()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement parent = await driver.FindElement(By.Id("my_span"));

            Assert.AreEqual(2, await parent.FindElements(By.TagName("div")).Count());
            Assert.AreEqual(2, await parent.FindElements(By.TagName("span")).Count());
        }

        [Test]
        public async Task ShouldBeAbleToInjectXPathEngineIfNeeded()
        {
            await driver.GoToUrl(alertsPage);
            await driver.FindElement(By.XPath("//body"));
            await driver.FindElement(By.XPath("//h1"));
            await driver.FindElement(By.XPath("//div"));
            await driver.FindElement(By.XPath("//p"));
            await driver.FindElement(By.XPath("//a"));
        }

        [Test]
        public async Task ShouldFindElementByLinkTextContainingDoubleQuote()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.LinkText("link with \" (double quote)"));
            Assert.AreEqual("quote", await element.GetAttribute("id"));
        }

        [Test]
        public async Task ShouldFindElementByLinkTextContainingBackslash()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement element = await driver.FindElement(By.LinkText("link with \\ (backslash)"));
            Assert.AreEqual("backslash", await element.GetAttribute("id"));
        }

        private bool SupportsSelectorApi()
        {
            IJavaScriptExecutor javascriptDriver = driver as IJavaScriptExecutor;
            IFindsByCssSelector cssSelectorDriver = driver as IFindsByCssSelector;
            return (cssSelectorDriver != null) && (javascriptDriver != null);
        }
    }
}
