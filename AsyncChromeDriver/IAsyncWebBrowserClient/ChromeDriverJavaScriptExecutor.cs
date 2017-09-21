// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using System;
using System.Collections.ObjectModel;

namespace Zu.Chrome
{
    public class ChromeDriverJavaScriptExecutor : IJavaScriptExecutor
    {
        private IAsyncChromeDriver asyncChromeDriver;

        public ChromeDriverJavaScriptExecutor(IAsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<object> ExecuteAsyncScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            var res = await asyncChromeDriver.WindowCommands.ExecuteAsyncScript(script, ArgsToStringList(args));
            var val = ((res as JObject)?["value"] as JObject)?["value"];
            if (val is JValue) return ((JValue)val).Value;
            if (val is JArray) return CreateListOfObjects((JArray)val);
            return val;
        }

        internal ReadOnlyCollection<object> CreateListOfObjects(JArray array)
        {
            var res = new List<object>();
            foreach (var val in array)
            {
                if (val is JValue) res.Add(((JValue)val).Value);
                else if (val is JArray) res.Add(CreateListOfObjects((JArray)val));
                else res.Add(val);
            }
            return new ReadOnlyCollection<object>(res);
        }

        public async Task<object> ExecuteScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            var res = await asyncChromeDriver.WindowCommands.ExecuteScript(script, ArgsToStringList(args));
            return ((res as JObject)?["value"] as JValue)?.Value;
        }

        internal List<string> ArgsToStringList(object[] args)
        {
            return args.Select(v =>
            {
                if (v is bool) return (bool)v ? "true" : "false";
                if (v is string) return $"'{(string)v}'";
                return v.ToString();
            }).ToList();
        }
    }
}