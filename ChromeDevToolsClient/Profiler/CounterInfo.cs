namespace Zu.ChromeDevTools.Profiler
{
    using Newtonsoft.Json;

    /// <summary>
    /// Collected counter information.
    /// </summary>
    public sealed class CounterInfo
    {
        /// <summary>
        /// Counter name.
        ///</summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Counter value.
        ///</summary>
        [JsonProperty("value")]
        public long Value
        {
            get;
            set;
        }
    }
}