using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zu.Chrome.DevTools
{
    public class DevToolsCommandResult
    {
        public int Id { get; set; }
        public JToken Result { get; set; }
        public string Error { get; set; }
    }
}
