# AsyncChromeDriver
Chrome WebDriver and Chrome DevTools in one library. 

It connects directly to [Chrome DevTools](https://chromedevtools.github.io/devtools-protocol/) and is async from this connection. No need in chromedriver.

AsyncChromeDriver implements [IAsyncWebBrowserClient](https://github.com/ToCSharp/IAsyncWebBrowserClient) and can be used as [AsyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver).

It also has DevTools property and you can easily use all power of Chrome DevTools from your .Net app. Thanks to [BaristaLabs/chrome-dev-tools-runtime](https://github.com/BaristaLabs/chrome-dev-tools-runtime)

[![Join the chat at https://gitter.im/AsyncWebDriver/Lobby](https://badges.gitter.im/AsyncWebDriver/Lobby.svg)](https://gitter.im/AsyncWebDriver/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Usage
### Install AsyncChromeDriver via NuGet

If you want to include AsyncChromeDriver in your project, you can [install it directly from NuGet](https://www.nuget.org/packages/AsyncChromeDriver/)
```
PM> Install-Package AsyncChromeDriver
```
### Write code example
```csharp
     var asyncChromeDriver = new AsyncChromeDriver();
     var webDriver = new WebDriver(asyncChromeDriver);
     await webDriver.Options().Timeouts.SetImplicitWait(TimeSpan.FromSeconds(3));
     await webDriver.GoToUrl("https://www.google.com/");
     var query = await webDriver.FindElementByName("q");
     
     //await query.SendKeys("ToCSharp");
     foreach (var v in "ToCSharp".ToList())
     {
        await Task.Delay(500 + new Random().Next(500));
        await query.SendKeys(v.ToString());
      }
      await Task.Delay(500);
      await query.SendKeys(Keys.Enter);
      var allCookies = await asyncChromeDriver.DevTools.Session.Network.GetAllCookies();

      var screenshot = await webDriver.GetScreenshot();
      using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
      {
        System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
        screenshotImage.Save(GetFilePathToSaveScreenshot(), System.Drawing.Imaging.ImageFormat.Png);
      }

```
### Using DevTools
```csharp
    asyncChromeDriver = new AsyncChromeDriver();
    await asyncChromeDriver.CheckConnected();
    await asyncChromeDriver.DevTools.Session.Page.Enable();
    asyncChromeDriver.DevTools.Session.Page.SubscribeToLoadEventFiredEvent(async (e2) =>
    {
        var screenshot = await asyncChromeDriver.DevTools.Session.Page.CaptureScreenshot();
        SaveScreenshot(screenshot.Data);
    });
    //await asyncChromeDriver.GoToUrl("https://www.google.com/");
    await asyncChromeDriver.DevTools.Session.Page.Navigate(new BaristaLabs.ChromeDevTools.Runtime.Page.NavigateCommand
    {
        Url = "https://www.google.com/"
    });
```
### Headless Chrome
```csharp
     var asyncChromeDriver = new AsyncChromeDriver(new ChromeDriverConfig().SetHeadless().SetWindowSize(width, height));
     var webDriver = new WebDriver(asyncChromeDriver);
     await webDriver.GoToUrl("https://www.google.com/");
     await Task.Delay(500);
     var screenshot = await webDriver.GetScreenshot();
     using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
     {
        System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
        screenshotImage.Save(GetFilePathToSaveScreenshot(), System.Drawing.Imaging.ImageFormat.Png);
     }
     await webDriver.Close();
```

## Examples
Look at AsyncChromeDriverExample.

## Tests
[Unit Tests from Selenuim](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter.Common.Tests) run through [SeleniumAdapter](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter) -> [SyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver/tree/master/AsyncWebDriver/SyncWrapper) -> [AsyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver) -> [AsyncChromeDriver](https://github.com/ToCSharp/AsyncChromeDriver)  


## Implemented

[Unit tests results for AsyncChromeDriver 0.2.6 and Chrome 61](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/wiki/Unit-tests-results-for-Chrome)  


In WebDriver part mostly implemented: Navigation, WebElements, Mouse, Keyboard, Clicks, ExecutingJavascript, TakesScreenshot.  
Medium realized: Frame switching,  
Poorly implemented: ExecutingAsyncJavascript  
Not implemeted: Stale elements, Cookies, WindowSwitching, storages

It was ported from [ChromeDriver](https://cs.chromium.org/chromium/src/chrome/test/chromedriver/README.txt) not directly, 
partially. 


## Contribute!
If you see NotImplementedException, means you need functionality, which is not implemented yet.

Write issue or to [![Join the chat at https://gitter.im/AsyncWebDriver/Lobby](https://badges.gitter.im/AsyncWebDriver/Lobby.svg)](https://gitter.im/AsyncWebDriver/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge). I implemented what I myself use at first.

Feel free to submit pull requests.

Please :star: star the project or [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=GNRXFA5WSNFF6) if you like the project.
