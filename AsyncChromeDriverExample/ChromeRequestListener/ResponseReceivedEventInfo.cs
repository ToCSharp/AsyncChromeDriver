using BaristaLabs.ChromeDevTools.Runtime.Network;

namespace AsyncChromeDriverExample
{
    internal class ResponseReceivedEventInfo
    {
        public ResponseReceivedEvent Event { get; private set; }

        public ResponseReceivedEventInfo(ResponseReceivedEvent ev)
        {
            Event = ev;
        }

        public override string ToString()
        {
            return Event.Response?.Url;
        }
    }
}