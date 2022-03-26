## AsyncChromeDriver
Async .Net Chrome WebDriver and Chrome DevTools in one library. 

It connects directly to [Chrome DevTools](https://chromedevtools.github.io/devtools-protocol/) and is async from this connection. No need in chromedriver.

AsyncChromeDriver implements [IAsyncWebBrowserClient](https://github.com/ToCSharp/IAsyncWebBrowserClient) and can be used as [AsyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver).

It also has DevTools property and you can easily use all power of Chrome DevTools from your .Net app. We use modified version from [BaristaLabs/chrome-dev-tools-generator](https://github.com/BaristaLabs/chrome-dev-tools-generator)

Look video of BrowsersDevToolsExample: https://youtu.be/dNMuoeDmEME  
[![Join the chat at https://gitter.im/AsyncWebDriver/Lobby](https://badges.gitter.im/AsyncWebDriver/Lobby.svg)](https://gitter.im/AsyncWebDriver/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### Usage
#### Install AsyncChromeDriver via NuGet
Targets Standard 2.0 and .NET Framework 4.6.2  
If you want to include AsyncChromeDriver in your project, you can [install it directly from NuGet](https://www.nuget.org/packages/AsyncChromeDriver/)
```
PM> Install-Package AsyncChromeDriver
> dotnet add package AsyncChromeDriver 
```
#### Write code example
```csharp
     var asyncChromeDriver = new AsyncChromeDriver();
     var webDriver = new WebDriver(asyncChromeDriver);
     await webDriver.Options().Timeouts.SetImplicitWait(TimeSpan.FromSeconds(3));
     await webDriver.GoToUrl("https://www.google.com/");
     var query = await webDriver.FindElementByName("q");
     
     //await query.SendKeys("ToCSharp");
     var rnd = new Random();
     foreach (var v in "ToCSharp".ToList())
     {
        await Task.Delay(500 + rnd.Next(500));
        await query.SendKeys(v.ToString());
      }
      await Task.Delay(500);
      await query.SendKeys(Keys.Enter);
      var allCookies = await asyncChromeDriver.DevTools.Network.GetAllCookies();

      var screenshot = await webDriver.GetScreenshot();
      using (MemoryStream imageStream = new MemoryStream(screenshot.AsByteArray))
      {
        System.Drawing.Image screenshotImage = System.Drawing.Image.FromStream(imageStream);
        screenshotImage.Save(GetFilePathToSaveScreenshot(), System.Drawing.Imaging.ImageFormat.Png);
      }

```
#### Using DevTools
Protocol r655971 on May 2, 2019 (Chrome 74).  
https://github.com/ChromeDevTools/devtools-protocol/tree/master/json  
```csharp
    asyncChromeDriver = new AsyncChromeDriver();
    await asyncChromeDriver.CheckConnected();
    await asyncChromeDriver.DevTools.Page.Enable();
    asyncChromeDriver.DevTools.Page.SubscribeToLoadEventFiredEvent(async (e2) =>
    {
        var screenshot = await asyncChromeDriver.DevTools.Page.CaptureScreenshot();
        SaveScreenshot(screenshot.Data);
    });
    //await asyncChromeDriver.GoToUrl("https://www.google.com/");
    await asyncChromeDriver.DevTools.Page.Navigate(new ChromeDevTools.Page.NavigateCommand
    {
        Url = "https://www.google.com/"
    });
```
#### Headless Chrome
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
#### Chrome native Developer Tools
This feature is out of date. And you may further use it. But it opens unnecessary WebSocket server (we need remove this). Someday I do it if you ask.  
https://developers.google.com/web/updates/2017/10/devtools-release-notes#multi-client  
Look [BrowsersDevToolsExample](https://github.com/ToCSharp/BrowsersDevToolsExample). It's interesting, you can open Developer tools for opened Developer tools.
```csharp
   asyncChromeDriver = new AsyncChromeDriver(
        new ChromeDriverConfig().SetDoOpenBrowserDevTools());
   webDriver = new WebDriver(asyncChromeDriver);
   await asyncChromeDriver.Connect();
```
#### Examples
Look at AsyncChromeDriverExample.

#### Tests
Tests ported to async/await from selenium project In AsyncChromeDriver.Tests. Before run, start HtmlForTests (Start Without Debugging)  
Or  
[Unit Tests from Selenuim](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter.Common.Tests) run through [SeleniumAdapter](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/tree/master/AsyncWebDriver.SeleniumAdapter) -> [SyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver/tree/master/AsyncWebDriver/SyncWrapper) -> [AsyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver) -> [AsyncChromeDriver](https://github.com/ToCSharp/AsyncChromeDriver)  


#### Implemented

[Unit tests results for AsyncChromeDriver](https://github.com/ToCSharp/AsyncChromeDriverExamplesAndTests/wiki/Unit-tests-results-for-Chrome)  


In WebDriver part mostly implemented: Navigation, WebElements, Mouse, Keyboard, Clicks, ExecutingJavascript, TakesScreenshot.  
Medium realized: Frame switching,  
Poorly implemented: ExecutingAsyncJavascript  
Not implemeted: Stale elements, Cookies, WindowSwitching, storages

It was ported from [ChromeDriver](https://cs.chromium.org/chromium/src/chrome/test/chromedriver/README.txt) not directly, 
partially. 


#### Contribute!
If you see NotImplementedException, means you need functionality, which is not implemented yet.

Write issue or to [![Join the chat at https://gitter.im/AsyncWebDriver/Lobby](https://badges.gitter.im/AsyncWebDriver/Lobby.svg)](https://gitter.im/AsyncWebDriver/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge). I implemented what I myself use at first.

Feel free to submit pull requests.

Please :star: star the project if you like it.

#### Thanks to
TODO