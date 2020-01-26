namespace Zu.ChromeDevTools.Media
{
    using Newtonsoft.Json;

    /// <summary>
    /// Player Property type
    /// </summary>
    public sealed class PlayerProperty
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
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Value
        {
            get;
            set;
        }
    }
}