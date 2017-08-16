// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.

using System.Linq;
using System.Threading.Tasks;
using BaristaLabs.ChromeDevTools.Runtime.Page;
using BaristaLabs.ChromeDevTools.Runtime.Runtime;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace Zu.Chrome.DriverCore
{
    public class FrameTracker
    {
        private ChromeDevTools devTools;
        private ConcurrentDictionary<string, long> frameToContext = new ConcurrentDictionary<string, long>();

        public FrameTracker(ChromeDevTools devTools)
        {
            this.devTools = devTools;
        }

        public async Task Enable()
        {
            devTools?.Session.Runtime.SubscribeToExecutionContextCreatedEvent(OnContextCreatedEvent);
            devTools?.Session.Runtime.SubscribeToExecutionContextDestroyedEvent(OnContextDestroyedEvent);
            devTools?.Session.Runtime.SubscribeToExecutionContextsClearedEvent(OnContextsClearedEvent);
            devTools?.Session.Page.SubscribeToFrameNavigatedEvent(OnFrameNavigatedEvent);
            await devTools?.Session.Runtime.Enable(new BaristaLabs.ChromeDevTools.Runtime.Runtime.EnableCommand());
            await devTools?.Session.Page.Enable(new BaristaLabs.ChromeDevTools.Runtime.Page.EnableCommand());
        }

        public long GetContextIdForFrame(string frame)
        {
            if (frameToContext.TryGetValue(frame, out long res)) return res;
            //throw new KeyNotFoundException(frame);
            return 0;
        }

        private void OnContextCreatedEvent(ExecutionContextCreatedEvent ev)
        {
            var auxData = ev.Context.AuxData as JObject;
            if (auxData != null)
            {
                var isDefault = auxData["isDefault"]?.Value<bool>();
                var frameId = auxData["frameId"]?.Value<string>();
                if (isDefault == true && !string.IsNullOrWhiteSpace(frameId))
                    frameToContext.TryAdd(frameId, ev.Context.Id);
            }
        }

        private void OnContextDestroyedEvent(ExecutionContextDestroyedEvent ev)
        {
            var itemsToRemove = frameToContext.Where(v => v.Value == ev.ExecutionContextId);
            foreach (var item in itemsToRemove)
                frameToContext.TryRemove(item.Key, out long context);
        }

        private void OnContextsClearedEvent(ExecutionContextsClearedEvent ev)
        {
            frameToContext.Clear();
        }

        private void OnFrameNavigatedEvent(FrameNavigatedEvent obj)
        {
            frameToContext.Clear();
        }
    }
}
