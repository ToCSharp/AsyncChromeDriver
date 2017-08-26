// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public static class ChromeProfilesWorker
    {
        static ChromeProfilesWorker()
        {
            if (File.Exists(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"))
                ChromeBinaryFileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            else if (File.Exists(@"C:\Program Files\Google\Chrome\Application\chrome.exe"))
                ChromeBinaryFileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        }

        public static string ChromeBinaryFileName { get; set; }

        public static ChromeProcessInfo OpenChromeProfile(ChromeDriverConfig config)
        {
            return OpenChromeProfile(config.UserDir, config.Port, config.Headless, config.WindowSize);
        }

        public static ChromeProcessInfo OpenChromeProfile(string userDir, int port = 5999, bool isHeadless = false, WebSize windowSize = null)
        {
            //if (string.IsNullOrWhiteSpace(userDir)) throw new ArgumentNullException(nameof(userDir));
            if (port < 1 || port > 65000) throw new ArgumentOutOfRangeException(nameof(port));
            bool firstRun = false;
            if (!string.IsNullOrWhiteSpace(userDir) && !Directory.Exists(userDir))
            {
                firstRun = true;
                Directory.CreateDirectory(userDir);
            }

            var args = "--remote-debugging-port=" + port
                + (string.IsNullOrWhiteSpace(userDir) ? "" : " --user-data-dir=\"" + userDir + "\"")
                + (firstRun ? " --bwsi --no-first-run" : "")
                + (isHeadless ? " --headless --disable-gpu" : "")
                + (windowSize != null ? $" --window-size={windowSize.Width},{windowSize.Height}" : "");


            if (isHeadless)
            {
                var process = new ProcessWithJobObject();
                process.StartProc(ChromeBinaryFileName, args);
                Thread.Sleep(1000);
                return new ChromeProcessInfo { ProcWithJobObject = process, UserDir = userDir, Port = port };
            }
            else
            {
                var process = new Process();
                process.StartInfo.FileName = ChromeBinaryFileName;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                Thread.Sleep(1000);
                return new ChromeProcessInfo { Proc = process, UserDir = userDir, Port = port };
            }
           
        }

    }
}
