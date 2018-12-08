using WebSocketSharp;
using WebSocketSharp.Server;

namespace Zu.ChromeWebSocketProxy
{
    public class ChromeEndpoint : WebSocketBehavior
    {
        public string ChromeEndpointUri { get; set; }
        public ProxyWS ProxyWS { get; set; }

        protected async override void OnMessage(MessageEventArgs e)
        {
            if (ProxyWS.ProxyChromeSession == null)
            {
                ProxyWS.ProxyChromeSession = new ProxyChromeSession(ChromeEndpointUri);
                ProxyWS.ProxyChromeSession.OnEvent += ProxyChromeSession_OnEvent;
            }
            var res = await ProxyWS.ProxyChromeSession.SendCommand(e.Data).ConfigureAwait(false);
            try
            {
                Send(res);
            }
            catch { }
        }

        private void ProxyChromeSession_OnEvent(object sender, string e)
        {
            Sessions.Broadcast(e);
        }

        protected override void OnOpen()
        {
            //var user = Context.UserEndPoint;
            if (ProxyWS.OnlyLocalConnections && !Context.IsLocal) Context.WebSocket.Close();
        }

    }
}
