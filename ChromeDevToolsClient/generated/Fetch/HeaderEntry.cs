namespace Zu.ChromeDevTools.Fetch
{
    using Newtonsoft.Json;

    /// <summary>
    /// Response HTTP header entry
    /// </summary>
    public sealed class HeaderEntry
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