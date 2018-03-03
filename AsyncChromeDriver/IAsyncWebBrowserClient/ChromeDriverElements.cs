// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;
using System;
using System.Linq;
using Zu.Chrome.DriverCore;
using System.Collections.Generic;

namespace Zu.Chrome
{
    public class ChromeDriverElements : IElements
    {
        private IAsyncChromeDriver _asyncChromeDriver;
        public ChromeDriverElements(IAsyncChromeDriver asyncChromeDriver)
        {
            _asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<string> ClearElement(string elementId, CancellationToken cancellationToken)
        {
            var res = await _asyncChromeDriver.ElementCommands.ClearElement(elementId, cancellationToken).ConfigureAwait(false);
            return "ok";
        }

        public Task Click(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementCommands.ClickElement(elementId);
        }

        public async Task<JToken> FindElement(string strategy, string expr, string startNode, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default (CancellationToken))
        {
            try
            {
                JToken res = null;
                var waitEnd = default (DateTime);
                var nowTime = DateTime.Now;
                while (true)
                {
                    res = await _asyncChromeDriver.WindowCommands.FindElement(strategy, expr, startNode, cancellationToken).ConfigureAwait(false);
                    if (!ResultValueConverter.ValueIsNull(res))
                    {
                        if (notElementId == null)
                            break;
                        else
                        {
                            var elId = GetElementFromResponse(res);
                            if (elId != notElementId)
                                break;
                        }
                    }

                    if (waitEnd == default (DateTime))
                    {
                        var implicitWait = timeout;
                        if (implicitWait == default (TimeSpan))
                            implicitWait = await _asyncChromeDriver.Options.Timeouts.GetImplicitWait().ConfigureAwait(false);
                        if (implicitWait == default (TimeSpan))
                            break;
                        waitEnd = nowTime + implicitWait;
                    }

                    if (DateTime.Now > waitEnd)
                        break;
                    await Task.Delay(50).ConfigureAwait(false);
                }

                if (ResultValueConverter.ValueIsNull(res))
                    throw new WebBrowserException($"Element not found by {strategy} = {expr}", "no such element");
                return res;
            }
            catch
            {
                throw;
            }
        //var res = await asyncChromeDriver.WindowCommands.FindElement(strategy, expr, startNode, cancellationToken);
        //if (ResultValueConverter.ValueIsNull(res)) 
        //{
        //    var implicitWait = await asyncChromeDriver.Options.Timeouts.GetImplicitWait();
        //    if (implicitWait != default(TimeSpan))
        //    {
        //        var waitEnd = DateTime.Now + implicitWait;
        //        while (ResultValueConverter.ValueIsNull(res) && DateTime.Now < waitEnd)
        //        {
        //            Thread.Sleep(50);
        //            res = await asyncChromeDriver.WindowCommands.FindElement(strategy, expr, startNode, cancellationToken = default(CancellationToken));
        //        }
        //    }
        //}
        //if (ResultValueConverter.ValueIsNull(res)) throw new WebBrowserException($"Element not found by {strategy} = {expr}", "no such element");
        //return res;
        }

        public static string GetElementFromResponse(JToken response)
        {
            if (response == null)
                return null;
            string id = null;
            var json = response is JValue ? JToken.Parse(response.Value<string>()) : response["value"];
            if (json is JValue)
            {
                if (((JValue)json).Value == null)
                    return null;
                else
                    return ((JValue)json).Value<string>();
            }

            id = json?["element-6066-11e4-a52e-4f735466cecf"]?.ToString();
            if (id == null)
                id = json?["ELEMENT"]?.ToString();
            return id;
        }

        public async Task<JToken> FindElements(string strategy, string expr, string startNode, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default (CancellationToken))
        {
            try
            {
                JToken res = null;
                var waitEnd = default (DateTime);
                var nowTime = DateTime.Now;
                while (true)
                {
                    res = await _asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken).ConfigureAwait(false);
                    if ((res as JArray)?.Any() != true)
                    {
                        if (notElementId == null)
                            break;
                        else
                        {
                            var elId = GetElementsFromResponse(res);
                            if (elId?.FirstOrDefault() != notElementId)
                                break;
                        }
                    }

                    if (waitEnd == default (DateTime))
                    {
                        var implicitWait = timeout;
                        if (implicitWait == default (TimeSpan))
                            implicitWait = await _asyncChromeDriver.Options.Timeouts.GetImplicitWait().ConfigureAwait(false);
                        if (implicitWait == default (TimeSpan))
                            break;
                        waitEnd = nowTime + implicitWait;
                    }

                    if (DateTime.Now > waitEnd)
                        break;
                    await Task.Delay(50).ConfigureAwait(false);
                }

                //if ((res as JArray)?.Any() != true) throw new WebBrowserException($"Elements not found by {strategy} = {expr}", "no such element");
                return res;
            }
            catch
            {
                throw;
            }
        //var res = await asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken = default(CancellationToken));
        //if ((res as JArray)?.Any() != true)
        //{
        //    var implicitWait = await asyncChromeDriver.Options.Timeouts.GetImplicitWait();
        //    if (implicitWait != default(TimeSpan))
        //    {
        //        var waitEnd = DateTime.Now + implicitWait;
        //        while (((res as JArray)?.Any() != true) && DateTime.Now < waitEnd)
        //        {
        //            Thread.Sleep(50);
        //            res = await asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken = default(CancellationToken));
        //        }
        //    }
        //}
        //if (res == null) throw new WebBrowserException($"Element not found by {strategy} = {expr}", "no such element");
        //return res;
        ////return asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken);
        }

