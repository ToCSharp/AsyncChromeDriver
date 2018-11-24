using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Environment;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Size = Zu.WebBrowser.BasicTypes.WebSize;
using Point = Zu.WebBrowser.BasicTypes.WebPoint;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class PositionAndSizeTest : DriverTestFixture
    {
        [Test]
        public async Task ShouldBeAbleToDetermineTheLocationOfAnElement()
        {
            await driver.GoToUrl(xhtmlTestPage);

            IWebElement element = await driver.FindElement(By.Id("username"));
            Point location = await element.Location();

            Assert.That(location.X, Is.GreaterThan(0));
            Assert.That(location.Y, Is.GreaterThan(0));
        }

        [Test]
        public async Task ShouldGetCoordinatesOfAnElement()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/simple_page.html"));
            Assert.AreEqual(new Point(10, 10), await GetLocationInViewPort(By.Id("box")));
            Assert.AreEqual(new Point(10, 10), await GetLocationOnPage(By.Id("box")));
        }

        [Test]
        public async Task ShouldGetCoordinatesOfAnEmptyElement()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_empty_element.html"));
            Assert.AreEqual(new Point(10, 10), await GetLocationInViewPort(By.Id("box")));
            Assert.AreEqual(new Point(10, 10), await GetLocationOnPage(By.Id("box")));
        }

        [Test]
        public async Task ShouldGetCoordinatesOfATransparentElement()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_transparent_element.html"));
            Assert.AreEqual(new Point(10, 10), await GetLocationInViewPort(By.Id("box")));
            Assert.AreEqual(new Point(10, 10), await GetLocationOnPage(By.Id("box")));
        }

        [Test]
        public async Task ShouldGetCoordinatesOfAHiddenElement()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_hidden_element.html"));
            Assert.AreEqual(new Point(10, 10), await GetLocationInViewPort(By.Id("box")));
            Assert.AreEqual(new Point(10, 10), await GetLocationOnPage(By.Id("box")));
        }

        [Test]
        public async Task ShouldGetCoordinatesOfAnInvisibleElement()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_invisible_element.html"));
            Assert.AreEqual(new Point(0, 0), await GetLocationInViewPort(By.Id("box")));
            Assert.AreEqual(new Point(0, 0), await GetLocationOnPage(By.Id("box")));
        }

        [Test]
        public async Task ShouldScrollPageAndGetCoordinatesOfAnElementThatIsOutOfViewPort()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_element_out_of_view.html"));
            int windowHeight = await driver.Options().Window.GetSize().Height();
            Point location = await GetLocationInViewPort(By.Id("box"));
            Assert.That(location.X, Is.EqualTo(10));
            Assert.That(location.Y, Is.GreaterThanOrEqualTo(0));
            Assert.That(await GetLocationOnPage(By.Id("box")), Is.EqualTo(new Point(10, 5010)));
            // GetLocationInViewPort only works within the context of a single frame
            // for W3C-spec compliant remote ends.
            // Assert.That(location.Y, Is.LessThanOrEqualTo(windowHeight - 100));
        }

        [Test]
        public async Task ShouldGetCoordinatesOfAnElementInAFrame()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/element_in_frame.html"));
            await driver.SwitchTo().Frame("ifr");
            IWebElement box = await driver.FindElement(By.Id("box"));
            Assert.AreEqual(new Point(10, 10), await box.Location());
            Assert.AreEqual(new Point(10, 10), await GetLocationOnPage(By.Id("box")));
        }

        [Test]
        public async Task ShouldGetCoordinatesInViewPortOfAnElementInAFrame()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/element_in_frame.html"));
            await driver.SwitchTo().Frame("ifr");
            Assert.AreEqual(new Point(10, 10), await GetLocationOnPage(By.Id("box")));
            // GetLocationInViewPort only works within the context of a single frame
            // for W3C-spec compliant remote ends.
            // Assert.AreEqual(new Point(25, 25), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        public async Task ShouldGetCoordinatesInViewPortOfAnElementInANestedFrame()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/element_in_nested_frame.html"));
            await driver.SwitchTo().Frame("ifr");
            await driver.SwitchTo().Frame("ifr");
            Assert.AreEqual(new Point(10, 10), await GetLocationOnPage(By.Id("box")));
            // GetLocationInViewPort only works within the context of a single frame
            // for W3C-spec compliant remote ends.
            // Assert.AreEqual(new Point(40, 40), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        public async Task ShouldGetCoordinatesOfAnElementWithFixedPosition()
        {
            await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_fixed_element.html"));
            Assert.That((await GetLocationInViewPort(By.Id("fixed"))).Y, Is.EqualTo(0));
            Assert.That((await GetLocationOnPage(By.Id("fixed"))).Y, Is.EqualTo(0));

            await driver.FindElement(By.Id("bottom")).Click();
            Assert.That((await GetLocationInViewPort(By.Id("fixed"))).Y, Is.EqualTo(0));
            Assert.That((await GetLocationOnPage(By.Id("fixed"))).Y, Is.GreaterThan(0));
        }

        [Test]
        public async Task ShouldCorrectlyIdentifyThatAnElementHasWidthAndHeight()
        {
            await driver.GoToUrl(xhtmlTestPage);

            IWebElement shrinko = await driver.FindElement(By.Id("linkId"));
            Size size = await shrinko.Size();
            Assert.That(size.Width, Is.GreaterThan(0), "Width expected to be greater than 0");
            Assert.That(size.Height, Is.GreaterThan(0), "Height expected to be greater than 0");
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------
        [Test]
        public async Task ShouldBeAbleToDetermineTheSizeOfAnElement()
        {
            await driver.GoToUrl(xhtmlTestPage);

            IWebElement element = await driver.FindElement(By.Id("username"));
            Size size = await element.Size();

            Assert.That(size.Width, Is.GreaterThan(0));
            Assert.That(size.Height, Is.GreaterThan(0));
        }

        private async Task<Point> GetLocationInViewPort(By locator)
        {
            IWebElement element = await driver.FindElement(locator);
            return await ((ILocatable)element).Coordinates.LocationInViewport();
        }

        private async Task<Point> GetLocationOnPage(By locator)
        {
            IWebElement element = await driver.FindElement(locator);
            return await ((ILocatable)element).Coordinates.LocationInDom();
        }
    }
}
