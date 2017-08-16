using Newtonsoft.Json.Linq;

namespace Zu.Chrome.DevTools
{
    public class DevToolsEventData
    {
        public string EventName { get; set; }
        public JToken Data { get; set; }
    }
}
