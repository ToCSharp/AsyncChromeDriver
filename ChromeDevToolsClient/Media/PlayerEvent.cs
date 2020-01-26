namespace Zu.ChromeDevTools.Media
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class PlayerEvent
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("type")]
        public PlayerEventType Type
        {
            get;
            set;
        }
        /// <summary>
        /// Events are timestamped relative to the start of the player creation
        /// not relative to the start of playback.
        ///</summary>
        [JsonProperty("timestamp")]
        public double Timestamp
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }
    }
}