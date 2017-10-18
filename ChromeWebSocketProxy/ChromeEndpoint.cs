using System;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Zu.ChromeWebSocketProxy
{
    public class ChromeEndpoint : WebSocketBehavior
    {
        public string ChromeEndpointUri { get; set; }
        public ProxyWS ProxyWS { get; set; }

        public bool Connected = false;
        //private ProxyChromeSession proxyChromeSession => ProxyWS;

        protected async override void OnMessage(MessageEventArgs e)
        {
            //CreateChromeSession();
            if (ProxyWS.ProxyChromeSession == null)
            {
                ProxyWS.ProxyChromeSession = new ProxyChromeSession(ChromeEndpointUri);
                ProxyWS.ProxyChromeSession.OnEvent += ProxyChromeSession_OnEvent;
            }
            var res = await ProxyWS.ProxyChromeSession.SendCommand(e.Data);
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

        //public static void StopSession()
        //{
        //    proxyChromeSession?.Dispose();
        //    proxyChromeSession = null;
        //}
    }
}
