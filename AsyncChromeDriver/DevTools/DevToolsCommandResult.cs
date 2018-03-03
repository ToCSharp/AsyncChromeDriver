using Newtonsoft.Json.Linq;

namespace Zu.Chrome.DevTools
{
    public class DevToolsCommandResult
    {
        public int Id { get; set; }
        public JToken Result { get; set; }
        public string Error { get; set; }
    }
}
