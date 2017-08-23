// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.BasicTypes;
using System.Linq;

namespace Zu.Chrome.DriverCore
{
    public class ElementUtils
    {
        public Session Session { get; private set; }
        public WebView webView { get; private set; }

        public ElementUtils(WebView webView, Session session)
        {
            Session = session;
            this.webView = webView;
        }


        public string GetElementKey()
        {
            if (Session?.w3c_compliant == true)
                return ElementKeys.ElementKeyW3C;
            else
                return ElementKeys.ElementKey;
        }

        public async Task<bool> VerifyElementClickable(string elementId, WebPoint location, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.IS_ELEMENT_CLICKABLE, $"{{\"{GetElementKey()}\":\"{elementId}\"}}, {WebPointToJsonString(location)}", null, true, false, cancellationToken);
            //todo add exceptions
            return (res?.Result?.Value as JObject)?["value"]?["clickable"]?.Value<bool>() == true; //  ResultValueConverter.ToBool(res?.Result?.Value);
        }

        public string WebPointToJsonString(WebPoint point)
        {
            return $"{{ \"x\": {point.X}, \"y\": {point.Y} }}";
        }
        public string WebRectToJsonString(WebRect rect)
        {
            return $"{{\"left\": {rect.X}, \"top\": {rect.Y}, \"width\": {rect.Width}, \"height\": {rect.Height} }}";
        }
        public string ElementJsonString(WebRect elementId)
        {
            return $"{{\"{GetElementKey()}\":\"{elementId}\"}}";
        }

        public async Task<WebPoint> ScrollElementRegionIntoViewHelper(string elementId, WebRect region, bool center = true, string clickableElementId = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.GET_LOCATION_IN_VIEW, $"{{\"{GetElementKey()}\":\"{elementId}\"}}, {center.ToString().ToLower()}, {WebRectToJsonString(region)}", null, true, false, cancellationToken);
            var location = ResultValueConverter.ToWebPoint(res?.Result?.Value);

