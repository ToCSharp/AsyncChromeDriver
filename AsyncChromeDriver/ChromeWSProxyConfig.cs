// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Chrome
{
    public class ChromeWSProxyConfig
    {
        public bool DoProxyHttpTraffic { get; set; } = false;

        public string DevToolsFilesDir { get; set; }
        public int HTTPServerPort { get; set; } = 0;
        public int ChromePort { get; internal set; }

        bool httpServerSaveRequestedFiles = false;
        /// <summary>
        /// Saves requested DevTools frontend file to DevToolsFilesDir("devtools") if there no one
        /// </summary>
        public bool HTTPServerSaveRequestedFiles
        {
            get
            {
                return httpServerSaveRequestedFiles;
            }
            set
            {
                httpServerSaveRequestedFiles = value;
                if (httpServerSaveRequestedFiles) DoProxyHttpTraffic = true;
            }
        } 

        bool httpServerTryFindRequestedFileLocaly = false;
        /// <summary>
        /// If true tries to find requested file in DevToolsFilesDir, 
        /// if no request it from Chrome
        /// </summary>
        public bool HTTPServerTryFindRequestedFileLocaly
        {
            get
            {
                return httpServerTryFindRequestedFileLocaly;
            }
            set
            {
                httpServerTryFindRequestedFileLocaly = value;
                if (httpServerTryFindRequestedFileLocaly) DoProxyHttpTraffic = true;
            }
        }

    }
}