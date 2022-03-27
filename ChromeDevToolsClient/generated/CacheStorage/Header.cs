namespace Zu.ChromeDevTools.CacheStorage
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Header
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