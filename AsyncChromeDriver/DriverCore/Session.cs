// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome.DriverCore
{
    public class Session
    {
        public Stack<FrameInfo> Frames = new Stack<FrameInfo>();

        public int Id { get; set; }

        private AsyncChromeDriver _asyncChromeDriver;
        public bool W3CCompliant = false;
        public bool Quit = false;
        public bool Detach = false;
        public bool ForceDevtoolsScreenshot = false;
        public int StickyModifiers = 0;
        public WebPoint MousePosition = new WebPoint(0, 0);
        public bool AutoReportingEnabled = false;
        //todo: implicit or await for element, element state...
        public TimeSpan PageLoadTimeout
        {
            get;//=> asyncChromeDriver.Options.Timeouts.PageLoad;
            set;// => asyncChromeDriver.Options.Timeouts.PageLoad = value;
        }
        public TimeSpan ImplicitWait
        {
            get;// => asyncChromeDriver.Options.Timeouts.ImplicitWait;
            set;// => asyncChromeDriver.Options.Timeouts.ImplicitWait = value;
        }
        public TimeSpan ScriptTimeout
        {
            get => ((ChromeDriverTimeouts)_asyncChromeDriver.Options.Timeouts).AsynchronousJavaScript;
            set => ((ChromeDriverTimeouts)_asyncChromeDriver.Options.Timeouts).AsynchronousJavaScript = value;
        }


        public Session(int id, AsyncChromeDriver asyncChromeDriver)
        {
            Id = id;
            _asyncChromeDriver = asyncChromeDriver;
        }

        public void SwitchToTopFrame()
        {
            Frames.Clear();
        }

        public void SwitchToParentFrame()
        {
            if (Frames.Any())
                Frames.Pop();
        }

        public void SwitchToSubFrame(string frameId, string cromeFrameId)
        {
            string parentFrameId = "";
            if (Frames.Any())
                parentFrameId = Frames.Peek().FrameId;
            Frames.Push(new FrameInfo(frameId, parentFrameId, cromeFrameId));
        }

        public string GetCurrentFrameId()
        {
            if (!Frames.Any())
                return "";
            return Frames.Peek().FrameId;
        }

        public string GetElementKey()
        {
            if (W3CCompliant == true)
                return ElementKeys.ElementKeyW3C;
            else
                return ElementKeys.ElementKey;
        }

        public JProperty GetElementJson(string elementId) => new JProperty(GetElementKey(), elementId);

        public string GetElementJsonString(string elementId) => $"{{\"{GetElementKey()}\":\"{elementId}\"}}";

    }
}
