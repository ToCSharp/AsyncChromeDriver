using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncChromeDriver.Tests
{
    [TestFixture]
    public class TakesScreenshotTest : DriverTestFixture
    {
        [TearDown]
        public async Task SwitchToTop()
        {
            await driver.SwitchTo().DefaultContent();
        }

        //[Test]
        //public async Task GetScreenshotAsFile()
        //{
        //    ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
        //    if (screenshotCapableDriver == null) {
        //        return;
        //    }

        //    await driver.GoToUrl(simpleTestPage);

        //    string filename = Path.Combine(Path.GetTempPath(), "snapshot" + new Random().Next().ToString() + ".png");
        //    Screenshot screenImage = await screenshotCapableDriver.GetScreenshot();
        //    screenImage.SaveAsFile(filename, ScreenshotImageFormat.Png);
        //    Assert.That(File.Exists(filename), Is.True);
        //    Assert.That(new FileInfo(filename).Length, Is.GreaterThan(0));
        //    File.Delete(filename);
        //}

        [Test]
        public async Task GetScreenshotAsBase64()
        {
            ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
            if (screenshotCapableDriver == null) {
                return;
            }

            await driver.GoToUrl(simpleTestPage);
            Screenshot screenImage = await screenshotCapableDriver.GetScreenshot();
            string base64 = screenImage.AsBase64EncodedString;
            Assert.That(base64.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetScreenshotAsBinary()
        {
            ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
            if (screenshotCapableDriver == null) {
                return;
            }

            await driver.GoToUrl(simpleTestPage);
            Screenshot screenImage = await screenshotCapableDriver.GetScreenshot();
            byte[] bytes = screenImage.AsByteArray;
            Assert.That(bytes.Length, Is.GreaterThan(0));
        }

        //[Test]
        //public async Task ShouldCaptureScreenshotOfCurrentViewport()
        //{
        //    ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
        //    if (screenshotCapableDriver == null) {
        //        return;
        //    }

        //    await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("screen/screen.html"));
        //    Screenshot screenshot = await screenshotCapableDriver.GetScreenshot();

        //    HashSet<string> actualColors = ScanActualColors(screenshot,
        //                                                /* stepX in pixels */ 5,
        //                                                /* stepY in pixels */ 5);

        //    HashSet<string> expectedColors = GenerateExpectedColors( /* initial color */ 0x0F0F0F,
        //                                                         /* color step */ 1000,
        //                                                         /* grid X size */ 6,
        //                                                         /* grid Y size */ 6);
        //    await CompareColors(expectedColors, actualColors);
        //}

        //[Test]
        //public async Task ShouldTakeScreenshotsOfAnElement()
        //{
        //    await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("screen/screen.html"));
        //    IWebElement element = await driver.FindElement(By.Id("cell11"));

        //    ITakesScreenshot screenshotCapableElement = element as ITakesScreenshot;
        //    if (screenshotCapableElement == null) {
        //        return;
        //    }

        //    Screenshot screenImage = await screenshotCapableElement.GetScreenshot();
        //    byte[] imageData = screenImage.AsByteArray;
        //    Assert.That(imageData, Is.Not.Null);
        //    Assert.That(imageData.Length, Is.GreaterThan(0));

        //    Color pixelColor = GetPixelColor(screenImage, 1, 1);
        //    string pixelColorString = FormatColorToHex(pixelColor.ToArgb());
        //    Assert.AreEqual("#0f12f7", pixelColorString);
        //}

   

        //[Test]
        //public async Task ShouldCaptureScreenshotAtFramePage()
        //{
        //    ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
        //    if (screenshotCapableDriver == null) {
        //        return;
        //    }

        //    await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("screen/screen_frames.html"));
        //    await WaitFor(FrameToBeAvailableAndSwitchedTo("frame1"), "Did not switch to frame1");
        //    await WaitFor(ElementToBeVisibleWithId("content"), "Did not find visible element with id content");

        //    await driver.SwitchTo().DefaultContent();
        //    await WaitFor(FrameToBeAvailableAndSwitchedTo("frame2"), "Did not switch to frame2");
        //    await WaitFor(ElementToBeVisibleWithId("content"), "Did not find visible element with id content");

        //    await driver.SwitchTo().DefaultContent();
        //    await WaitFor(TitleToBe("screen test"), "Title was not expected value");
        //    Screenshot screenshot = await screenshotCapableDriver.GetScreenshot();

        //    HashSet<string> actualColors = ScanActualColors(screenshot,
        //                                               /* stepX in pixels */ 5,
        //                                               /* stepY in pixels */ 5);

        //    HashSet<string> expectedColors = GenerateExpectedColors( /* initial color */ 0x0F0F0F,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6);
        //    expectedColors.UnionWith(GenerateExpectedColors( /* initial color */ 0xDFDFDF,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6));
        //    await // expectation is that screenshot at page with frames will be taken for full page
        //               CompareColors(expectedColors, actualColors);
        //}

        //[Test]
        //public async Task ShouldCaptureScreenshotAtIFramePage()
        //{
        //    ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
        //    if (screenshotCapableDriver == null) {
        //        return;
        //    }

        //    await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("screen/screen_iframes.html"));

        //    Screenshot screenshot = await screenshotCapableDriver.GetScreenshot();

        //    HashSet<string> actualColors = ScanActualColors(screenshot,
        //                                               /* stepX in pixels */ 5,
        //                                               /* stepY in pixels */ 5);

        //    HashSet<string> expectedColors = GenerateExpectedColors( /* initial color */ 0x0F0F0F,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6);
        //    expectedColors.UnionWith(GenerateExpectedColors( /* initial color */ 0xDFDFDF,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6));
        //    await // expectation is that screenshot at page with Iframes will be taken for full page
        //               CompareColors(expectedColors, actualColors);
        //}

        //[Test]
        //public async Task ShouldCaptureScreenshotAtFramePageAfterSwitching()
        //{
        //    ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
        //    if (screenshotCapableDriver == null) {
        //        return;
        //    }

        //    await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("screen/screen_frames.html"));

        //    await driver.SwitchTo().Frame(await driver.FindElement(By.Id("frame2")));

        //    Screenshot screenshot = await screenshotCapableDriver.GetScreenshot();

        //    HashSet<string> actualColors = ScanActualColors(screenshot,
        //                                               /* stepX in pixels */ 5,
        //                                               /* stepY in pixels */ 5);

        //    HashSet<string> expectedColors = GenerateExpectedColors( /* initial color */ 0x0F0F0F,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6);
        //    expectedColors.UnionWith(GenerateExpectedColors( /* initial color */ 0xDFDFDF,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6));
        //    await // expectation is that screenshot at page with frames after switching to a frame
        //          // will be taken for full page
        //               CompareColors(expectedColors, actualColors);
        //}

        //[Test]
        //public async Task ShouldCaptureScreenshotAtIFramePageAfterSwitching()
        //{
        //    ITakesScreenshot screenshotCapableDriver = driver as ITakesScreenshot;
        //    if (screenshotCapableDriver == null) {
        //        return;
        //    }

        //    await driver.GoToUrl(EnvironmentManager.Instance.UrlBuilder.WhereIs("screen/screen_iframes.html"));

        //    await driver.SwitchTo().Frame(await driver.FindElement(By.Id("iframe2")));

        //    Screenshot screenshot = await screenshotCapableDriver.GetScreenshot();

        //    HashSet<string> actualColors = ScanActualColors(screenshot,
        //                                               /* stepX in pixels */ 5,
        //                                               /* stepY in pixels */ 5);

        //    HashSet<string> expectedColors = GenerateExpectedColors( /* initial color */ 0x0F0F0F,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6);
        //    expectedColors.UnionWith(GenerateExpectedColors( /* initial color */ 0xDFDFDF,
        //                                             /* color step*/ 1000,
        //                                             /* grid X size */ 6,
        //                                             /* grid Y size */ 6));
        //    await // expectation is that screenshot at page with Iframes after switching to a Iframe
        //          // will be taken for full page
        //               CompareColors(expectedColors, actualColors);
        //}

        private string FormatColorToHex(int colorValue)
        {
            string pixelColorString = string.Format("#{0:x2}{1:x2}{2:x2}", (colorValue & 0xFF0000) >> 16, (colorValue & 0x00FF00) >> 8, (colorValue & 0x0000FF));
            return pixelColorString;
        }

        private async Task CompareColors(HashSet<string> expectedColors, HashSet<string> actualColors)
        {
            // Ignore black and white for further comparison
            actualColors.Remove("#000000");
            actualColors.Remove("#ffffff");

            Assert.That(actualColors, Is.EquivalentTo(expectedColors));
        }

        private HashSet<string> GenerateExpectedColors(int initialColor, int stepColor, int numberOfSamplesX, int numberOfSamplesY)
        {
            HashSet<string> colors = new HashSet<string>();
            int count = 1;
            for (int i = 1; i < numberOfSamplesX; i++) {
                for (int j = 1; j < numberOfSamplesY; j++) {
                    int color = initialColor + (count * stepColor);
                    string hex = FormatColorToHex(color);
                    colors.Add(hex);
                    count++;
                }
            }

            return colors;
        }

//        private HashSet<string> ScanActualColors(Screenshot screenshot, int stepX, int stepY)
//        {
//            HashSet<string> colors = new HashSet<string>();

//#if !NETCOREAPP2_0 && !NETSTANDARD2_0
//            try {
//                MediaTypeNames.Image image = MediaTypeNames.Image.FromStream(new MemoryStream(screenshot.AsByteArray));
//                Bitmap bitmap = new Bitmap(image);
//                int height = bitmap.Height;
//                int width = bitmap.Width;
//                Assert.That(width, Is.GreaterThan(0));
//                Assert.That(height, Is.GreaterThan(0));

//                for (int i = 0; i < width; i = i + stepX) {
//                    for (int j = 0; j < height; j = j + stepY) {
//                        string hex = FormatColorToHex(bitmap.GetPixel(i, j).ToArgb());
//                        colors.Add(hex);
//                    }
//                }
//            } catch (Exception e) {
//                Assert.Fail("Unable to get actual colors from screenshot: " + e.Message);
//            }

//            Assert.That(colors.Count, Is.GreaterThan(0));
//#endif

//            return colors;
//        }

//        private Color GetPixelColor(Screenshot screenshot, int x, int y)
//        {
//            Color pixelColor = Color.Black;

//#if !NETCOREAPP2_0 && !NETSTANDARD2_0
//            MediaTypeNames.Image image = MediaTypeNames.Image.FromStream(new MemoryStream(screenshot.AsByteArray));
//            Bitmap bitmap = new Bitmap(image);
//            pixelColor = bitmap.GetPixel(1, 1);
//#endif
//            return pixelColor;
//        }

        private Func<Task<bool>> FrameToBeAvailableAndSwitchedTo(string frameId)
        {
            return async () => {
                try {
                    IWebElement frameElement = await driver.FindElement(By.Id(frameId));
                    await driver.SwitchTo().Frame(frameElement);
                } catch (Exception) {
                    return false;
                }

                return true;
            };
        }

        private Func<Task<bool>> ElementToBeVisibleWithId(string elementId)
        {
            return async () => {
                try {
                    IWebElement element = await driver.FindElement(By.Id(elementId));
                    return await element.Displayed();
                } catch (Exception) {
                    return false;
                }
            };
        }

        private Func<Task<bool>> TitleToBe(string desiredTitle)
        {
            return async () => await driver.Title() == desiredTitle;
        }
    }
}
