using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class SelectElementHandlingTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldBePossibleToDeselectASingleOptionFromASelectWhichAllowsMultipleChoices()
        {
            await driver.GoToUrl(formsPage);

            IWebElement multiSelect = await driver.FindElement(By.Id("multi"));
            ReadOnlyCollection<IWebElement> options = await multiSelect.FindElements(By.TagName("option"));

            IWebElement option = options[0];
            Assert.That(await option.Selected(), Is.True);
            await option.Click();
            Assert.That(await option.Selected(), Is.False);
            await option.Click();
            Assert.That(await option.Selected(), Is.True);

            option = options[2];
            Assert.That(await option.Selected(), Is.True);
        }

        [Test]
        public async Task ShouldBeAbleToChangeTheSelectedOptionInASelect()
        {
            await driver.GoToUrl(formsPage);
            IWebElement selectBox = await driver.FindElement(By.XPath("//select[@name='selectomatic']"));
            ReadOnlyCollection<IWebElement> options = await selectBox.FindElements(By.TagName("option"));
            IWebElement one = options[0];
            IWebElement two = options[1];
            Assert.That(await one.Selected(), Is.True);
            Assert.That(await two.Selected(), Is.False);
            await two.Click();
            Assert.That(await one.Selected(), Is.False);
            Assert.That(await two.Selected(), Is.True);
        }

        [Test]
        public async Task ShouldBeAbleToSelectMoreThanOneOptionFromASelectWhichAllowsMultipleChoices()
        {
            await driver.GoToUrl(formsPage);

            IWebElement multiSelect = await driver.FindElement(By.Id("multi"));
            ReadOnlyCollection<IWebElement> options = await multiSelect.FindElements(By.TagName("option"));
            foreach (IWebElement option in options) {
                if (!(await option.Selected())) {
                    await option.Click();
                }
            }

            for (int i = 0; i < options.Count; i++) {
                IWebElement option = options[i];
                Assert.That(await option.Selected(), Is.True, "Option at index is not selected but should be: " + i.ToString());
            }
        }

        [Test]
        public async Task ShouldSelectFirstOptionByDefaultIfNoneIsSelected()
        {
            await driver.GoToUrl(formsPage);
            IWebElement selectBox = await driver.FindElement(By.XPath("//select[@name='select-default']"));
            IList<IWebElement> options = await selectBox.FindElements(By.TagName("option"));
            IWebElement one = options[0];
            IWebElement two = options[1];
            Assert.That(await one.Selected(), Is.True);
            Assert.That(await two.Selected(), Is.False);
            await two.Click();
            Assert.That(await one.Selected(), Is.False);
            Assert.That(await two.Selected(), Is.True);
        }

        [Test]
        public async Task CanSelectElementsInOptGroups()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.Id("two-in-group"));
            await element.Click();
            Assert.That(await element.Selected(), Is.True, "Expected to be selected");
        }

        [Test]
        public async Task CanGetValueFromOptionViaAttributeWhenAttributeDoesntExist()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.CssSelector("select[name='select-default'] option"));
            Assert.That(await element.GetAttribute("value"), Is.EqualTo("One"));
            element = await driver.FindElement(By.Id("blankOption"));
            Assert.That(await element.GetAttribute("value"), Is.EqualTo(""));
        }

        [Test]
        public async Task CanGetValueFromOptionViaAttributeWhenAttributeIsEmptyString()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("optionEmptyValueSet"));
            Assert.That(await element.GetAttribute("value"), Is.EqualTo(""));
        }

        [Test]
        public async Task CanSelectFromMultipleSelectWhereValueIsBelowVisibleRange()
        {
            await driver.GoToUrl(selectPage);
            IWebElement option = (await driver.FindElements(By.CssSelector("#selectWithMultipleLongList option")))[4];
            await option.Click();
            Assert.That(await option.Selected(), Is.True);
        }

        [Test]
        public async Task CannotSetDisabledOption()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.CssSelector("#visibility .disabled"));
            await element.Click();
            Assert.That(await element.Selected(), Is.False, "Expected to not be selected");
        }

        [Test]
        public async Task CanSetHiddenOption()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.CssSelector("#visibility .hidden"));
            await element.Click();
            Assert.That(await element.Selected(), Is.True, "Expected to be selected");
        }

        [Test]
        public async Task CanSetInvisibleOption()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.CssSelector("#visibility .invisible"));
            await element.Click();
            Assert.That(await element.Selected(), Is.True, "Expected to be selected");
        }

        [Test]
        public async Task CanHandleTransparentSelect()
        {
            await driver.GoToUrl(selectPage);
            IWebElement element = await driver.FindElement(By.CssSelector("#transparent option"));
            await element.Click();
            Assert.That(await element.Selected(), Is.True, "Expected to be selected");
        }
    }
}
