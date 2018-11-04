namespace Zu.ChromeDevTools.Fetch
{
    using Newtonsoft.Json;

    /// <summary>
    /// Response HTTP header entry
    /// </summary>
    public sealed class HeaderEntry
    {
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