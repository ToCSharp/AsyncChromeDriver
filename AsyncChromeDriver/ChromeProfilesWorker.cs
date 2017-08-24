// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
            return OpenChromeProfile(config.UserDir, config.Port, config.IsHeadless, config.WindowSize);
        }

        public static ChromeProcessInfo OpenChromeProfile(string userDir, int port = 5999, bool isHeadless = false, Tuple<int, int> windowSize = null)
        {
            //if (string.IsNullOrWhiteSpace(userDir)) throw new ArgumentNullException(nameof(userDir));
            if (port < 1 || port > 65000) throw new ArgumentOutOfRangeException(nameof(port));
            bool firstRun = false;
            if (!string.IsNullOrWhiteSpace(userDir) && !Directory.Exists(userDir))
            {
                firstRun = true;
                Directory.CreateDirectory(userDir);
            }
            //var process = new Process();
            //process.StartInfo.FileName = ChromeBinaryFileName;
            //process.StartInfo.Arguments = "--remote-debugging-port=" + port + " "
            //    + "--user-data-dir=\"" + userDir + "\""
            //    + (firstRun ? " --bwsi --no-first-run" : "")
            //    + (isHeadless ? " --headless --disable-gpu" : "")
            //    + (windowSize != null ? $" --window-size={windowSize.Item1},{windowSize.Item2}" : "");
            var args = "--remote-debugging-port=" + port + " "
                + (string.IsNullOrWhiteSpace(userDir) ? "" : "--user-data-dir=\"" + userDir + "\"")
                + (firstRun ? " --bwsi --no-first-run" : "")
                + (isHeadless ? " --headless --disable-gpu" : "")
                + (windowSize != null ? $" --window-size={windowSize.Item1},{windowSize.Item2}" : "");
            var process = new ProcessWithJobObject();
            process.StartProc(ChromeBinaryFileName, args);

            //process.StartInfo.UseShellExecute = false;
            ////process.StartInfo.RedirectStandardOutput = true;

            //process.Start();

            Thread.Sleep(1000);

            //// wait for closing previos Firefox
            //if (process.MainWindowTitle != "" && !process.MainWindowTitle.EndsWith("Google Chrome"))
            //{
            //    var reader = process.StandardOutput;
            //    var v = reader.ReadToEnd();
            //}
            return new ChromeProcessInfo { ProcWithJobObject = process, UserDir = userDir, Port = port };
        }

    }
}
