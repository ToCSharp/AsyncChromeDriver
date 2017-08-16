using Newtonsoft.Json.Linq;

namespace Zu.Chrome.DevTools
{
    public class DevToolsCommandData
    {
        public int Id { get; set; }
        public string BrowserId { get; set; }
        public string CommandName { get; set; }
        public JToken Params { get; set; }
        public int? MillisecondsTimeout { get; set; }

    }
}
