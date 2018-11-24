using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class SlowLoadingPageTest : DriverTestFixture
    {
        private const long LoadTimeInSeconds = 3;

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldBlockUntilPageLoads()
        {
            DateTime start = DateTime.Now;
            await driver.GoToUrl(sleepingPage + "?time=" + LoadTimeInSeconds.ToString());
            DateTime now = DateTime.Now;
            double elapsedTime = now.Subtract(start).TotalSeconds;
            Assert.That(elapsedTime, Is.GreaterThanOrEqualTo(LoadTimeInSeconds));
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task ShouldBlockUntilIFramesAreLoaded()
        {
            DateTime start = DateTime.Now;
            await driver.GoToUrl(slowIframes);
            DateTime now = DateTime.Now;
            double elapsedTime = now.Subtract(start).TotalSeconds;
            Assert.That(elapsedTime, Is.GreaterThanOrEqualTo(LoadTimeInSeconds));
        }

        [Test]
        [NeedsFreshDriver(IsCreatedBeforeTest = true)]
        public async Task RefreshShouldBlockUntilPageLoads()
        {
            DateTime start = DateTime.Now;
            await driver.GoToUrl(sleepingPage + "?time=" + LoadTimeInSeconds.ToString());
            DateTime now = DateTime.Now;
            double elapsedTime = now.Subtract(start).TotalSeconds;
            Assert.That(elapsedTime, Is.GreaterThanOrEqualTo(LoadTimeInSeconds));
            start = DateTime.Now;
            await driver.Navigate().Refresh();
            now = DateTime.Now;
            elapsedTime = now.Subtract(start).TotalSeconds;
            Assert.That(elapsedTime, Is.GreaterThanOrEqualTo(LoadTimeInSeconds));
        }
    }
}
