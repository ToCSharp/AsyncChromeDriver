// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.

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

        private AsyncChromeDriver asyncChromeDriver;
        public bool w3c_compliant = false;
        public bool quit = false;
        public bool detach = false;
        public bool force_devtools_screenshot = false;
        public int sticky_modifiers = 0;
        public WebPoint mouse_position = new WebPoint(0, 0);
        public bool auto_reporting_enabled = false;
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
            get => ((ChromeDriverTimeouts)asyncChromeDriver.Options.Timeouts).AsynchronousJavaScript;
            set => ((ChromeDriverTimeouts)asyncChromeDriver.Options.Timeouts).AsynchronousJavaScript = value;
        }


        public Session(int id, AsyncChromeDriver asyncChromeDriver)
        {
            Id = id;
            this.asyncChromeDriver = asyncChromeDriver;
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

    }
}
