namespace Zu.ChromeDevTools.Cast
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Sink
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        [JsonProperty("id")]
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// Text describing the current session. Present only if there is an active
        /// session on the sink.
        ///</summary>
        [JsonProperty("session", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Session
        {
            get;
            set;
        }
    }
}