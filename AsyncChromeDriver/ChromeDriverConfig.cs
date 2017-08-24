// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace Zu.Chrome
{
    public class ChromeDriverConfig
    {
        public string UserDir { get; set; }
        public bool IsTempUserDir { get; set; } = true;
        public int TempDirCreateDelay { get; set; } = 3000;
        public int Port { get; set; } = 0;
        public bool IsHeadless { get; set; } = false;
        public Tuple<int, int> WindowSize { get; set; } = null;
        public bool DoNotOpenChromeProfile { get; set; } = false;

        public ChromeDriverConfig SetUserDir(string userDir)
        {
            IsTempUserDir = false;
            UserDir = userDir;
            return this;
        }
        public ChromeDriverConfig SetIsTempUserDir(bool isTempUserDir = true)
        {
            IsTempUserDir = isTempUserDir;
            if(IsTempUserDir) UserDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return this;
        }
        public ChromeDriverConfig SetPort(int port)
        {
            Port = port;
            return this;
        }
        public ChromeDriverConfig SetHeadless(bool headless = true)
        {
            IsHeadless = headless;
            if (IsHeadless && WindowSize == null) WindowSize = Tuple.Create(1200, 900);
            return this;
        }
        public ChromeDriverConfig SetWindowSize(int width, int height)
        {
            WindowSize = Tuple.Create(width, height);
            return this;
        }
        public ChromeDriverConfig SetDoNotOpenChromeProfile(bool doNotOpenChromeProfile = true)
        {
            DoNotOpenChromeProfile = doNotOpenChromeProfile;
            return this;
        }
        public ChromeDriverConfig SetTempDirCreateDelay(int delay)
        {
            TempDirCreateDelay = delay;
            return this;
        }
    }
}