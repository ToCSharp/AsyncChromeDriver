// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome.DriverCore
{
    internal class ResultValueConverter
    {
        internal static WebPoint ToWebPoint(object value)
        {
            var res = (value as JObject)?["value"];
            if (res != null)
            {
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
            if (res != null)
            {
                var width = res["width"]?.Value<int>();
                var height = res["height"]?.Value<int>();
                if (width != null && height != null) return new WebSize((int)width, (int)height);
            }
            return null;
        }

        internal static WebRect ToWebRect(object value)
        {
            var res = (value as JObject)?["value"];
            if (res != null)
            {
                var x = res["x"]?.Value<int>() ?? res["left"]?.Value<int>();
                var y = res["y"]?.Value<int>() ?? res["top"]?.Value<int>();
                var width = res["width"]?.Value<int>();
                var height = res["height"]?.Value<int>();
                if (x != null && y != null && width != null && height != null) return new WebRect((int)x, (int)y, (int)width, (int)height);
            }
            return null;
        }

        internal static string AsString(object value)
        {
            return (value as JObject)?["value"]?.ToString();
        }

        internal static string ToElementId(object value, string elementKey = "ELEMENT")
        {
            return (value as JObject)?["value"]?[elementKey]?.ToString();
        }
    }
}