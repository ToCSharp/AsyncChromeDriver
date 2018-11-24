using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class ChildrenFindingTest : DriverTestFixture
    {
        [Test]
        public async Task FindElementByXPath()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            IWebElement child = await element.FindElement(By.XPath("select"));
            Assert.AreEqual(await child.GetAttribute("id"), "2");
        }

        [Test]
        public async Task FindingElementsOnElementByXPathShouldFindTopLevelElements()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement parent = await driver.FindElement(By.Id("multiline"));
            ReadOnlyCollection<IWebElement> allParaElements = await driver.FindElements(By.XPath("//p"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.XPath("//p"));
            Assert.AreEqual(allParaElements.Count, children.Count);
        }

        [Test]
        public async Task FindingDotSlashElementsOnElementByXPathShouldFindNotTopLevelElements()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement parent = await driver.FindElement(By.Id("multiline"));
            ReadOnlyCollection<IWebElement> children = await parent.FindElements(By.XPath("./p"));
            Assert.AreEqual(1, children.Count);
            Assert.AreEqual("A div containing", await children[0].Text());
        }

        [Test]
        public async Task FindElementByXPathWhenNoMatch()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            //Assert.That(async () => await element.FindElement(By.XPath("select/x")), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.FindElement(By.XPath("select/x")),
                exception => Assert.AreEqual(exception.Error, "no such element"));
        }

        [Test]
        public async Task FindElementsByXPath()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            ReadOnlyCollection<IWebElement> children = await element.FindElements(By.XPath("select/option"));
            Assert.AreEqual(children.Count, 8);
            Assert.AreEqual(await children[0].Text(), "One");
            Assert.AreEqual(await children[1].Text(), "Two");
        }

        [Test]
        public async Task FindElementsByXPathWhenNoMatch()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            ReadOnlyCollection<IWebElement> children = await element.FindElements(By.XPath("select/x"));
            Assert.AreEqual(0, children.Count);
        }

        [Test]
        public async Task FindElementByName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            IWebElement child = await element.FindElement(By.Name("selectomatic"));
            Assert.AreEqual(await child.GetAttribute("id"), "2");
        }

        [Test]
        public async Task FindElementsByName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            ReadOnlyCollection<IWebElement> children = await element.FindElements(By.Name("selectomatic"));
            Assert.AreEqual(children.Count, 2);
        }

        [Test]
        public async Task FindElementById()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            IWebElement child = await element.FindElement(By.Id("2"));
            Assert.AreEqual(await child.GetAttribute("name"), "selectomatic");
        }


        [Test]
        public async Task FindElementByIdWhenMultipleMatchesExist()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Id("test_id_div"));
            IWebElement child = await element.FindElement(By.Id("test_id"));
            Assert.AreEqual(await child.Text(), "inside");
        }

        [Test]
        public async Task FindElementByIdWhenIdContainsNonAlphanumericCharacters()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Id("test_special_chars"));
            IWebElement childWithSpaces = await element.FindElement(By.Id("white space"));
            Assert.That(await childWithSpaces.Text().Contains("space"));
            IWebElement childWithCssChars = await element.FindElement(By.Id("css#.chars"));
            Assert.That(await childWithCssChars.Text(), Is.EqualTo("css escapes"));
        }

        [Test]
        public async Task FindElementByIdWhenNoMatchInContext()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Id("test_id_div"));
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await element.FindElement(By.Id("test_id_out")), 
                exception => Assert.AreEqual(exception.Error, "no such element"));
        }

        [Test]
        public async Task FindElementsById()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("form2"));
            ReadOnlyCollection<IWebElement> children = await element.FindElements(By.Id("2"));
            Assert.AreEqual(children.Count, 2);
        }

        [Test]
        public async Task FindElementsByIdWithNonAlphanumericCharacters()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Id("test_special_chars"));
            ReadOnlyCollection<IWebElement> children = await element.FindElements(By.Id("white space"));
            Assert.That(children.Count, Is.EqualTo(1));
            ReadOnlyCollection<IWebElement> children2 = await element.FindElements(By.Id("css#.chars"));
            Assert.That(children2.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task FindElementByLinkText()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("div1"));
            IWebElement child = await element.FindElement(By.LinkText("hello world"));
            Assert.AreEqual(await child.GetAttribute("name"), "link1");
        }


        [Test]
        public async Task FindElementsByLinkText()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement element = await driver.FindElement(By.Name("div1"));
            ReadOnlyCollection<IWebElement> elements = await element.FindElements(By.LinkText("hello world"));

            Assert.AreEqual(2, elements.Count);
            Assert.AreEqual(await elements[0].GetAttribute("name"), "link1");
            Assert.AreEqual(await elements[1].GetAttribute("name"), "link2");
        }

        [Test]
        public async Task ShouldFindChildElementsById()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Id("test_id_div"));
            IWebElement element = await parent.FindElement(By.Id("test_id"));
            Assert.AreEqual("inside", await element.Text());
        }

        [Test]
        public async Task ShouldFindChildElementsByClassName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("classes"));

            IWebElement element = await parent.FindElement(By.ClassName("one"));

            Assert.AreEqual("Find me", await element.Text());
        }

        [Test]
        public async Task ShouldFindChildrenByClassName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("classes"));

            ReadOnlyCollection<IWebElement> elements = await parent.FindElements(By.ClassName("one"));

            Assert.AreEqual(2, elements.Count);
        }


        [Test]
        public async Task ShouldFindChildElementsByTagName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("div1"));

            IWebElement element = await parent.FindElement(By.TagName("a"));

            Assert.AreEqual("link1", await element.GetAttribute("name"));
        }


        [Test]
        public async Task ShouldFindChildrenByTagName()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("div1"));

            ReadOnlyCollection<IWebElement> elements = await parent.FindElements(By.TagName("a"));

            Assert.AreEqual(2, elements.Count);
        }

        [Test]
        public async Task ShouldBeAbleToFindAnElementByCssSelector()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("form2"));

            IWebElement element = await parent.FindElement(By.CssSelector("*[name=\"selectomatic\"]"));

            Assert.AreEqual("2", await element.GetAttribute("id"));
        }

        [Test]
        public async Task ShouldBeAbleToFindAnElementByCss3Selector()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("form2"));

            IWebElement element = await parent.FindElement(By.CssSelector("*[name^=\"selecto\"]"));

            Assert.AreEqual("2", await element.GetAttribute("id"));
        }

        [Test]
        public async Task ShouldBeAbleToFindElementsByCssSelector()
        {
            await driver.GoToUrl(nestedPage);
            IWebElement parent = await driver.FindElement(By.Name("form2"));

            ReadOnlyCollection<IWebElement> elements = await parent.FindElements(By.CssSelector("*[name=\"selectomatic\"]"));

            Assert.AreEqual(2, elements.Count);
        }

        [Test]
        public async Task ShouldBeAbleToFindChildrenOfANode()
        {
            await driver.GoToUrl(selectableItemsPage);
            ReadOnlyCollection<IWebElement> elements = await driver.FindElements(By.XPath("/html/head"));
            IWebElement head = elements[0];
            ReadOnlyCollection<IWebElement> importedScripts = await head.FindElements(By.TagName("script"));
            Assert.That(importedScripts.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task ReturnAnEmptyListWhenThereAreNoChildrenOfANode()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement table = await driver.FindElement(By.Id("table"));
            ReadOnlyCollection<IWebElement> rows = await table.FindElements(By.TagName("tr"));

            Assert.That(rows.Count, Is.EqualTo(0));
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
                exception => Assert.AreEqual(exception.Error, "no such element"));
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
        public async Task FindMultipleElements()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            ReadOnlyCollection<IWebElement> elements = await elem.FindElements(By.PartialLinkText("link"));
            Assert.That(elements, Is.Not.Null);
            Assert.AreEqual(6, elements.Count);
        }

        [Test]
        public async Task LinkWithLeadingSpaces()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement res = await elem.FindElement(By.PartialLinkText("link with leading space"));
            Assert.AreEqual("link with leading space", await res.Text());
        }

        [Test]
        public async Task LinkWithTrailingSpace()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement res = await elem.FindElement(By.PartialLinkText("link with trailing space"));
            Assert.AreEqual("link with trailing space", await res.Text());
        }

        [Test]
        public async Task ElementCanGetLinkByLinkTestIgnoringTrailingWhitespace()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement elem = await driver.FindElement(By.Id("links"));

            IWebElement link = await elem.FindElement(By.LinkText("link with trailing space"));
            Assert.AreEqual("linkWithTrailingSpace", await link.GetAttribute("id"));
        }
    }
}
