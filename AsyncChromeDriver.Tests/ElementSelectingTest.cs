using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class ElementSelectingTest : DriverTestFixture
    {
        private const string assertCannotPerformActionFormat_action_element = "Expected exception - should not be able to {0} element {1}";

        [Test]
        public async Task ShouldBeAbleToSelectAnEnabledUnselectedCheckbox()
        {
            await driver.GoToUrl(formsPage);
            await AssertCanSelect(await this.EnabledUnselectedCheckbox());
        }

        [Test]
        public async Task ShouldBeAbleToSelectAnEnabledUnselectedRadioButton()
        {
            await driver.GoToUrl(formsPage);
            await AssertCanSelect(await this.EnabledUnselectedRadioButton());
        }

        [Test]
        public async Task ShouldNotBeAbleToSelectADisabledCheckbox()
        {
            await driver.GoToUrl(formsPage);
            await AssertCannotSelect(await this.DisabledUnselectedCheckbox());
        }

        [Test]
        public async Task ShouldNotBeAbleToSelectADisabledCheckboxDisabledWithRandomString()
        {
            await driver.GoToUrl(formsPage);
            await AssertCannotSelect(await this.RandomlyDisabledSelectedCheckbox());
        }

        [Test]
        public async Task ShouldNotBeAbleToSelectADisabledRadioButton()
        {
            await driver.GoToUrl(formsPage);
            await AssertCannotSelect(await this.DisabledUnselectedRadioButton());
        }

        [Test]
        public async Task ShouldNotBeAbleToSelectADisabledRadioButtonDisabledWithRandomString()
        {
            await driver.GoToUrl(formsPage);
            await AssertCannotSelect(await this.RandomlyDisabledUnselectedRadioButton());
        }

        [Test]
        public async Task SelectingRadioButtonShouldUnselectItsSibling()
        {
            await driver.GoToUrl(formsPage);

            IWebElement originallySelected = await this.EnabledSelectedRadioButton();
            await AssertSelected(originallySelected);

            IWebElement toSelect = await this.EnabledUnselectedRadioButton();
            await AssertNotSelected(toSelect);

            await toSelect.Click();
            await AssertNotSelected(originallySelected);
            await AssertSelected(toSelect);
        }

        [Test]
        public async Task ShouldBeAbleToToggleAnEnabledUnselectedCheckbox()
        {
            await driver.GoToUrl(formsPage);

            IWebElement checkbox = await this.EnabledUnselectedCheckbox();
            await AssertNotSelected(checkbox);

            await checkbox.Click();
            await AssertSelected(checkbox);

            await checkbox.Click();
            await AssertNotSelected(checkbox);
        }

        [Test]
        public async Task ShouldBeAbleToToggleAnEnabledSelectedCheckbox()
        {
            await driver.GoToUrl(formsPage);

            IWebElement checkbox = await this.EnabledSelectedCheckbox();
            await AssertSelected(checkbox);

            await checkbox.Click();
            await AssertNotSelected(checkbox);

            await checkbox.Click();
            await AssertSelected(checkbox);
        }

        [Test]
        public async Task ClickingOnASelectedRadioButtonShouldLeaveItSelected()
        {
            await driver.GoToUrl(formsPage);

            IWebElement button = await this.EnabledSelectedRadioButton();
            Assert.That(await button.Selected(), "Radio button should be selected");

            await button.Click();

            Assert.That(await button.Selected(), "Radio button should be selected");
        }

        [Test]
        public async Task ShouldBeAbleToToggleEnabledMultiSelectOption()
        {
            await driver.GoToUrl(formsPage);
            await AssertCanToggle(await this.SelectedMultipleSelectOption());
        }

        [Test]
        public async Task ShouldBeAbleToToggleSelectableCheckboxByClickingOnIt()
        {
            await driver.GoToUrl(formsPage);

            IWebElement checkbox = await this.EnabledUnselectedCheckbox();
            await AssertNotSelected(checkbox);

            await checkbox.Click();
            await AssertSelected(checkbox);

            await checkbox.Click();
            await AssertNotSelected(checkbox);
        }

        [Test]
        public async Task ShouldBeAbleToSelectSelectableRadioButtonByClickingOnIt()
        {
            await driver.GoToUrl(formsPage);

            IWebElement radioButton = await this.EnabledUnselectedRadioButton();
            await AssertNotSelected(radioButton);

            await radioButton.Click();
            await AssertSelected(radioButton);

            await radioButton.Click();
            await AssertSelected(radioButton);
        }

        [Test]
        public async Task ClickingDisabledSelectedCheckboxShouldBeNoop()
        {
            await driver.GoToUrl(formsPage);
            await AssertClickingPreservesCurrentlySelectedStatus(await this.RandomlyDisabledSelectedCheckbox());
        }

        [Test]
        public async Task ClickingDisabledUnselectedCheckboxShouldBeNoop()
        {
            await driver.GoToUrl(formsPage);
            await AssertClickingPreservesCurrentlySelectedStatus(await this.DisabledUnselectedCheckbox());
        }

        [Test]
        public async Task ClickingDisabledSelectedRadioButtonShouldBeNoop()
        {
            await driver.GoToUrl(formsPage);
            await AssertClickingPreservesCurrentlySelectedStatus(await this.DisabledSelectedRadioButton());
        }

        [Test]
        public async Task ClickingDisabledUnselectedRadioButtonShouldBeNoop()
        {
            await driver.GoToUrl(formsPage);
            await AssertClickingPreservesCurrentlySelectedStatus(await this.DisabledUnselectedRadioButton());
        }

        private static async Task AssertNotSelected(IWebElement element)
        {
            await AssertSelected(element, false);
        }

        private static async Task AssertSelected(IWebElement element)
        {
            await AssertSelected(element, true);
        }

        private static async Task AssertSelected(IWebElement element, bool isSelected)
        {
            Assert.AreEqual(isSelected, await element.Selected(), string.Format("Expected element {0} to be {1} but was {2}", Describe(element), SelectedToString(isSelected), SelectedToString(!isSelected)));
        }

        private static async Task AssertCannotSelect(IWebElement element)
        {
            bool previous = await element.Selected();
            await element.Click();
            Assert.AreEqual(previous, await element.Selected());
        }

        private static async Task AssertCanSelect(IWebElement element)
        {
            await AssertNotSelected(element);

            await element.Click();
            await AssertSelected(element);
        }

        private static async Task AssertClickingPreservesCurrentlySelectedStatus(IWebElement element)
        {
            bool currentSelectedStatus = await element.Selected();
            try {
                await element.Click();
            } catch (InvalidElementStateException) {
                // This is expected, as we are clicking disabled elements.
            }

            await AssertSelected(element, currentSelectedStatus);
        }

        private static string SelectedToString(bool isSelected)
        {
            return isSelected ? "[selected]" : "[not selected]";
        }

        private static Task<string> Describe(IWebElement element)
        {
            return element.GetAttribute("id");
        }

        private static async Task AssertCanToggle(IWebElement element)
        {
            bool originalState = await element.Selected();

            await AssertSelected(element, originalState);

            await AssertTogglingSwapsSelectedStateFrom(element, originalState);
            await AssertTogglingSwapsSelectedStateFrom(element, !originalState);
        }

        private static async Task AssertTogglingSwapsSelectedStateFrom(IWebElement element, bool originalState)
        {
            await element.Click();
            bool isNowSelected = await element.Selected();
            Assert.AreNotEqual(isNowSelected, originalState,
                $"Expected element {Describe(element)} to have been toggled to {SelectedToString(!originalState)} but was {SelectedToString(originalState)}");
            await AssertSelected(element, !originalState);
        }

        //TODO: Test disabled multi-selects
        //TODO: Test selecting options


        private Task<IWebElement> EnabledUnselectedCheckbox()
        {
            return driver.FindElement(By.Id("checky"));
        }

        private Task<IWebElement> EnabledSelectedCheckbox()
        {
            return driver.FindElement(By.Id("checkedchecky"));
        }

        private Task<IWebElement> DisabledUnselectedCheckbox()
        {
            return driver.FindElement(By.Id("disabledchecky"));
        }

        private Task<IWebElement> RandomlyDisabledSelectedCheckbox()
        {
            return driver.FindElement(By.Id("randomly_disabled_checky"));
        }

        private Task<IWebElement> EnabledUnselectedRadioButton()
        {
            return driver.FindElement(By.Id("peas"));
        }

        private Task<IWebElement> EnabledSelectedRadioButton()
        {
            return driver.FindElement(By.Id("cheese_and_peas"));
        }

        private Task<IWebElement> DisabledSelectedRadioButton()
        {
            return driver.FindElement(By.Id("lone_disabled_selected_radio"));
        }

        private Task<IWebElement> DisabledUnselectedRadioButton()
        {
            return driver.FindElement(By.Id("nothing"));
        }

        private Task<IWebElement> RandomlyDisabledUnselectedRadioButton()
        {
            return driver.FindElement(By.Id("randomly_disabled_nothing"));
        }

        private async Task<IWebElement> SelectedMultipleSelectOption()
        {
            IWebElement select = await driver.FindElement(By.Name("multi"));
            return (await select.FindElements(By.TagName("option")))[0];
        }

        private Task<IWebElement> NonSelectableElement()
        {
            return driver.FindElement(By.TagName("div"));
        }
    }
}
