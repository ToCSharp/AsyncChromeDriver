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
        public bool w3c_compliant = false;
        public bool quit = false;
        public bool detach = false;
        public bool force_devtools_screenshot = false;
        public int sticky_modifiers = 0;
        public WebPoint mouse_position = new WebPoint(0, 0);
        public TimeSpan page_load_timeout = TimeSpan.FromMinutes(5);
        public bool auto_reporting_enabled = false;
        //todo: implicit or await for element, element state...
        internal TimeSpan implicitWait;

        public Session(int id)
        {
            Id = id;
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
