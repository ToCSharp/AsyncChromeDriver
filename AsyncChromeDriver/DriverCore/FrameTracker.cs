// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;
using Zu.ChromeDevTools.Page;
using Zu.ChromeDevTools.Runtime;

namespace Zu.Chrome.DriverCore
{
    public class FrameTracker
    {
        private ChromeDevToolsConnection _devTools;
        private ConcurrentDictionary<string, long> _frameToContext = new ConcurrentDictionary<string, long>();
        public FrameTracker(ChromeDevToolsConnection devTools)
        {
            _devTools = devTools;
        }

        public async Task Enable()
        {
            _devTools.Runtime.SubscribeToExecutionContextCreatedEvent(OnContextCreatedEvent);
            _devTools.Runtime.SubscribeToExecutionContextDestroyedEvent(OnContextDestroyedEvent);
            _devTools.Runtime.SubscribeToExecutionContextsClearedEvent(OnContextsClearedEvent);
            _devTools.Page.SubscribeToFrameNavigatedEvent(OnFrameNavigatedEvent);
            await _devTools.Runtime.Enable().ConfigureAwait(false);
            await _devTools.Page.Enable().ConfigureAwait(false);
        }

        public long ? GetContextIdForFrame(string frame)
        {
            if (_frameToContext.TryGetValue(frame, out long res))
                return res;
            //throw new KeyNotFoundException(frame);
            return null;
        }

        private void OnContextCreatedEvent(ExecutionContextCreatedEvent ev)
        {
            var auxData = ev.Context.AuxData as JObject;
            if (auxData != null)
            {
                var isDefault = auxData["isDefault"]?.Value<bool>();
                var frameId = auxData["frameId"]?.Value<string>();
                if (isDefault == true && !string.IsNullOrWhiteSpace(frameId))
                    _frameToContext[frameId] = ev.Context.Id;
            }
        }

        private void OnContextDestroyedEvent(ExecutionContextDestroyedEvent ev)
        {
            var itemsToRemove = _frameToContext.Where(v => v.Value == ev.ExecutionContextId);
            foreach (var item in itemsToRemove)
                _frameToContext.TryRemove(item.Key, out long context);
        }

        private void OnContextsClearedEvent(ExecutionContextsClearedEvent ev)
        {
            _frameToContext.Clear();
        }

        private void OnFrameNavigatedEvent(FrameNavigatedEvent obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Frame.ParentId))
                _frameToContext.Clear();
        }
    }
}