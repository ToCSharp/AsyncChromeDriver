using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.BrowserOptions;
using Size = Zu.WebBrowser.BasicTypes.WebSize;
using Point = Zu.WebBrowser.BasicTypes.WebPoint;
namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class WindowTest : DriverTestFixture
    {
        private Size originalWindowSize;

        [SetUp]
        public async Task GetBrowserWindowSize()
        {
            this.originalWindowSize = await driver.Options().Window.GetSize();

        }

        [TearDown]
        public async Task RestoreBrowserWindow()
        {
            await driver.Options().Window.SetSize(originalWindowSize);
        }

        [Test]
        public async Task ShouldBeAbleToGetTheSizeOfTheCurrentWindow()
        {
            Size size = await driver.Options().Window.GetSize();
            Assert.That(size.Width, Is.GreaterThan(0));
            Assert.That(size.Height, Is.GreaterThan(0));
        }

        [Test]
        public async Task ShouldBeAbleToSetTheSizeOfTheCurrentWindow()
        {
            IWindow window = driver.Options().Window;
            Size size = await window.GetSize();

            // resize relative to the initial size, since we don't know what it is
            Size targetSize = new Size(size.Width - 20, size.Height - 20);
            await ChangeSizeBy(-20, -20);

            Size newSize = await window.GetSize();
            Assert.AreEqual(targetSize.Width, newSize.Width);
            Assert.AreEqual(targetSize.Height, newSize.Height);
        }

        [Test]
        public async Task ShouldBeAbleToSetTheSizeOfTheCurrentWindowFromFrame()
        {
            IWindow window = driver.Options().Window;
            Size size = await window.GetSize();
            await driver.GoToUrl(framesetPage);
            await driver.SwitchTo().Frame("fourth");

            try {
                // resize relative to the initial size, since we don't know what it is
                Size targetSize = new Size(size.Width - 20, size.Height - 20);
                await window.SetSize(targetSize);


                Size newSize = await window.GetSize();
                Assert.AreEqual(targetSize.Width, newSize.Width);
                Assert.AreEqual(targetSize.Height, newSize.Height);
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldBeAbleToSetTheSizeOfTheCurrentWindowFromIFrame()
        {
            IWindow window = driver.Options().Window;
            Size size = await window.GetSize();
            await driver.GoToUrl(iframePage);
            await driver.SwitchTo().Frame("iframe1-name");

            try {
                // resize relative to the initial size, since we don't know what it is
                Size targetSize = new Size(size.Width - 20, size.Height - 20);
                await window.SetSize(targetSize);


                Size newSize = await window.GetSize();
                Assert.AreEqual(targetSize.Width, newSize.Width);
                Assert.AreEqual(targetSize.Height, newSize.Height);
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldBeAbleToGetThePositionOfTheCurrentWindow()
        {
            Point position = await driver.Options().Window.GetPosition();
            Assert.That(position.X, Is.GreaterThan(0));
            Assert.That(position.Y, Is.GreaterThan(0));
        }

        [Test]
        public async Task ShouldBeAbleToSetThePositionOfTheCurrentWindow()
        {
            IWindow window = driver.Options().Window;
            await window.SetSize(new Size(200, 200));
            Point position = await window.GetPosition();

            Point targetPosition = new Point(position.X + 10, position.Y + 10);
            await window.SetPosition(targetPosition);

            Point newLocation = await window.GetPosition();

            Assert.AreEqual(targetPosition.X, newLocation.X);
            Assert.AreEqual(targetPosition.Y, newLocation.Y);
        }

        [Test]
        public async Task ShouldBeAbleToMaximizeTheCurrentWindow()
        {
            Size targetSize = new Size(640, 275);
            await ChangeSizeTo(targetSize);
            await Maximize();

            IWindow window = driver.Options().Window;
            Assert.That((await window.GetSize()).Height, Is.GreaterThan(targetSize.Height));
            Assert.That((await window.GetSize()).Width, Is.GreaterThan(targetSize.Width));
        }

        [Test]
        public async Task ShouldBeAbleToMaximizeTheWindowFromFrame()
        {
            await driver.GoToUrl(framesetPage);
            await ChangeSizeTo(new Size(640, 275));

            await driver.SwitchTo().Frame("fourth");
            try {
                await Maximize();
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        [Test]
        public async Task ShouldBeAbleToMaximizeTheWindowFromIframe()
        {
            await driver.GoToUrl(iframePage);
            await ChangeSizeTo(new Size(640, 275));

            await driver.SwitchTo().Frame("iframe1-name");
            try {
                await Maximize();
            } finally {
                await driver.SwitchTo().DefaultContent();
            }
        }

        //------------------------------------------------------------------
        // Tests below here are not included in the Java test suite
        //------------------------------------------------------------------

        //[Test]
        //[IgnoreBrowser(Browser.Edge, "Edge driver does not implement the full screen command")]
        //[IgnoreBrowser(Browser.Opera, "Not implemented in driver")]
        //public async Task ShouldBeAbleToFullScreenTheCurrentWindow()
        //{
        //    Size targetSize = new Size(640, 275);
        //    await ChangeSizeTo(targetSize);
        //    await FullScreen();

        //    IWindow window = driver.Options().Window;
        //    Size windowSize = await window.GetSize();
        //    Point windowPosition = await window.GetPosition();
        //    Assert.That(windowSize.Height, Is.GreaterThan(targetSize.Height));
        //    Assert.That(windowSize.Width, Is.GreaterThan(targetSize.Width));
        //}

        //[Test]
        //[IgnoreBrowser(Browser.Chrome, "Chrome window size does not report zero when minimized.")]
        //[IgnoreBrowser(Browser.Opera, "Not implemented in driver")]
        //public async Task ShouldBeAbleToMinimizeTheCurrentWindow()
        //{
        //    Size targetSize = new Size(640, 275);
        //    await ChangeSizeTo(targetSize);
        //    await Minimize();

        //    IWindow window = driver.Options().Window;
        //    Size windowSize = await window.GetSize();
        //    Point windowPosition = window.Position;
        //    Assert.That(windowSize.Height, Is.LessThan(targetSize.Height));
        //    Assert.That(windowSize.Width, Is.LessThan(targetSize.Width));
        //    Assert.That(windowPosition.X, Is.LessThan(0));
        //    Assert.That(windowPosition.Y, Is.LessThan(0));
        //}

        //private async Task FullScreen()
        //{
        //    IWindow window = driver.Options().Window;
        //    Size currentSize = await window.GetSize();
        //    await window.FullScreen();
        //}

        private async Task Maximize()
        {
            IWindow window = driver.Options().Window;
            Size currentSize = await window.GetSize();
            await window.Maximize();
            await WaitFor(WindowHeightToBeGreaterThan(currentSize.Height), "Window height was not greater than " + currentSize.Height);
            await WaitFor(WindowWidthToBeGreaterThan(currentSize.Width), "Window width was not greater than " + currentSize.Width);
        }

        //private async Task Minimize()
        //{
        //    IWindow window = driver.Options().Window;
        //    Size currentSize = await window.GetSize();
        //    await window.Minimize();
        //    WaitFor(WindowHeightToBeLessThan(currentSize.Height), "Window height was not less than " + currentSize.Height.ToString());
        //    WaitFor(WindowWidthToBeLessThan(currentSize.Width), "Window width was not less than " + currentSize.Width.ToString());
        //}

        private async Task ChangeSizeTo(Size targetSize)
        {
            IWindow window = driver.Options().Window;
            await window.SetSize(targetSize);
            await WaitFor(WindowHeightToBeEqualTo(targetSize.Height), "Window height was not " + targetSize.Height);
            await WaitFor(WindowWidthToBeEqualTo(targetSize.Width), "Window width was not " + targetSize.Width);
        }

        private async Task ChangeSizeBy(int deltaX, int deltaY)
        {
            IWindow window = driver.Options().Window;
            Size size = await window.GetSize();
            await ChangeSizeTo(new Size(size.Width + deltaX, size.Height + deltaY));
        }

        private Func<Task<bool>> WindowHeightToBeEqualTo(int height)
        {
            return async () => await driver.Options().Window.GetSize().Height() == height;
        }

        private Func<Task<bool>> WindowWidthToBeEqualTo(int width)
        {
            return async () => await driver.Options().Window.GetSize().Width() == width;
        }

        private Func<Task<bool>> WindowHeightToBeGreaterThan(int height)
        {
            return async () => await driver.Options().Window.GetSize().Height() > height;
        }

        private Func<Task<bool>> WindowWidthToBeGreaterThan(int width)
        {
            return async () => await driver.Options().Window.GetSize().Width() > width;
        }

        private Func<Task<bool>> WindowHeightToBeLessThan(int height)
        {
            return async () => await driver.Options().Window.GetSize().Height() < height;
        }

        private Func<Task<bool>> WindowWidthToBeLessThan(int width)
        {
            return async () => await driver.Options().Window.GetSize().Width() < width;
        }
    }
}
