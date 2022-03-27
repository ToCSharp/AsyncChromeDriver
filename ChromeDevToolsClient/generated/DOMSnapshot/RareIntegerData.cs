namespace Zu.ChromeDevTools.DOMSnapshot
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class RareIntegerData
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