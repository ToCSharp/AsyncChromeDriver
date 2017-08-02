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


        public static ChromeProcessInfo OpenChromeProfile(string userDir, int port = 5999)
        {
            if (string.IsNullOrWhiteSpace(userDir)) throw new ArgumentNullException(nameof(userDir));
            if (port < 1 || port > 65000) throw new ArgumentOutOfRangeException(nameof(port));
            bool firstRun = false;
            if(!Directory.Exists(userDir))
            {
                firstRun = true;
                Directory.CreateDirectory(userDir);
            }
            var process = new Process();
            process.StartInfo.FileName = ChromeBinaryFileName; 
            process.StartInfo.Arguments = "--remote-debugging-port=" + port + " " 
                + "--user-data-dir=\"" + userDir + "\"" 
                + (firstRun ? " --bwsi --no-first-run" : "");
            process.StartInfo.UseShellExecute = false;
            //process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            Thread.Sleep(1000);

            //// wait for closing previos Firefox
            //if (process.MainWindowTitle != "" && !process.MainWindowTitle.EndsWith("Google Chrome"))
            //{
            //    var reader = process.StandardOutput;
            //    var v = reader.ReadToEnd();
            //}
            return new ChromeProcessInfo { Proc = process, UserDir = userDir, Port = port } ;
        }

    }
}
