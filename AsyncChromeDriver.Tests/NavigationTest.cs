using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace OpenQA.Selenium
{

    [TestFixture]
    public class NavigationTest : DriverTestFixture
    {

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldNotHaveProblemNavigatingWithNoPagesBrowsed()
        {
            INavigation navigation;
            navigation = driver.Navigate();
            await navigation.Back();
            await navigation.Forward();
        }

        [Test]
        public async Task ShouldGoBackAndForward()
        {
            INavigation navigation;
            navigation = driver.Navigate();

            await driver.GoToUrl(macbethPage);
            await driver.GoToUrl(simpleTestPage);
            await navigation.Back();
            Assert.AreEqual(macbethTitle, await driver.Title());
            await navigation.Forward();
            Assert.AreEqual(simpleTestTitle, await driver.Title());
        }

        [Test]
        public async Task ShouldAcceptInvalidUrlsUsingUris()
        {
            INavigation navigation;
            navigation = driver.Navigate();
            //Assert.That(async () => await navigation.GoToUrl((Uri)null), Throws.InstanceOf<ArgumentNullException>());
            await AssertEx.ThrowsAsync<WebBrowserException>(async () => await navigation.GoToUrl((Uri)null),
                exception => Assert.AreEqual("ArgumentNullException", exception.Error));
            // new Uri("") and new Uri("isidsji30342??éåµñ©æ") 
            // throw an exception, so we needn't worry about them.
        }

        [Test]
        public async Task ShouldGoToUrlUsingString()
        {
            INavigation navigation;
            navigation = driver.Navigate();
            await navigation.GoToUrl(macbethPage);
            Assert.AreEqual(macbethTitle, await driver.Title());
            await // We go to two pages to ensure that the browser wasn't
                  // already at the desired page through a previous test.
                       navigation.GoToUrl(simpleTestPage);
            Assert.AreEqual(simpleTestTitle, await driver.Title());
        }

        [Test]
        public async Task ShouldGoToUrlUsingUri()
        {
            Uri macBeth = new Uri(macbethPage);
            Uri simpleTest = new Uri(simpleTestPage);
            INavigation navigation;
            navigation = driver.Navigate();
            await navigation.GoToUrl(macBeth);
            Assert.AreEqual(await driver.Title(), macbethTitle);
            await // We go to two pages to ensure that the browser wasn't
                  // already at the desired page through a previous test.
                       navigation.GoToUrl(simpleTest);
            Assert.AreEqual(simpleTestTitle, await driver.Title());
        }

        [Test]
        public async Task ShouldRefreshPage()
        {
            await driver.GoToUrl(javascriptPage);
            IWebElement changedDiv = await driver.FindElement(By.Id("dynamo"));
            await driver.FindElement(By.Id("updatediv")).Click();

            Assert.AreEqual("Fish and chips!", await changedDiv.Text());
            await driver.Navigate().Refresh();

            changedDiv = await driver.FindElement(By.Id("dynamo"));
            Assert.AreEqual("What's for dinner?", await changedDiv.Text());
        }

    }
}
