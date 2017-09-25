// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeDriverElements: IElements
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverElements(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public Task<string> ClearElement(string elementId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task Click(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.ElementCommands.ClickElement(elementId);
        }

        public Task<JToken> FindElement(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.WindowCommands.FindElement(strategy, expr, startNode, cancellationToken);
        }

        public Task<JToken> FindElements(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken);
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
            return asyncChromeDriver.ElementCommands.GetElementLocation(elementId);
        }

        public Task<string> GetElementProperty(string elementId, string propertyName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
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

        public Task<string> GetElementValueOfCssProperty(string elementId, string propertyName, CancellationToken cancellationToken)
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

        public Task<string> SubmitElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}