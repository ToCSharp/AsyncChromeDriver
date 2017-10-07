using System;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Zu.ChromeWebSocketProxy
{
    public class ChromeEndpoint : WebSocketBehavior
    {
        public string ChromeEndpointUri { get; set; }

        public bool Connected = false;
        private ProxyChromeSession proxyChromeSession;

        protected async override void OnMessage(MessageEventArgs e)
        {
            //CreateChromeSession();
            if (proxyChromeSession == null)
            {
                proxyChromeSession = new ProxyChromeSession(ChromeEndpointUri);
                proxyChromeSession.OnEvent += ProxyChromeSession_OnEvent;
            }
            var res = await proxyChromeSession.SendCommand(e.Data);
            Send(res);
        }

        private void ProxyChromeSession_OnEvent(object sender, string e)
        {
            Sessions.Broadcast(e);
        }
    }
}
