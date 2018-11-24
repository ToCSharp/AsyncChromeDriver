using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class UploadTest : DriverTestFixture
    {
        private const string LoremIpsumText = "lorem ipsum dolor sit amet";
        private const string FileHtml = "<div>" + LoremIpsumText + "</div>";
        private System.IO.FileInfo testFile;

        [OneTimeSetUp]
        public void Setup()
        {
            CreateTempFile(FileHtml);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (testFile != null && testFile.Exists) {
                testFile.Delete();
            }
        }

        [Test]
        public async Task ShouldAllowFileUploading()
        {
            await driver.GoToUrl(uploadPage);
            await driver.FindElement(By.Id("upload")).SendKeys(testFile.FullName);
            await driver.FindElement(By.Id("go")).Submit();

            await driver.SwitchTo().Frame("upload_target");

            IWebElement body = null;
            await WaitFor(async () => {
                body = await driver.FindElement(By.CssSelector("body"));
                return LoremIpsumText == await body.Text();
            }, "Page source is: " + await driver.PageSource());
            Assert.That(await body.Text(), Is.EqualTo(LoremIpsumText), "Page source is: " + await driver.PageSource());
        }

        [Test]
        public async Task CleanFileInput()
        {
            await driver.GoToUrl(uploadPage);
            IWebElement element = await driver.FindElement(By.Id("upload"));
            await element.SendKeys(testFile.FullName);
            await element.Clear();
            Assert.AreEqual(string.Empty, await element.GetAttribute("value"));
        }

        [Test]
        public async Task UploadingWithHiddenFileInput()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("upload_invisible.html"));
            await driver.FindElement(By.Id("upload")).SendKeys(testFile.FullName);
            await driver.FindElement(By.Id("go")).Click();

            // Uploading files across a network may take a while, even if they're really small
            IWebElement label = await driver.FindElement(By.Id("upload_label"));
            await driver.SwitchTo().Frame("upload_target");

            IWebElement body = null;
            await WaitFor(async () => {
                body = await driver.FindElement(By.XPath("//body"));
                return LoremIpsumText == await body.Text();
            }, "Page source is: " + await driver.PageSource());
            Assert.That(await body.Text(), Is.EqualTo(LoremIpsumText), "Page source is: " + await driver.PageSource());

        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldAllowFileUploadingUsingTransparentUploadElement()
        {
            await driver.GoToUrl(transparentUploadPage);
            await driver.FindElement(By.Id("upload")).SendKeys(testFile.FullName);
            await driver.FindElement(By.Id("go")).Submit();

            await driver.SwitchTo().Frame("upload_target");

            IWebElement body = null;
            await WaitFor(async () => {
                body = await driver.FindElement(By.XPath("//body"));
                return LoremIpsumText == await body.Text();
            }, "Page source is: " + await driver.PageSource());
            Assert.That(await body.Text(), Is.EqualTo(LoremIpsumText), "Page source is: " + await driver.PageSource());
            await driver.GoToUrl("about:blank");
        }

        private void CreateTempFile(string content)
        {
            string testFileName = System.IO.Path.Combine(EnvironmentManager.Instance.CurrentDirectory, "webdriver.tmp");
            testFile = new System.IO.FileInfo(testFileName);
            if (testFile.Exists) {
                testFile.Delete();
            }
            System.IO.StreamWriter testFileWriter = testFile.CreateText();
            testFileWriter.WriteLine(content);
            testFileWriter.Close();
        }
    }
}
