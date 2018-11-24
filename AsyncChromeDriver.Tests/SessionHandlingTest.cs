using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncChromeDriver.Tests.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class SessionHandlingTest : DriverTestFixture
    {
        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task CallingQuitMoreThanOnceOnASessionIsANoOp()
        {
            await driver.GoToUrl(simpleTestPage);
            await driver.Quit();
            await driver.Quit();
            driver = EnvironmentManager.Instance.CreateDriverInstance();
            await driver.GoToUrl(xhtmlTestPage);
            await driver.Quit();
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task CallingQuitAfterClosingTheLastWindowIsANoOp()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            IWebDriver testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(simpleTestPage);
            await testDriver.Close();
            await testDriver.Quit();
            testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(xhtmlTestPage);
            Assert.AreEqual("XHTML Test Page", await testDriver.Title());
            await testDriver.Quit();
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task CallingAnyOperationAfterClosingTheLastWindowShouldThrowAnException()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            IWebDriver testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            try {
                string url = string.Empty;
                await testDriver.GoToUrl(simpleTestPage);
                await testDriver.Close();
                //Assert.That(async () => url = await testDriver.GetUrl(), Throws.InstanceOf<WebDriverException>().Or.InstanceOf<InvalidOperationException>(), "Should not be able to access Url property after close of only open window");
                await AssertEx.ThrowsAsync<WebBrowserException>(async () => await testDriver.GetUrl(),
                    exception => Assert.AreEqual("InvalidOperationException", exception.Error));
            } finally {
                testDriver.Dispose();
            }
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task CallingAnyOperationAfterQuitShouldThrowAnException()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            IWebDriver testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            try {
                string url = string.Empty;
                await testDriver.GoToUrl(simpleTestPage);
                await testDriver.Quit();
                //Assert.That(async () => url = await testDriver.GetUrl(), Throws.InstanceOf<WebDriverException>().Or.InstanceOf<InvalidOperationException>(), "Should not be able to access Url property after close of only open window");
                await AssertEx.ThrowsAsync<WebBrowserException>(async () => await testDriver.GetUrl());
            } finally {
                testDriver.Dispose();
            }
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldBeAbleToStartNewDriverAfterCallingCloseOnOnlyOpenWindow()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            IWebDriver testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(simpleTestPage);
            await testDriver.Close();
            testDriver.Dispose();
            testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(xhtmlTestPage);
            Assert.AreEqual("XHTML Test Page", await testDriver.Title());
            await testDriver.Close();
            testDriver.Dispose();
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldBeAbleToDisposeOfDriver()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            IWebDriver testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(simpleTestPage);
            testDriver.Dispose();
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldBeAbleToCallDisposeConsecutively()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            IWebDriver testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(simpleTestPage);
            testDriver.Dispose();
            testDriver.Dispose();
        }

        [Test]
        [NeedsFreshDriver(IsCreatedAfterTest = true)]
        public async Task ShouldBeAbleToCallDisposeAfterQuit()
        {
            EnvironmentManager.Instance.CloseCurrentDriver();
            IWebDriver testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(simpleTestPage);
            await testDriver.Quit();
            testDriver.Dispose();
            testDriver = EnvironmentManager.Instance.CreateDriverInstance();
            await testDriver.GoToUrl(xhtmlTestPage);
            Assert.AreEqual("XHTML Test Page", await testDriver.Title());
            await testDriver.Quit();
        }

        [Test]
        public async Task ShouldOpenAndCloseBrowserRepeatedly()
        {
            for (int i = 0; i < 5; i++) {
                EnvironmentManager.Instance.CloseCurrentDriver();
                CreateFreshDriver();
                await driver.GoToUrl(simpleTestPage);
                Assert.AreEqual(simpleTestTitle, await driver.Title());
            }
        }
    }
}
