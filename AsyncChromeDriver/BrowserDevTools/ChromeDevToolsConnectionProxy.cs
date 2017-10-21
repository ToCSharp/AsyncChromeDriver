// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using BaristaLabs.ChromeDevTools.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Zu.ChromeWebSocketProxy;

namespace Zu.Chrome.BrowserDevTools
{
    public class ChromeDevToolsConnectionProxy : ChromeDevToolsConnection
    {
        private ChromeWSProxyConfig wsProxyConfig;
        private HTTPServer httpServer;
        private string proxyEndpointUrl;
        public ChromeDevToolsConnectionProxy(int port = 5999, int port2 = 5888): base (port)
        {
            Port2 = port2;
        }

        static Random rnd = new Random();
        public ChromeDevToolsConnectionProxy(int port = 5999, int port2 = 5888, ChromeWSProxyConfig wsProxyConfig = null): this (port, port2)
        {
            this.wsProxyConfig = wsProxyConfig ?? new ChromeWSProxyConfig();
            if (this.wsProxyConfig.HTTPServerPort == 0)
                this.wsProxyConfig.HTTPServerPort = 18000 + rnd.Next(3000);
        }

        public int Port2
        {
            get;
            set;
        }

        public ProxyWS Proxy
        {
            get;
            private set;
        }

        //public bool Connected { get; set; } = false;
        public override async Task Connect()
        {
            var sessions = await GetSessions(Port).ConfigureAwait(false);
            var endpointUrl = sessions.FirstOrDefault(s => s.Type == "page")?.WebSocketDebuggerUrl;
            if (endpointUrl == null)
                throw new Exception("Cannot get page session from Chrome");
            //var dir = wsProxyConfig.DevToolsFilesDir ?? Directory.GetCurrentDirectory();
            //await LoadDevToolsFiles(dir);
            if (wsProxyConfig?.DoProxyHttpTraffic == true)
            {
                if (wsProxyConfig.DevToolsFilesDir == null)
                    wsProxyConfig.DevToolsFilesDir = Directory.GetCurrentDirectory();
                wsProxyConfig.ChromePort = Port;
                httpServer = new HTTPServer(wsProxyConfig); // dir, wsProxyConfig.HTTPServerPort, Port);
            }

            proxyEndpointUrl = await OpenProxyWS(endpointUrl).ConfigureAwait(false);
            Session = new ChromeSession(proxyEndpointUrl);
        }

        //private async Task LoadDevToolsFiles(string devToolsFilesDir)
        //{
        //    foreach (var filename in devToolsFiles)
        //    {
        //        var filePath = Path.Combine(devToolsFilesDir, filename.Replace("/", "\\").TrimStart('\\'));
        //        if (File.Exists(filePath)) continue;
        //        try
        //        {
        //            var webClient = new HttpClient();
        //            var uriBuilder = new UriBuilder { Scheme = "http", Host = "127.0.0.1", Port = Port, Path = filename };
        //            var doc = await webClient.GetStringAsync(uriBuilder.Uri);
        //            var dir = Path.GetDirectoryName(filePath);
        //            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        //            File.WriteAllText(filePath, doc);
        //        }
        //        catch { }
        //    }
        //}
        private async Task<string> OpenProxyWS(string endpointUrl)
        {
            if (Proxy == null)
            {
                Proxy = new ProxyWS(endpointUrl, Port2);
                await Proxy.Open().ConfigureAwait(false);
            }

            return Proxy.ProxyEndpointUrl;
        }

        public void Close()
        {
            httpServer?.Stop();
            Proxy?.Stop();
        }

        public override void Disconnect()
        {
            Close();
            Session?.Dispose();
            Session = null;
        }
    //List<string> devToolsFiles = new List<string>
    //{
    //    "devtools/inspector.html",
    //    "devtools/inspector.js",
    //    "devtools/Images/treeoutlineTriangles.png",
    //    "devtools/inline_editor/inline_editor_module.js",
    //    "devtools/color_picker/color_picker_module.js",
    //    "devtools/object_ui/object_ui_module.js",
    //    "devtools/Images/largeIcons.png",
    //    "devtools/event_listeners/event_listeners_module.js",
    //    //"favicon.ico",
    //    "devtools/elements/elements_module.js",
    //    "devtools/data_grid/data_grid_module.js",
    //    "devtools/console/console_module.js",
    //    "devtools/cm/cm_module.js",
    //    "devtools/formatter/formatter_module.js",
    //    "devtools/diff/diff_module.js",
    //    "devtools/text_editor/text_editor_module.js",
    //    "devtools/workspace_diff/workspace_diff_module.js",
    //    "devtools/quick_open/quick_open_module.js",
    //    "devtools/source_frame/source_frame_module.js",
    //    "devtools/snippets/snippets_module.js",
    //    "devtools/sources/sources_module.js"
    //};
    }
}