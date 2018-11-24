using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class FormHandlingTests : DriverTestFixture
    {
        [Test]
        public async Task ShouldClickOnSubmitInputElements()
        {
            await driver.GoToUrl(formsPage);
            await driver.FindElement(By.Id("submitButton")).Click();
            await WaitFor(TitleToBe("We Arrive Here"), "Browser title is not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ClickingOnUnclickableElementsDoesNothing()
        {
            await driver.GoToUrl(formsPage);
            await driver.FindElement(By.XPath("//body")).Click();
        }

        [Test]
        public async Task ShouldBeAbleToClickImageButtons()
        {
            await driver.GoToUrl(formsPage);
            await driver.FindElement(By.Id("imageButton")).Click();
            await WaitFor(TitleToBe("We Arrive Here"), "Browser title is not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldBeAbleToSubmitForms()
        {
            await driver.GoToUrl(formsPage);
            await driver.FindElement(By.Name("login")).Submit();
            await WaitFor(TitleToBe("We Arrive Here"), "Browser title is not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldSubmitAFormWhenAnyInputElementWithinThatFormIsSubmitted()
        {
            await driver.GoToUrl(formsPage);
            await driver.FindElement(By.Id("checky")).Submit();
            await WaitFor(TitleToBe("We Arrive Here"), "Browser title is not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldSubmitAFormWhenAnyElementWithinThatFormIsSubmitted()
        {
            await driver.GoToUrl(formsPage);
            await driver.FindElement(By.XPath("//form/p")).Submit();
            await WaitFor(TitleToBe("We Arrive Here"), "Browser title is not 'We Arrive Here'");
            Assert.AreEqual(await driver.Title(), "We Arrive Here");
        }

        [Test]
        public async Task ShouldNotBeAbleToSubmitAFormThatDoesNotExist()
        {
            await driver.GoToUrl(formsPage);
            //Assert.That(async () => await driver.FindElement(By.Name("SearchableText")).Submit(), Throws.InstanceOf<NoSuchElementException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await driver.FindElement(By.Name("SearchableText")).Submit(),
                exception => Assert.AreEqual("no such element", exception.Error));
        }

        [Test]
        public async Task ShouldBeAbleToEnterTextIntoATextAreaBySettingItsValue()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement textarea = await driver.FindElement(By.Id("keyUpArea"));
            string cheesey = "Brie and cheddar";
            await textarea.SendKeys(cheesey);
            Assert.AreEqual(await textarea.GetAttribute("value"), cheesey);
        }

        [Test]
        public async Task SendKeysKeepsCapitalization()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement textarea = await driver.FindElement(By.Id("keyUpArea"));
            string cheesey = "BrIe And CheDdar";
            await textarea.SendKeys(cheesey);
            Assert.AreEqual(await textarea.GetAttribute("value"), cheesey);
        }

        [Test]
        public async Task ShouldSubmitAFormUsingTheNewlineLiteral()
        {
            await driver.GoToUrl(formsPage);
            IWebElement nestedForm = await driver.FindElement(By.Id("nested_form"));
            IWebElement input = await nestedForm.FindElement(By.Name("x"));
            await input.SendKeys("\n");

            await WaitFor(TitleToBe("We Arrive Here"), "Browser title is not 'We Arrive Here'");
            Assert.AreEqual("We Arrive Here", await driver.Title());
            Assert.That(await driver.GetUrl(), Does.EndWith("?x=name"));
        }

        [Test]
        public async Task ShouldSubmitAFormUsingTheEnterKey()
        {
            await driver.GoToUrl(formsPage);
            IWebElement nestedForm = await driver.FindElement(By.Id("nested_form"));
            IWebElement input = await nestedForm.FindElement(By.Name("x"));
            await input.SendKeys(Keys.Enter);

            await WaitFor(TitleToBe("We Arrive Here"), "Browser title is not 'We Arrive Here'");
            Assert.AreEqual("We Arrive Here", await driver.Title());
            Assert.That(await driver.GetUrl(), Does.EndWith("?x=name"));
        }

        [Test]
        public async Task ShouldEnterDataIntoFormFields()
        {
            await driver.GoToUrl(xhtmlTestPage);
            IWebElement element = await driver.FindElement(By.XPath("//form[@name='someForm']/input[@id='username']"));
            String originalValue = await element.GetAttribute("value");
            Assert.AreEqual(originalValue, "change");
            await element.Clear();
            await element.SendKeys("some text");

            element = await driver.FindElement(By.XPath("//form[@name='someForm']/input[@id='username']"));
            String newFormValue = await element.GetAttribute("value");
            Assert.AreEqual(newFormValue, "some text");
        }

        [Test]
        public async Task ShouldBeAbleToAlterTheContentsOfAFileUploadInputElement()
        {
            string testFileName = string.Format("test-{0}.txt", Guid.NewGuid().ToString("D"));
            await driver.GoToUrl(formsPage);
            IWebElement uploadElement = await driver.FindElement(By.Id("upload"));
            Assert.That(await uploadElement.GetAttribute("value"), Is.Null.Or.EqualTo(string.Empty));

            string filePath = System.IO.Path.Combine(EnvironmentManager.Instance.CurrentDirectory, testFileName);
            System.IO.FileInfo inputFile = new System.IO.FileInfo(filePath);
            System.IO.StreamWriter inputFileWriter = inputFile.CreateText();
            inputFileWriter.WriteLine("Hello world");
            inputFileWriter.Close();
            await uploadElement.SendKeys(inputFile.FullName);

            System.IO.FileInfo outputFile = new System.IO.FileInfo(await uploadElement.GetAttribute("value"));
            Assert.AreEqual(inputFile.Name, outputFile.Name);
            inputFile.Delete();
        }

        [Test]
        public async Task ShouldBeAbleToSendKeysToAFileUploadInputElementInAnXhtmlDocument()
        {
            await driver.GoToUrl(xhtmlFormPage);
            IWebElement uploadElement = await driver.FindElement(By.Id("file"));
            Assert.AreEqual(string.Empty, uploadElement.GetAttribute("value"));

            string testFileName = string.Format("test-{0}.txt", Guid.NewGuid().ToString("D"));
            string filePath = System.IO.Path.Combine(EnvironmentManager.Instance.CurrentDirectory, testFileName);
            System.IO.FileInfo inputFile = new System.IO.FileInfo(filePath);
            System.IO.StreamWriter inputFileWriter = inputFile.CreateText();
            inputFileWriter.WriteLine("Hello world");
            inputFileWriter.Close();
            await uploadElement.SendKeys(inputFile.FullName);

            System.IO.FileInfo outputFile = new System.IO.FileInfo(await uploadElement.GetAttribute("value"));
            Assert.AreEqual(inputFile.Name, outputFile.Name);
            inputFile.Delete();
        }

        [Test]
        public async Task ShouldBeAbleToUploadTheSameFileTwice()
        {
            string testFileName = $"test-{Guid.NewGuid():D}.txt";
            string filePath = System.IO.Path.Combine(EnvironmentManager.Instance.CurrentDirectory, testFileName);
            System.IO.FileInfo inputFile = new System.IO.FileInfo(filePath);
            System.IO.StreamWriter inputFileWriter = inputFile.CreateText();
            inputFileWriter.WriteLine("Hello world");
            inputFileWriter.Close();

            for (int i = 0; i < 2; ++i) {
                await driver.GoToUrl(formsPage);
                IWebElement uploadElement = await driver.FindElement(By.Id("upload"));
                Assert.That(await uploadElement.GetAttribute("value"), Is.Null.Or.EqualTo(string.Empty));
                await uploadElement.SendKeys(inputFile.FullName);
                await uploadElement.Submit();
            }

            inputFile.Delete();
            // If we get this far, then we're all good.
        }

        [Test]
        public async Task SendingKeyboardEventsShouldAppendTextInInputs()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("working"));
            await element.SendKeys("Some");
            String value = await element.GetAttribute("value");
            Assert.AreEqual(value, "Some");
            await element.SendKeys(" text");
            value = await element.GetAttribute("value");
            Assert.AreEqual(value, "Some text");
        }

        [Test]
        public async Task SendingKeyboardEventsShouldAppendTextInInputsWithExistingValue()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("inputWithText"));
            await element.SendKeys(". Some text");
            string value = await element.GetAttribute("value");

            Assert.AreEqual("Example text. Some text", value);
        }

        [Test]
        public async Task SendingKeyboardEventsShouldAppendTextInTextAreas()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("withText"));
            await element.SendKeys(". Some text");
            String value = await element.GetAttribute("value");

            Assert.AreEqual(value, "Example text. Some text");
        }

        [Test]
        public async Task EmptyTextBoxesShouldReturnAnEmptyStringNotNull()
        {
            await driver.GoToUrl(formsPage);
            IWebElement emptyTextBox = await driver.FindElement(By.Id("working"));
            Assert.AreEqual(await emptyTextBox.GetAttribute("value"), "");

            IWebElement emptyTextArea = await driver.FindElement(By.Id("emptyTextArea"));
            Assert.AreEqual(await emptyTextBox.GetAttribute("value"), "");
        }

        [Test]
        public async Task HandleFormWithJavascriptAction()
        {
            string url = EnvironmentManager.Instance.UrlBuilder.WhereIs("form_handling_js_submit.html");
            await driver.GoToUrl(url);
            IWebElement element = await driver.FindElement(By.Id("theForm"));
            await element.Submit();
            IAlert alert = await WaitFor(async () => {
                try {
                    return await driver.SwitchTo().Alert();
                } catch (NoAlertPresentException) {
                    return null;
                }
            }, "No alert found before timeout.");

            string text = await alert.Text();
            await alert.Dismiss();

            Assert.AreEqual("Tasty cheese", text);
        }

        [Test]
        public async Task CanClickOnASubmitButton()
        {
            await CheckSubmitButton("internal_explicit_submit");
        }

        [Test]
        public async Task CanClickOnASubmitButtonNestedSpan()
        {
            await CheckSubmitButton("internal_span_submit");
        }

        [Test]
        public async Task CanClickOnAnImplicitSubmitButton()
        {
            await CheckSubmitButton("internal_implicit_submit");
        }

        [Test]
        public async Task CanClickOnAnExternalSubmitButton()
        {
            await CheckSubmitButton("external_explicit_submit");
        }

        [Test]
        public async Task CanClickOnAnExternalImplicitSubmitButton()
        {
            await CheckSubmitButton("external_implicit_submit");
        }

        [Test]
        public async Task CanSubmitFormWithSubmitButtonIdEqualToSubmit()
        {
            string blank = EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithTitle("Submitted Successfully!"));
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                 .WithBody(string.Format("<form action='{0}'>", blank),
                 "  <input type='submit' id='submit' value='Submit'>",
                 "</form>")));

            await driver.FindElement(By.Id("submit")).Submit();
            await WaitFor(TitleToBe("Submitted Successfully!"), "Title was not expected value");
        }

        [Test]
        public async Task CanSubmitFormWithSubmitButtonNameEqualToSubmit()
        {
            string blank = EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                .WithTitle("Submitted Successfully!"));
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.CreateInlinePage(new InlinePage()
                 .WithBody(string.Format("<form action='{0}'>", blank),
                 "  <input type='submit' name='submit' value='Submit'>",
                 "</form>")));

            await driver.FindElement(By.Name("submit")).Submit();
            await WaitFor(TitleToBe("Submitted Successfully!"), "Title was not expected value");
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldBeAbleToClearTextFromInputElements()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("working"));
            await element.SendKeys("Some text");
            String value = await element.GetAttribute("value");
            Assert.That(value.Length, Is.GreaterThan(0));
            await element.Clear();
            value = await element.GetAttribute("value");

            Assert.That(value.Length, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldBeAbleToClearTextFromTextAreas()
        {
            await driver.GoToUrl(formsPage);
            IWebElement element = await driver.FindElement(By.Id("withText"));
            await element.SendKeys("Some text");
            String value = await element.GetAttribute("value");
            Assert.That(value.Length, Is.GreaterThan(0));
            await element.Clear();
            value = await element.GetAttribute("value");

            Assert.That(value.Length, Is.EqualTo(0));
        }

        private async Task CheckSubmitButton(string buttonId)
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("click_tests/html5_submit_buttons.html"));
            string name = "Gromit";

            await driver.FindElement(By.Id("name")).SendKeys(name);
            await driver.FindElement(By.Id(buttonId)).Click();

            await WaitFor(TitleToBe("Submitted Successfully!"), "Browser title is not 'Submitted Successfully!'");

            Assert.That((await driver.GetUrl()).Contains("name=" + name), "URL does not contain 'name=" + name + "'. Actual URL:" + await driver.GetUrl());
        }

        private Func<Task<bool>> TitleToBe(string desiredTitle)
        {
            return async () => await driver.Title() == desiredTitle;
        }
    }
}