            if (clickableElementId != null)
            {
                var middle = location.Offset(region.Width / 2, region.Height / 2);
                var isClickable = await VerifyElementClickable(clickableElementId, middle);
                if (!isClickable) return null;
            }
            return location;
        }

        public async Task<string> GetActiveElement(CancellationToken cancellationToken = new CancellationToken())
        {
            var func = "function() { return document.activeElement || document.body }";
            var frameId = Session == null ? "" : Session.GetCurrentFrameId();
            var res = await webView.CallFunction(func, null, frameId, true, false, cancellationToken);
            return ResultValueConverter.ToElementId(res?.Result?.Value, GetElementKey());
            //return res?.Result?.Value as JToken;
        }
        public async Task<bool> IsElementFocused(string elementId)
        {
            var activeElement = await GetActiveElement();
            return activeElement == elementId;
        }
        public async Task<string> GetElementAttribute(string elementId, string attributeName, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.GET_ATTRIBUTE, $"{{\"{GetElementKey()}\":\"{elementId}\"}}, \"{attributeName}\"", null, true, false, cancellationToken);
            return (res?.Result?.Value as JObject)?["value"]?.ToString();
        }
        public async Task<WebPoint> GetElementClickableLocation(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var tagName = await GetElementTagName(elementId, cancellationToken);
            if (tagName == "area")
            {
                var func =
                "function (element) {" +
                "  var map = element.parentElement;" +
                "  if (map.tagName.toLowerCase() != 'map')" +
                "    throw new Error('the area is not within a map');" +
                "  var mapName = map.getAttribute('name');" +
                "  if (mapName == null)" +
                "    throw new Error ('area\\'s parent map must have a name');" +
                "  mapName = '#' + mapName.toLowerCase();" +
                "  var images = document.getElementsByTagName('img');" +
                "  for (var i = 0; i < images.length; i++) {" +
                "    if (images[i].useMap.toLowerCase() == mapName)" +
                "      return images[i];" +
                "  }" +
                "  throw new Error('no img is found for the area');" +
                "}";
                var frameId = Session == null ? "" : Session.GetCurrentFrameId();
                var res = await webView.CallFunction(func, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", frameId, true, false);
                return ResultValueConverter.ToWebPoint(res?.Result?.Value);
            }
            var isDisplayed = await IsElementDisplayed(elementId, cancellationToken);
            if (isDisplayed)
            {
                var rect = await GetElementRegion(elementId, cancellationToken);
                var location = await ScrollElementRegionIntoView(elementId, rect, true, elementId, cancellationToken);
                return location.Offset(rect.Width / 2, rect.Height / 2);
            }
            return null;
        }

        public async Task<WebRect> GetElementRegion(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            //var expression = $"({get_element_region.JsSource}).apply(null, {{\"{GetElementKey()}\":\"{elementId}\"}})";
            //var frameId = Session == null ? "" : Session.GetCurrentFrameId();
            //var res = await webView.EvaluateScript(expression, frameId, true, cancellationToken);
            var res = await webView.CallFunction(get_element_region.JsSource, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", null, true, false, cancellationToken);
            return ResultValueConverter.ToWebRect(res?.Result?.Value);
        }

        public async Task<string> GetElementTagName(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var func = "function(elem) { return elem.tagName.toLowerCase(); }";
            var res = await webView.CallFunction(func, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", null, true, false, cancellationToken);
            return (res?.Result?.Value as JObject)?["value"]?.ToString();
        }
        public async Task<WebSize> GetElementSize(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.GET_SIZE, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", null, true, false, cancellationToken);
            return ResultValueConverter.ToWebSize(res?.Result?.Value);
        }
        public async Task<bool> IsElementDisplayed(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.IS_DISPLAYED, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", null, true, false, cancellationToken);
            return (res?.Result?.Value as JObject)?["value"]?.Value<bool>() == true;
        }
        public async Task<bool> IsElementEnabled(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.IS_ENABLED, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", null, true, false, cancellationToken);
            return ResultValueConverter.ToBool(res?.Result?.Value);
        }
        public async Task<bool> IsOptionElementSelected(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.IS_SELECTED, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", null, true, false, cancellationToken);
            return ResultValueConverter.ToBool(res?.Result?.Value);
        }
        public async Task<bool> IsOptionElementTogglable(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            //var expression = $"({is_option_element_toggleable.JsSource}).apply(null, {{\"{GetElementKey()}\":\"{elementId}\"}})";
            //var frameId = Session == null ? "" : Session.GetCurrentFrameId();
            //var res = await webView.EvaluateScript(expression, frameId, true, cancellationToken);
            var res = await webView.CallFunction(is_option_element_toggleable.JsSource, $"{{\"{GetElementKey()}\":\"{elementId}\"}}", null, true, false, cancellationToken);
            return ResultValueConverter.ToBool(res?.Result?.Value);
        }

        public async Task<bool> SetOptionElementSelected(string elementId, bool selected = true, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.CLICK, $"{{\"{GetElementKey()}\":\"{elementId}\"}}, {selected.ToString().ToLower()}", null, true, false, cancellationToken);
            return ResultValueConverter.ToBool(res?.Result?.Value);
        }
        public async Task ToggleOptionElement(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var isSelected = await IsOptionElementSelected(elementId, cancellationToken);
            await SetOptionElementSelected(elementId, !isSelected);
        }
        public async Task ScrollElementIntoView(string elementId, WebPoint offset = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var region = await GetElementRegion(elementId, cancellationToken);
            //var location = await ScrollElementRegionIntoView(elementId, region, false);
            //if(offset != null)
            //{
            //    location = location.Offset(offset.X, offset.Y);
            //}
            //else
            //{
            //    location = location.Offset(region.Size.Width / 2, region.Size.Height / 2);
            //}
        }
        public async Task<WebPoint> ScrollElementRegionIntoView(string elementId, WebRect region, bool center, string clickableElementId = null, CancellationToken cancellationToken = new CancellationToken())
        {
            var regionOffset = await ScrollElementRegionIntoViewHelper(elementId, region, center, clickableElementId, cancellationToken);
            if (Session?.Frames?.Any() == true)
            {
                var func =
                          "function(xpath) {" +
                          "  return document.evaluate(xpath, document, null," +
                          "      XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;" +
                          "}";
                foreach (var frame in Session.Frames)
                {
                    var args = $"\"//*[@cd_frame_id_ = '{frame.CromeFrameId}']\"";
                    var res = await webView.CallFunction(func, args);
                    //todo
                    var frameElementId = res.Result?.Value?.ToString();
                    var border = await GetElementBorder(frameElementId, cancellationToken);
                    var regionOffset2 = region.Offset(border.X, border.Y);

                    var location = await ScrollElementRegionIntoViewHelper(frameElementId, new WebRect(regionOffset, region.Size()), center, frameElementId, cancellationToken);
                    return location;
                }
            }
            return regionOffset;
        }

        public async Task<WebPoint> GetElementBorder(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            var border_left_str = await GetElementEffectiveStyle(elementId, "border-left-width", cancellationToken);
            var border_top_str = await GetElementEffectiveStyle(elementId, "border-top-width", cancellationToken);
            if (int.TryParse(border_left_str, out int x) && int.TryParse(border_top_str, out int y))
            {
                return new WebPoint(x, y);
            }
            return null;
        }

        public async Task<string> GetElementEffectiveStyle(string elementId, string property, CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await webView.CallFunction(atoms.GET_EFFECTIVE_STYLE, $"{{\"{GetElementKey()}\":\"{elementId}\"}}, \"{property}\"", null, true, false, cancellationToken);
            return ResultValueConverter.AsString(res?.Result?.Value);
        }

        public async Task<bool> IsElementAttributeEqualToIgnoreCase(string elementId, string attributeName, string attributeValue, CancellationToken cancellationToken = new CancellationToken())
        {
            var attr = await GetElementAttribute(elementId, attributeName, cancellationToken);
            return string.Equals(attr, attributeValue, StringComparison.InvariantCultureIgnoreCase);
        }


    }
}
