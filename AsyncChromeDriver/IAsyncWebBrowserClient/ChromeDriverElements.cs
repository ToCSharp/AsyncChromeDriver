// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;
using System;
using System.Linq;
using Zu.Chrome.DriverCore;

namespace Zu.Chrome
{
    public class ChromeDriverElements : IElements
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverElements(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<string> ClearElement(string elementId, CancellationToken cancellationToken)
        {
            var res = await asyncChromeDriver.ElementCommands.ClearElement(elementId, cancellationToken);
            return "ok";
        }

        public Task Click(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementCommands.ClickElement(elementId);
        }

        public async Task<JToken> FindElement(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await asyncChromeDriver.WindowCommands.FindElement(strategy, expr, startNode, cancellationToken);
            if (ResultValueConverter.ValueIsNull(res)) 
            {
                var implicitWait = await asyncChromeDriver.Options.Timeouts.GetImplicitWait();
                if (implicitWait != default(TimeSpan))
                {
                    var waitEnd = DateTime.Now + implicitWait;
                    while (ResultValueConverter.ValueIsNull(res) && DateTime.Now < waitEnd)
                    {
                        Thread.Sleep(50);
                        res = await asyncChromeDriver.WindowCommands.FindElement(strategy, expr, startNode, cancellationToken = default(CancellationToken));
                    }
                }
            }
            if (ResultValueConverter.ValueIsNull(res)) throw new WebBrowserException($"Element not found by {strategy} = {expr}", "no such element");
            return res;
        }
         
        public async Task<JToken> FindElements(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken = default(CancellationToken));
            if ((res as JArray)?.Any() != true)
            {
                var implicitWait = await asyncChromeDriver.Options.Timeouts.GetImplicitWait();
                if (implicitWait != default(TimeSpan))
                {
                    var waitEnd = DateTime.Now + implicitWait;
                    while (((res as JArray)?.Any() != true) && DateTime.Now < waitEnd)
                    {
                        Thread.Sleep(50);
                        res = await asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken = default(CancellationToken));
                    }
                }
            }
            if (res == null) throw new WebBrowserException($"Element not found by {strategy} = {expr}", "no such element");
            return res;
            //return asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken);
        }

        public Task<string> GetActiveElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.GetActiveElement(cancellationToken);
        }

        public Task<string> GetElementAttribute(string elementId, string attrName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.GetElementAttribute(elementId, attrName, cancellationToken);
        }

        public Task<WebPoint> GetElementLocation(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementCommands.GetElementLocation(elementId, cancellationToken);
        }

        public async Task<string> GetElementProperty(string elementId, string propertyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return null;
        }

        public Task<WebRect> GetElementRect(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.GetElementRegion(elementId, cancellationToken);
        }

        public Task<WebSize> GetElementSize(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.GetElementSize(elementId, cancellationToken);
        }

        public Task<string> GetElementTagName(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.GetElementTagName(elementId, cancellationToken);
        }

        public Task<string> GetElementText(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.GetElementText(elementId, cancellationToken);
        }

        public Task<string> GetElementValueOfCssProperty(string elementId, string propertyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementCommands.GetElementValueOfCssProperty(elementId, propertyName, cancellationToken);
        }

        public Task<bool> IsElementDisplayed(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.IsElementDisplayed(elementId, cancellationToken);
        }

        public Task<bool> IsElementEnabled(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.IsElementEnabled(elementId, cancellationToken);
        }

        public Task<bool> IsElementSelected(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementUtils.IsOptionElementSelected(elementId, cancellationToken);
        }

        public Task<string> SendKeysToElement(string elementId, string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementCommands.SendKeysToElement(elementId, value);
        }

        public async Task<string> SubmitElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await asyncChromeDriver.ElementCommands.SubmitElement(elementId, cancellationToken);
            return "ok";
        }
    }
}