// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.Chrome
{
    public class ChromeWSProxyConfig
    {
        public bool DoProxyHttpTraffic { get; set; } = false;

        public string DevToolsFilesDir { get; set; }
        public int HttpServerPort { get; set; } = 0;
        public int ChromePort { get; internal set; }

        public bool OnlyLocalConnections { get; set; } = true;

        bool _httpServerSaveRequestedFiles = false;
        /// <summary>
        /// Saves requested DevTools frontend file to DevToolsFilesDir("devtools") if there no one
        /// </summary>
        public bool HttpServerSaveRequestedFiles
        {
            get => _httpServerSaveRequestedFiles;
            set
            {
                _httpServerSaveRequestedFiles = value;
                if (_httpServerSaveRequestedFiles) DoProxyHttpTraffic = true;
            }
        } 

        bool _httpServerTryFindRequestedFileLocaly = false;
        /// <summary>
        /// If true tries to find requested file in DevToolsFilesDir, 
        /// if no request it from Chrome
        /// </summary>
        public bool HttpServerTryFindRequestedFileLocaly
        {
            get => _httpServerTryFindRequestedFileLocaly;
            set
            {
                _httpServerTryFindRequestedFileLocaly = value;
                if (_httpServerTryFindRequestedFileLocaly) DoProxyHttpTraffic = true;
            }
        }
    }
}