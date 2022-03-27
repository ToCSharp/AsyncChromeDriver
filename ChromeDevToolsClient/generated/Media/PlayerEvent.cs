namespace Zu.ChromeDevTools.Media
{
    using Newtonsoft.Json;

    /// <summary>
    /// Corresponds to kMediaEventTriggered
    /// </summary>
    public sealed class PlayerEvent
    {
        /// <summary>
        /// Gets or sets the timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public double Timestamp
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }
    }
}