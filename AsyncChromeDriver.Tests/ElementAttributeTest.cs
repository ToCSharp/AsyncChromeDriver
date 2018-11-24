using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ElementAttributeTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldReturnNullWhenGettingTheValueOfAnAttributeThatIsNotListed()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement head = await driver.FindElement(By.XPath("/html"));
            string attribute = await head.GetAttribute("cheese");
            Assert.That(attribute, Is.Null);
        }

        [Test]
        public async Task ShouldReturnNullWhenGettingSrcAttributeOfInvalidImgTag()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement img = await driver.FindElement(By.Id("invalidImgTag"));
            string attribute = await img.GetAttribute("src");
            Assert.That(attribute, Is.Null);
        }

        [Test]
        public async Task ShouldReturnAnAbsoluteUrlWhenGettingSrcAttributeOfAValidImgTag()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement img = await driver.FindElement(By.Id("validImgTag"));
            string attribute = await img.GetAttribute("src");
            Assert.AreEqual(EnvironmentManager.Instance.UrlBuilder.WhereIs("icon.gif"), attribute);
        }

        [Test]
        public async Task ShouldReturnAnAbsoluteUrlWhenGettingHrefAttributeOfAValidAnchorTag()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement img = await driver.FindElement(By.Id("validAnchorTag"));
            string attribute = await img.GetAttribute("href");
            Assert.AreEqual(EnvironmentManager.Instance.UrlBuilder.WhereIs("icon.gif"), attribute);
        }


        [Test]
        public async Task ShouldReturnEmptyAttributeValuesWhenPresentAndTheValueIsActuallyEmpty()
        {
            await driver.GoToUrl(simpleTestPage);
            IWebElement body = await driver.FindElement(By.XPath("//body"));
            Assert.AreEqual(string.Empty, await body.GetAttribute("style"));
        }

        [Test]
        public async Task ShouldReturnTheValueOfTheDisabledAttributeAsNullIfNotSet()
        {
            await driver.GoToUrl(formsPage);
            IWebElement inputElement = await driver.FindElement(By.XPath("//input[@id='working']"));
            Assert.That(await inputElement.GetAttribute("disabled"), Is.Null);
            Assert.That(await inputElement.Enabled(), "Element is not enabled");

            IWebElement pElement = await driver.FindElement(By.Id("peas"));
            Assert.That(await inputElement.GetAttribute("disabled"), Is.Null);
            Assert.That(await inputElement.Enabled(), "Element is not enabled");
        }

        [Test]
        public async Task ShouldReturnTheValueOfTheIndexAttrbuteEvenIfItIsMissing()
        {
            await driver.GoToUrl(formsPage);

            IWebElement multiSelect = await driver.FindElement(By.Id("multi"));
            ReadOnlyCollection<IWebElement> options = await multiSelect.FindElements(By.TagName("option"));
            Assert.AreEqual("1", await options[1].GetAttribute("index"));
        }


        [Test]
        public async Task ShouldIndicateTheElementsThatAreDisabledAreNotEnabled()
        {
            await driver.GoToUrl(formsPage);
            IWebElement inputElement = await driver.FindElement(By.XPath("//input[@id='notWorking']"));
            Assert.That(await inputElement.Enabled(), Is.False, "Element should be disabled");

            inputElement = await driver.FindElement(By.XPath("//input[@id='working']"));
            Assert.That(await inputElement.Enabled(), Is.True, "Element should be enabled");
        }

        [Test]
        public async Task ElementsShouldBeDisabledIfTheyAreDisabledUsingRandomDisabledStrings()
        {
            await driver.GoToUrl(formsPage);
            IWebElement disabledTextElement1 = await driver.FindElement(By.Id("disabledTextElement1"));
            Assert.That(await disabledTextElement1.Enabled(), Is.False, "disabledTextElement1 should be disabled");

            IWebElement disabledTextElement2 = await driver.FindElement(By.Id("disabledTextElement2"));
            Assert.That(await disabledTextElement2.Enabled(), Is.False, "disabledTextElement2 should be disabled");

            IWebElement disabledSubmitElement = await driver.FindElement(By.Id("disabledSubmitElement"));
            Assert.That(await disabledSubmitElement.Enabled(), Is.False, "disabledSubmitElement should be disabled");
        }

        [Test]
        public async Task ShouldThrowExceptionIfSendingKeysToElementDisabledUsingRandomDisabledStrings()
        {
            await driver.GoToUrl(formsPage);
            IWebElement disabledTextElement1 = await driver.FindElement(By.Id("disabledTextElement1"));
            try {
                await disabledTextElement1.SendKeys("foo");
                Assert.Fail("Should have thrown exception");
            } catch (InvalidElementStateException) {
                //Expected
            }

            Assert.AreEqual(string.Empty, await disabledTextElement1.Text());

            IWebElement disabledTextElement2 = await driver.FindElement(By.Id("disabledTextElement2"));
            try {
                await disabledTextElement2.SendKeys("bar");
                Assert.Fail("Should have thrown exception");
            } catch (InvalidElementStateException) {
                //Expected
            }

            Assert.AreEqual(string.Empty, await disabledTextElement2.Text());
        }

        [Test]
        public async Task ShouldIndicateWhenATextAreaIsDisabled()
        {
            await driver.GoToUrl(formsPage);
            IWebElement textArea = await driver.FindElement(By.XPath("//textarea[@id='notWorkingArea']"));
            Assert.That(await textArea.Enabled(), Is.False);
        }

        [Test]
        public async Task ShouldIndicateWhenASelectIsDisabled()
        {
            await driver.GoToUrl(formsPage);

            IWebElement enabled = await driver.FindElement(By.Name("selectomatic"));
            IWebElement disabled = await driver.FindElement(By.Name("no-select"));

            Assert.That(await enabled.Enabled(), Is.True, "Expected select element to be enabled");
            Assert.That(await disabled.Enabled(), Is.False, "Expected select element to be disabled");
        }

        [Test]
        public async Task ShouldReturnTheValueOfCheckedForACheckboxOnlyIfItIsChecked()
        {
            await driver.GoToUrl(formsPage);
            IWebElement checkbox = await driver.FindElement(By.XPath("//input[@id='checky']"));
            Assert.AreEqual(null, await checkbox.GetAttribute("checked"));
            await checkbox.Click();
            Assert.AreEqual("true", await checkbox.GetAttribute("checked"));
        }

        [Test]
        public async Task ShouldOnlyReturnTheValueOfSelectedForRadioButtonsIfItIsSet()
        {
            await driver.GoToUrl(formsPage);
            IWebElement neverSelected = await driver.FindElement(By.Id("cheese"));
            IWebElement initiallyNotSelected = await driver.FindElement(By.Id("peas"));
            IWebElement initiallySelected = await driver.FindElement(By.Id("cheese_and_peas"));

            Assert.AreEqual(null, await neverSelected.GetAttribute("selected"), "false");
            Assert.AreEqual(null, await initiallyNotSelected.GetAttribute("selected"), "false");
            Assert.AreEqual("true", await initiallySelected.GetAttribute("selected"), "true");

            await initiallyNotSelected.Click();
            Assert.AreEqual(null, await neverSelected.GetAttribute("selected"));
            Assert.AreEqual("true", await initiallyNotSelected.GetAttribute("selected"));
            Assert.AreEqual(null, await initiallySelected.GetAttribute("selected"));
        }

        [Test]
        public async Task ShouldReturnTheValueOfSelectedForOptionsOnlyIfTheyAreSelected()
        {
            await driver.GoToUrl(formsPage);
            IWebElement selectBox = await driver.FindElement(By.XPath("//select[@name='selectomatic']"));
            ReadOnlyCollection<IWebElement> options = await selectBox.FindElements(By.TagName("option"));
            IWebElement one = options[0];
            IWebElement two = options[1];
            Assert.That(await one.Selected(), Is.True);
            Assert.That(await two.Selected(), Is.False);
            Assert.AreEqual("true", await one.GetAttribute("selected"));
            Assert.AreEqual(null, await two.GetAttribute("selected"));
        }

        [Test]
        public async Task ShouldReturnValueOfClassAttributeOfAnElement()
        {
            await driver.GoToUrl(xhtmlTestPage);

            IWebElement heading = await driver.FindElement(By.XPath("//h1"));
            String className = await heading.GetAttribute("class");

            Assert.AreEqual("header", className);
        }

        [Test]
        public async Task ShouldReturnTheContentsOfATextAreaAsItsValue()
        {
            await driver.GoToUrl(formsPage);

            String value = await driver.FindElement(By.Id("withText")).GetAttribute("value");

            Assert.AreEqual("Example text", value);
        }

        [Test]
        public async Task ShouldReturnInnerHtml()
        {
            await driver.GoToUrl(simpleTestPage);

            string html = await driver.FindElement(By.Id("wrappingtext")).GetAttribute("innerHTML");
            Assert.That(html, Does.Contain("<tbody>"));
        }

        [Test]
        public async Task ShouldTreatReadonlyAsAValue()
        {
            await driver.GoToUrl(formsPage);

            IWebElement element = await driver.FindElement(By.Name("readonly"));
            string readOnlyAttribute = await element.GetAttribute("readonly");

            Assert.That(readOnlyAttribute, Is.Not.Null);

            IWebElement textInput = await driver.FindElement(By.Name("x"));
            string notReadOnly = await textInput.GetAttribute("readonly");

            Assert.That(notReadOnly, Is.Null);
        }

        [Test]
        public async Task ShouldReturnHiddenTextForTextContentAttribute()
        {
            await driver.GoToUrl(simpleTestPage);

            IWebElement element = await driver.FindElement(By.Id("hiddenline"));
            string textContent = await element.GetAttribute("textContent");

            Assert.AreEqual("A hidden line of text", textContent);
        }

        [Test]
        public async Task ShouldGetNumericAtribute()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("withText"));
            Assert.AreEqual("5", await element.GetAttribute("rows"));
        }

        [Test]
        public async Task CanReturnATextApproximationOfTheStyleAttribute()
        {
            await driver.GoToUrl(javascriptPage);
            string style = await driver.FindElement(By.Id("red-item")).GetAttribute("style");

            Assert.That(style.ToLower(), Does.Contain("background-color"));
        }

        public async Task ShouldCorrectlyReportValueOfColspan()
        {
            await driver.GoToUrl(tables);
            System.Threading.Thread.Sleep(1000);

            IWebElement th1 = await driver.FindElement(By.Id("th1"));
            IWebElement td2 = await driver.FindElement(By.Id("td2"));

            Assert.AreEqual("th1", await th1.GetAttribute("id"), "th1 id");
            Assert.AreEqual("3", await th1.GetAttribute("colspan"), "th1 colspan should be 3");

            Assert.AreEqual("td2", await td2.GetAttribute("id"), "td2 id");
            Assert.AreEqual("2", await td2.GetAttribute("colspan"), "td2 colspan should be 2");
        }

        // This is a test-case re-creating issue 900.
        [Test]
        public async Task ShouldReturnValueOfOnClickAttribute()
        {
            await driver.GoToUrl(javascriptPage);

            IWebElement mouseclickDiv = await driver.FindElement(By.Id("mouseclick"));

            string onClickValue = await mouseclickDiv.GetAttribute("onclick");
            string expectedOnClickValue = "displayMessage('mouse click');";
            List<string> acceptableOnClickValues = new List<string>();
            acceptableOnClickValues.Add("javascript:" + expectedOnClickValue);
            acceptableOnClickValues.Add("function anonymous()\n{\n" + expectedOnClickValue + "\n}");
            acceptableOnClickValues.Add("function onclick()\n{\n" + expectedOnClickValue + "\n}");
            Assert.That(acceptableOnClickValues, Contains.Item(onClickValue));

            IWebElement mousedownDiv = await driver.FindElement(By.Id("mousedown"));
            Assert.That(await mousedownDiv.GetAttribute("onclick"), Is.Null);
        }

        [Test]
        public async Task GetAttributeDoesNotReturnAnObjectForSvgProperties()
        {
            await driver.GoToUrl(svgPage);
            IWebElement svgElement = await driver.FindElement(By.Id("rotate"));
            Assert.AreEqual("rotate(30)", await svgElement.GetAttribute("transform"));
        }

        [Test]
        public async Task CanRetrieveTheCurrentValueOfATextFormField_textInput()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("working"));
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
            await element.SendKeys("hello world");
            Assert.AreEqual("hello world", await element.GetAttribute("value"));
        }

        [Test]
        public async Task CanRetrieveTheCurrentValueOfATextFormField_emailInput()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("email"));
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
            await element.SendKeys("hello world");
            Assert.AreEqual("hello world", await element.GetAttribute("value"));
        }

        [Test]
        public async Task CanRetrieveTheCurrentValueOfATextFormField_textArea()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("emptyTextArea"));
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
            await element.SendKeys("hello world");
            Assert.AreEqual("hello world", await element.GetAttribute("value"));
        }

        [Test]
        public async Task ShouldReturnNullForNonPresentBooleanAttributes()
        {
            await driver.GoToUrl(booleanAttributes);
            IWebElement element1 = await driver.FindElement(By.Id("working"));
            Assert.That(await element1.GetAttribute("required"), Is.Null);
            IWebElement element2 = await driver.FindElement(By.Id("wallace"));
            Assert.That(await element2.GetAttribute("nowrap"), Is.Null);
        }

        [Test]
        public async Task ShouldReturnTrueForPresentBooleanAttributes()
        {
            await driver.GoToUrl(booleanAttributes);
            IWebElement element1 = await driver.FindElement(By.Id("emailRequired"));
            Assert.AreEqual("true", await element1.GetAttribute("required"));
            IWebElement element2 = await driver.FindElement(By.Id("emptyTextAreaRequired"));
            Assert.AreEqual("true", await element2.GetAttribute("required"));
            IWebElement element3 = await driver.FindElement(By.Id("inputRequired"));
            Assert.AreEqual("true", await element3.GetAttribute("required"));
            IWebElement element4 = await driver.FindElement(By.Id("textAreaRequired"));
            Assert.AreEqual("true", await element4.GetAttribute("required"));
            IWebElement element5 = await driver.FindElement(By.Id("unwrappable"));
            Assert.AreEqual("true", await element5.GetAttribute("nowrap"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeNullWhenNotSet()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithoutMultiple"));
            Assert.AreEqual(null, await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSet()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithMultipleEqualsMultiple"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSelectHasMultipleWithValueAsBlank()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithEmptyStringMultiple"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSelectHasMultipleWithoutAValue()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithMultipleWithoutValue"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task MultipleAttributeShouldBeTrueWhenSelectHasMultipleWithValueAsSomethingElse()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("selectWithRandomMultipleValue"));
            Assert.AreEqual("true", await element.GetAttribute("multiple"));
        }

        [Test]
        public async Task GetAttributeOfUserDefinedProperty()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("userDefinedProperty.html"));
            IWebElement element = await driver.FindElement(By.Id("d"));
            Assert.AreEqual("sampleValue", await element.GetAttribute("dynamicProperty"));
        }

        [Test]
        public async Task ShouldReturnValueOfClassAttributeOfAnElementAfterSwitchingIFrame()
        {
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame("iframe1");

            IWebElement wallace = await driver.FindElement(By.XPath("//div[@id='wallace']"));
            String className = await wallace.GetAttribute("class");
            Assert.AreEqual("gromit", className);
        }
    }
}
