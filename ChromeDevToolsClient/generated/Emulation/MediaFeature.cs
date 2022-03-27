namespace Zu.ChromeDevTools.Emulation
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class MediaFeature
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