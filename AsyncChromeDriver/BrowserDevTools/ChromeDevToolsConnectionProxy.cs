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

        public int Port2 { get; set; }

        public ProxyWS Proxy  { get; private set; }

        public ChromeDevToolsConnectionProxy(int port = 5999, int port2 = 5888) : base(port)
        {
            Port2 = port2;
        }

        static Random rnd = new Random();
        public ChromeDevToolsConnectionProxy(int port = 5999, int port2 = 5888, ChromeWSProxyConfig wsProxyConfig = null) : this(port, port2)
        {
            this.wsProxyConfig = wsProxyConfig ?? new ChromeWSProxyConfig();
            if (this.wsProxyConfig.HTTPServerPort == 0)
                this.wsProxyConfig.HTTPServerPort = 18000 + rnd.Next(3000);
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

        private async Task<string> OpenProxyWS(string endpointUrl)
        {
            if (Proxy == null)
            {
                Proxy = new ProxyWS(endpointUrl, Port2);
                Proxy.OnlyLocalConnections = wsProxyConfig.OnlyLocalConnections;
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

    }
}