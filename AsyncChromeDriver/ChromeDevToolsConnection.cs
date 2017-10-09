// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using BaristaLabs.ChromeDevTools.Runtime;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zu.Chrome
{
    public class ChromeDevToolsConnection
    {
        public int Port { get; private set; }
        public virtual ChromeSession Session { get; set; }

        public ChromeDevToolsConnection(int port = 5999)
        {
            Port = port;
        }

        public virtual async Task Connect()
        {
            var sessions = await GetSessions(Port);
            var endpointUrl = sessions.FirstOrDefault(s => s.Type == "page")?.WebSocketDebuggerUrl;
            Session = new ChromeSession(endpointUrl);
        }

        // todo check
        public async Task<ChromeSessionInfo[]> GetSessions(int port)
        {
            //var remoteSessionUrls = new List<string>();
            var webClient = new HttpClient();
            var uriBuilder = new UriBuilder { Scheme = "http", Host = "127.0.0.1", Port = port, Path = "/json" };
            var remoteSessions = await webClient.GetStringAsync(uriBuilder.Uri);
            try
            {
                //return JsonConvert.DeserializeObject<RemoteSession[]>(remoteSessions); 
                return JsonConvert.DeserializeObject<ChromeSessionInfo[]>(remoteSessions);
            }
            catch (Exception ex)
            { return null; }
        }
        public virtual void Disconnect()
        {
            Session?.Dispose();
            Session = null;
        }

        ////todo
        
    }
}
