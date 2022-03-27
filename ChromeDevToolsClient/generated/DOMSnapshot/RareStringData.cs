namespace Zu.ChromeDevTools.DOMSnapshot
{
    using Newtonsoft.Json;

    /// <summary>
    /// Data that is only present on rare nodes.
    /// </summary>
    public sealed class RareStringData
    {
        /// <summary>
        /// Gets or sets the index
        /// </summary>
        [JsonProperty("index")]
        public long[] Index
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [JsonProperty("value")]
        public long[] Value
        {
            get;
            set;
        }
    }
}