using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Zu.ChromeWebSocketProxy
{
    public class ProxyWS
    {
        private int port;
        private string endpointUrl;
        private WebSocketServer wsServer;

        public ProxyWS(string endpointUrl, int port)
        {
            this.port = port;
            this.endpointUrl = endpointUrl;
            var uri = new Uri(endpointUrl);
            var builder = new UriBuilder(uri);
            builder.Port = port;
            builder.Path = ProxyPath;
            //uri = uri.SetPort(port);
            ProxyEndpointUrl = builder.ToString();
        }

        public string ProxyEndpointUrl { get; set; }
        public string ProxyPath { get; set; } = "WSProxy";

        public async Task Open()
        {
            await Task.Run(() =>
            {
                wsServer = new WebSocketServer(port);
                wsServer.AddWebSocketService<ChromeEndpoint>("/" + ProxyPath, (a) => { a.ChromeEndpointUri = endpointUrl; });
                wsServer.Start();
            });
        }

    }

    //public static class UriExtensions
    //{
    //    public static Uri SetPort(this Uri uri, int newPort)
    //    {
    //        var builder = new UriBuilder(uri);
    //        builder.Port = newPort;
    //        return builder.Uri;
    //    }
    //}
}
