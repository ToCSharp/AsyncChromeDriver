using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Zu.ChromeWebSocketProxy
{
    public class ProxyWS : IDisposable
    {
        private int port;
        private string endpointUrl;
        private WebSocketServer wsServer;

        public string ProxyEndpointUrl { get; set; }
        public string ProxyPath { get; set; } = "WSProxy";
        public ProxyChromeSession ProxyChromeSession { get; internal set; }

        public bool OnlyLocalConnections { get; set; } = true;

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

        public async Task Open()
        {
            await Task.Run(() =>
                        {
                            wsServer = new WebSocketServer(port);
                            wsServer.AddWebSocketService<ChromeEndpoint>("/" + ProxyPath, (a) => { a.ChromeEndpointUri = endpointUrl; a.ProxyWS = this; });
                            wsServer.Start();
                        }).ConfigureAwait(false);
        }

        public void Stop()
        {
            /*ChromeEndpoint.*/
            StopSession();
            wsServer?.Stop();
        }

        private void StopSession()
        {
            ProxyChromeSession?.Dispose();
            ProxyChromeSession = null;
        }

        #region IDisposable Support
        private bool m_isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!m_isDisposed)
            {
                if (disposing)
                {
                    Stop();
                }

                m_isDisposed = true;
            }
        }

        /// <summary>
        /// Disposes of the ChromeSession and frees all resources.
        ///</summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

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
