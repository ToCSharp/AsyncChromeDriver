using System;
using System.Threading.Tasks;
using BaristaLabs.ChromeDevTools.Runtime.Network;
using Zu.Chrome;

namespace AsyncChromeDriverExample
{
    internal class ChromeRequestListener
    {
        private AsyncChromeDriver asyncChromeDriver;

        public event EventHandler<ResponseReceivedEventInfo> ResponseReceived;
        public event EventHandler<WebSocketFrameReceivedEventInfo> WebSocketFrameReceived;

        public bool DoListenResponse { get; set; } = true;
        public bool DoListenWebSocket { get; set; } = true;

        public ChromeRequestListener(AsyncChromeDriver asyncChromeDriver)
        {
            this.asyncChromeDriver = asyncChromeDriver;
        }

        public async Task StartListen()
        {
            asyncChromeDriver.DevTools.Session.Network.SubscribeToResponseReceivedEvent(OnResponseReceivedEvent);
            asyncChromeDriver.DevTools.Session.Network.SubscribeToWebSocketFrameReceivedEvent(OnWebSocketFrameReceivedEvent);
            await asyncChromeDriver.DevTools.Session.Network.Enable(new EnableCommand());
        }

        public async Task StopListen()
        {
            await asyncChromeDriver.DevTools.Session.Network.Disable(new DisableCommand());
        }

        private void OnResponseReceivedEvent(ResponseReceivedEvent obj)
        {
            ResponseReceived?.Invoke(this, new ResponseReceivedEventInfo(obj));
        }

        private void OnWebSocketFrameReceivedEvent(WebSocketFrameReceivedEvent obj)
        {
            WebSocketFrameReceived?.Invoke(this, new WebSocketFrameReceivedEventInfo(obj));
        }

        public Task<string> GetResponseBody(ResponseReceivedEventInfo responseReceivedEventInfo)
        {
            return GetResponseBody(responseReceivedEventInfo?.Event?.RequestId);
        }

        public async Task<string> GetResponseBody(string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId)) return null;
            try
            {
                var res = await asyncChromeDriver.DevTools.Session.Network.GetResponseBody(new GetResponseBodyCommand
                {
                    RequestId = requestId
                });
                if (res.Base64Encoded)
                {
                    return System.Text.Encoding.Default.GetString(Convert.FromBase64String(res.Body));
                }
                return res.Body;
            }
            catch (Exception ex) { return ex.ToString(); }
        }
        public Task<Cookie[]> GetCookies(ResponseReceivedEventInfo responseReceivedEventInfo)
        {
            if (responseReceivedEventInfo == null) return null;
            return GetCookies(new string[] { responseReceivedEventInfo?.Event?.Response.Url });
        }

        public async Task<Cookie[]> GetCookies(string[] urls)
        {
            if (urls == null || urls.Length == 0) return null;
            var res = await asyncChromeDriver.DevTools.Session.Network.GetCookies(new GetCookiesCommand
            {
                Urls = urls
            });

            return res.Cookies;
        }

        public async Task<Cookie[]> GetAllCookies()
        {
            var res = await asyncChromeDriver.DevTools.Session.Network.GetAllCookies();
            return res.Cookies;
        }
    }
}