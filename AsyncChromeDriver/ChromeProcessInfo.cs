// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Zu.Chrome
{
    public class ChromeProcessInfo
    {
        public Process Proc { get; set; }
        public ProcessWithJobObject ProcWithJobObject { get; set; }
        public string UserDir { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return UserDir + " " + Port;
        }
    }
}