        public static List<string> GetElementsFromResponse(JToken response)
        {
            var toReturn = new List<string>();
            if (response is JArray)
                foreach (var item in response)
                {
                    string id = null;
                    try
                    {
                        var json = item is JValue ? JToken.Parse(item.Value<string>()) : item;
                        id = json?["element-6066-11e4-a52e-4f735466cecf"]?.ToString();
                        if (id == null)
                            id = json?["ELEMENT"]?.ToString();
                    }
                    catch
                    {
                    }

                    toReturn.Add(id);
                }

            return toReturn;
        }

        public Task<string> GetActiveElement(CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.GetActiveElement(cancellationToken);
        }

        public Task<string> GetElementAttribute(string elementId, string attrName, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.GetElementAttribute(elementId, attrName, cancellationToken);
        }

        public Task<WebPoint> GetElementLocation(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementCommands.GetElementLocation(elementId, cancellationToken);
        }

        public async Task<string> GetElementProperty(string elementId, string propertyName, CancellationToken cancellationToken = default (CancellationToken))
        {
            return null;
        }

        public Task<WebRect> GetElementRect(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.GetElementRegion(elementId, cancellationToken);
        }

        public Task<WebSize> GetElementSize(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.GetElementSize(elementId, cancellationToken);
        }

        public Task<string> GetElementTagName(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.GetElementTagName(elementId, cancellationToken);
        }

        public Task<string> GetElementText(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.GetElementText(elementId, cancellationToken);
        }

        public Task<string> GetElementValueOfCssProperty(string elementId, string propertyName, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementCommands.GetElementValueOfCssProperty(elementId, propertyName, cancellationToken);
        }

        public Task<bool> IsElementDisplayed(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.IsElementDisplayed(elementId, cancellationToken);
        }

        public Task<bool> IsElementEnabled(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.IsElementEnabled(elementId, cancellationToken);
        }

        public Task<bool> IsElementSelected(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementUtils.IsOptionElementSelected(elementId, cancellationToken);
        }

        public Task<string> SendKeysToElement(string elementId, string value, CancellationToken cancellationToken = default (CancellationToken))
        {
            return _asyncChromeDriver.ElementCommands.SendKeysToElement(elementId, value);
        }

        public async Task<string> SubmitElement(string elementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            var res = await _asyncChromeDriver.ElementCommands.SubmitElement(elementId, cancellationToken).ConfigureAwait(false);
            return "ok";
        }

#region FindElement variants
        public Task<JToken> FindElement(string strategy, string expr, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, null, null, default (TimeSpan), cancellationToken);
        }

        public Task<JToken> FindElement(string strategy, string expr, TimeSpan timeout, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, null, null, timeout, cancellationToken);
        }

        public Task<JToken> FindElement(string strategy, string expr, int timeoutMs, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, null, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public Task<JToken> FindElement(string strategy, string expr, string startNode, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, startNode, null, default (TimeSpan), cancellationToken);
        }

        public Task<JToken> FindElement(string strategy, string expr, string startNode, TimeSpan timeout, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, startNode, null, timeout, cancellationToken);
        }

        public Task<JToken> FindElement(string strategy, string expr, string startNode, int timeoutMs, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, startNode, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public Task<JToken> FindElement(string strategy, string expr, string startNode, string notElementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, startNode, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<JToken> FindElement(string strategy, string expr, string startNode, string notElementId, int timeoutMs, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElement(strategy, expr, startNode, notElementId, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, null, null, default (TimeSpan), cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, TimeSpan timeout, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, null, null, timeout, cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, int timeoutMs, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, null, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, string startNode, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, startNode, null, default (TimeSpan), cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, string startNode, TimeSpan timeout, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, startNode, null, timeout, cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, string startNode, int timeoutMs, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, startNode, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, string startNode, string notElementId, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, startNode, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, string startNode, string notElementId, int timeoutMs, CancellationToken cancellationToken = default (CancellationToken))
        {
            return FindElements(strategy, expr, startNode, notElementId, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }
#endregion
    }
}