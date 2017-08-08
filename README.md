# AsyncChromeDriver
Chrome WebDriver and Chrome DevTools in one library. 

It connects directly to [Chrome DevTools](https://chromedevtools.github.io/devtools-protocol/) and is async from this connection. No need in chromedriver.

AsyncChromeDriver implements [IAsyncWebBrowserClient](https://github.com/ToCSharp/AsyncWebDriver/blob/master/IAsyncWebBrowserClient/IAsyncWebBrowserClient.cs) and can be used as [AsyncWebDriver](https://github.com/ToCSharp/AsyncWebDriver).

It also has DevTools property and you can easily use all power of Chrome DevTools from your .Net app. Thanks to [BaristaLabs/chrome-dev-tools-sample](https://github.com/BaristaLabs/chrome-dev-tools-sample)

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
     await webDriver.GoToUrl("https://www.google.com/");
     var query = await webDriver.FindElement(By.Name("q"));
     foreach (var v in "ToCSharp".ToList())
     {
        await Task.Delay(500 + new Random().Next(500));
        await query.SendKeys(v.ToString());
      }
      await Task.Delay(500);
      await query.SendKeys(Keys.Enter);
      var allCookies = await asyncChromeDriver.DevTools.Session.Network.GetAllCookies(new GetAllCookiesCommand());

      var screenshot = await webDriver.GetScreenshot();
      screenshot.SaveAsFile(GetFilePathToSaveScreenshot(), Zu.WebBrowser.BasicTypes.ScreenshotImageFormat.Png);

```

## Examples
Look at AsyncChromeDriverExample.

Run built Example in release tab.

## Implemented
WebDriver part has only basic functionality. Navigation, FindElements, Mouse, Keyboard.

It was ported from [ChromeDriver](https://cs.chromium.org/chromium/src/chrome/test/chromedriver/README.txt) not directly, 
partially. 

## TODO (in WebDriver part, DevTools are good)
* [ ] Frames and tabs
* [ ] Exceptions
* [ ] Timeouts
* [ ] Tests
* [ ] Documentation

## Contribute!
If you see NotImplementedException, means you need functionality, which is not implemented yet.

Write issue or to [![Join the chat at https://gitter.im/AsyncWebDriver/Lobby](https://badges.gitter.im/AsyncWebDriver/Lobby.svg)](https://gitter.im/AsyncWebDriver/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge). I implemented what I myself use at first.

Feel free to submit pull requests.

Please star the project. More stars - more time I'll spend on it. There's so much to do here.
