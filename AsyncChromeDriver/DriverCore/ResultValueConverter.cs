// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome.DriverCore
{
    internal class ResultValueConverter
    {
        internal static WebPoint ToWebPoint(object value)
        {
            var res = (value as JObject)?["value"];
            if (res != null) {
                var x = res["x"]?.Value<int>();
                var y = res["y"]?.Value<int>();
                if (x != null && y != null) return new WebPoint((int)x, (int)y);
            }
            return null;
        }

        internal static bool ToBool(object value)
        {
            return (value as JObject)?["value"]?.Value<bool>() == true;
        }

        internal static WebSize ToWebSize(object value)
        {
            var res = (value as JObject)?["value"];
            if (res != null) {
                var width = res["width"]?.Value<int>();
                var height = res["height"]?.Value<int>();
                if (width != null && height != null) return new WebSize((int)width, (int)height);
            }
            return null;
        }

        internal static WebRect ToWebRect(object value)
        {
            var res = (value as JObject)?["value"];
            if (res != null) {
                var x = res["x"]?.Value<int>() ?? res["left"]?.Value<int>();
                var y = res["y"]?.Value<int>() ?? res["top"]?.Value<int>();
                var width = res["width"]?.Value<int>();
                var height = res["height"]?.Value<int>();
                if (x != null && y != null && width != null && height != null) return new WebRect((int)x, (int)y, (int)width, (int)height);
            }
            return null;
        }

        internal static bool ValueIsNull(JToken res)
        {
            if (res == null) return true;
            if (res?["value"] is JValue && (res?["value"] as JValue)?.Value == null) return true;
            return false;
        }

        internal static string AsString(object value)
        {
            return ((string)(value as JObject)?["value"])?.Replace("\n", "\r\n").Replace("\r\r", "\r");
        }

        internal static string ToElementId(object value, string elementKey = "ELEMENT")
        {
            return (value as JObject)?["value"]?[elementKey]?.ToString();
        }

        internal static Exception ToWebBrowserException(JToken json)
        {
            if (json is JArray) return null;
            var status = (json as JObject)?["status"]?.ToString();
            if (status == "0") return null;
            var value = (json as JObject)?["value"]?.ToString();
            var res = new WebBrowserException(value) {
                Json = json
            };
            if (value == null) { }
            else if (status == "10" && value == "element is not attached to the page document") {
                res.Error = "stale element reference";
            } else if (status == "13" && value.EndsWith("is not defined")) {
                res.Error = "invalid operation";
            } else if (status == "32") {
                res.Error = "invalid selector";
            } else if (status == "17") {
                return new InvalidOperationException(value);
            } else {
                throw new NotImplementedException(nameof(ToWebBrowserException) + ": " + value);
            }
            return res;
        }
    }
}