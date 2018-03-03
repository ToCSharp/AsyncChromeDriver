// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using System.Collections.ObjectModel;
using System.Collections;
using Zu.Chrome.DriverCore;

namespace Zu.Chrome
{
    public class ChromeDriverJavaScriptExecutor : IJavaScriptExecutor
    {
        private IAsyncChromeDriver _asyncChromeDriver;
        public ChromeDriverJavaScriptExecutor(IAsyncChromeDriver asyncChromeDriver)
        {
            _asyncChromeDriver = asyncChromeDriver;
        }

        public async Task<object> ExecuteAsyncScript(string script, CancellationToken cancellationToken = default (CancellationToken), params object[] args)
        {
            var res = await _asyncChromeDriver.WindowCommands.ExecuteAsyncScript(script, ArgsToStringList(args)).ConfigureAwait(false);
            var value = (res as JObject)?["value"];
            var exception = ResultValueConverter.ToWebBrowserException(value);
            if (exception != null)
                throw exception;
            return ParseExecuteScriptReturnValue((value as JObject)?["value"]);
        }

        internal ReadOnlyCollection<object> CreateListOfObjects(JArray array)
        {
            var res = new List<object>();
            foreach (var val in array)
            {
                if (val is JValue)
                    res.Add(((JValue)val).Value);
                else if (val is JArray)
                    res.Add(CreateListOfObjects((JArray)val));
                else
                    res.Add(val);
            }

            return new ReadOnlyCollection<object>(res);
        }

        public async Task<object> ExecuteScript(string script, CancellationToken cancellationToken = default (CancellationToken), params object[] args)
        {
            var res = await _asyncChromeDriver.WindowCommands.ExecuteScript(script, ArgsToStringList(args)).ConfigureAwait(false);
            var exception = ResultValueConverter.ToWebBrowserException(res);
            if (exception != null)
                throw exception;
            return ParseExecuteScriptReturnValue((res as JObject)?["value"]); // (res as JObject)?["value"] as JValue)?.Value;
        }

        internal List<string> ArgsToStringList(object[] args)
        {
            return args.Select(v => ArgToString(v)).ToList();
        }

        internal string ArgToString(object arg)
        {
            if (arg == null)
                return "null";
            if (arg is bool)
                return (bool)arg ? "true" : "false";
            if (arg is string)
                return $"'{(string)arg}'";
            IDictionary dictionaryArg = arg as IDictionary;
            if (dictionaryArg != null)
            {
                List<string> stringList = new List<string>();
                foreach (DictionaryEntry kv in dictionaryArg)
                {
                    stringList.Add($"'{kv.Key}': {ArgToString(kv.Value)}");
                }

                return $"{{ {string.Join(", ", stringList)} }}";
            }

            if (arg is IDictionary<string, object>)
                return $"{{ {string.Join(", ", ((IDictionary<string, object>)arg).Select(v => ArgToString(v)))} }}";
            if (arg is KeyValuePair<string, object>)
            {
                var kv = (KeyValuePair<string, object>)arg;
                return $"{{ '{kv.Key}': {ArgToString(kv.Value)} }}";
            }

            IEnumerable enumerableArg = arg as IEnumerable;
            if (enumerableArg != null)
            {
                List<object> objectList = new List<object>();
                foreach (object item in enumerableArg)
                {
                    objectList.Add(item);
                }

                return $"[ {string.Join(", ", ArgsToStringList((objectList.ToArray())))} ]";
            }

            return arg.ToString();
        }

        private object ParseExecuteScriptReturnValue(JToken responseValue)
        {
            if (responseValue is JValue)
                return ((JValue)responseValue).Value;
            if (responseValue is JArray)
            {
                var res = new List<object>();
                foreach (var item in (JArray)responseValue)
                {
                    res.Add(ParseExecuteScriptReturnValue(item));
                }

                return res.ToArray();
            }
            else if (responseValue is JObject)
            {
                var res = new Dictionary<string, object>();
                foreach (var item in (JObject)responseValue)
                {
                    res.Add(item.Key, ParseExecuteScriptReturnValue(item.Value));
                }

                return res;
            }

            return responseValue;
        }
    }
}