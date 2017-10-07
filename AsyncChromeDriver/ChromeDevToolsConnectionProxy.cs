// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using BaristaLabs.ChromeDevTools.Runtime;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Zu.ChromeWebSocketProxy;

namespace Zu.Chrome
{
    public class ChromeDevToolsConnectionProxy: ChromeDevToolsConnection
    {
        private ChromeWSProxyConfig wsProxyConfig;
        private string proxyEndpointUrl;

        public ChromeDevToolsConnectionProxy(int port = 5999, int port2 = 5888)
            :base(port)
        {
            Port2 = port2;
        }

        public ChromeDevToolsConnectionProxy(int port = 5999, int port2 = 5888, ChromeWSProxyConfig wsProxyConfig = null) : this(port, port2)
        {
            this.wsProxyConfig = wsProxyConfig;
        }

        public int Port2 { get; set; }
        public ProxyWS Proxy { get; private set; }
        //public bool Connected { get; set; } = false;

        public override async Task Connect()
        {
            var sessions = await GetSessions(Port);
            var endpointUrl = sessions.FirstOrDefault(s => s.Type == "page")?.WebSocketDebuggerUrl;

            proxyEndpointUrl = await OpenProxyWS(endpointUrl);

            Session = new ChromeSession(proxyEndpointUrl);
        }

        private async Task<string> OpenProxyWS(string endpointUrl)
        {
            if (Proxy == null)
            {
                Proxy = new ProxyWS(endpointUrl, Port2);
                await Proxy.Open();
            }
            return Proxy.ProxyEndpointUrl;
        }
    }
}
