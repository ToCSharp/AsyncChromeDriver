// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.


namespace Zu.Chrome.DriverCore
{
    public class FrameInfo
    {
        public string ParentFrameId { get; set; }
        public string FrameId { get; set; }
        public string CromeFrameId { get; set; }

        public FrameInfo(string frameId, string parentFrameId, string cromeFrameId)
        {
            FrameId = frameId;
            ParentFrameId = parentFrameId;
            CromeFrameId = cromeFrameId;
        }

    }
}
