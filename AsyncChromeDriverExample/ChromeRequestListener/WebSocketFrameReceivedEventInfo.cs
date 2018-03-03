
using Zu.ChromeDevTools.Network;

namespace AsyncChromeDriverExample
{
    internal class WebSocketFrameReceivedEventInfo
    {
        public WebSocketFrameReceivedEvent Event { get; private set; }

        public WebSocketFrameReceivedEventInfo(WebSocketFrameReceivedEvent ev)
        {
            Event = ev;
        }

        public override string ToString()
        {
            return Event.Response?.PayloadData;
        }
    }
}