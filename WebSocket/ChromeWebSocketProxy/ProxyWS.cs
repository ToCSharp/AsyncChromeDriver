using System;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Zu.ChromeWebSocketProxy
{
    public class ProxyWS : IDisposable
    {
        private int _port;
        private string _endpointUrl;
        private WebSocketServer _wsServer;

        public string ProxyEndpointUrl { get; set; }
        public string ProxyPath { get; set; } = "WSProxy";
        public ProxyChromeSession ProxyChromeSession { get; internal set; }

        public bool OnlyLocalConnections { get; set; } = true;

        public ProxyWS(string endpointUrl, int port)
        {
            _port = port;
            _endpointUrl = endpointUrl;
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
                            _wsServer = new WebSocketServer(_port);
                            _wsServer.AddWebSocketService<ChromeEndpoint>("/" + ProxyPath, (a) => { a.ChromeEndpointUri = _endpointUrl; a.ProxyWS = this; });
                            _wsServer.Start();
                        }).ConfigureAwait(false);
        }

        public void Stop()
        {
            /*ChromeEndpoint.*/
            StopSession();
            _wsServer?.Stop();
        }

        private void StopSession()
        {
            ProxyChromeSession?.Dispose();
            ProxyChromeSession = null;
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Stop();
                }

                _isDisposed = true;
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
