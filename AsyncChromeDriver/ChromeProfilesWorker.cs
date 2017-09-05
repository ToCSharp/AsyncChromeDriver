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

        public static ChromeProcessInfo OpenChromeProfile(string userDir, int port = 5999, bool isHeadless = false, WebSize windowSize = null)
        {
            return OpenChromeProfile(new ChromeDriverConfig { UserDir = userDir, Port = port, Headless = isHeadless, WindowSize = windowSize });
        }

        public static ChromeProcessInfo OpenChromeProfile(ChromeDriverConfig config)
        {
            //if (string.IsNullOrWhiteSpace(userDir)) throw new ArgumentNullException(nameof(userDir));
            if (config.Port < 1 || config.Port > 65000) throw new ArgumentOutOfRangeException(nameof(config.Port));
            bool firstRun = false;
            if (!string.IsNullOrWhiteSpace(config.UserDir) && !Directory.Exists(config.UserDir))
            {
                firstRun = true;
                Directory.CreateDirectory(config.UserDir);
            }

            var args = "--remote-debugging-port=" + config.Port
                + (string.IsNullOrWhiteSpace(config.UserDir) ? "" : " --user-data-dir=\"" + config.UserDir + "\"")
                + (firstRun ? " --bwsi --no-first-run" : "")
                + (config.Headless ? " --headless --disable-gpu" : "")
                + (config.WindowSize != null ? $" --window-size={config.WindowSize.Width},{config.WindowSize.Height}" : "")
                + (string.IsNullOrWhiteSpace(config.CommandLineArgumets) ? "" : " " + config.CommandLineArgumets);


            if (config.Headless)
            {
                var process = new ProcessWithJobObject();
                process.StartProc(ChromeBinaryFileName, args);
                Thread.Sleep(1000);
                return new ChromeProcessInfo { ProcWithJobObject = process, UserDir = config.UserDir, Port = config.Port };
            }
            else
            {
                var process = new Process();
                process.StartInfo.FileName = ChromeBinaryFileName;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                Thread.Sleep(1000);
                return new ChromeProcessInfo { Proc = process, UserDir = config.UserDir, Port = config.Port };
            }
           
        }

    }
}
