namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// Post data entry for HTTP request
    /// </summary>
    public sealed class PostDataEntry
    {
        /// <summary>
        /// Gets or sets the bytes
        /// </summary>
        [JsonProperty("bytes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Bytes
        {
            get;
            set;
        }
    }
}