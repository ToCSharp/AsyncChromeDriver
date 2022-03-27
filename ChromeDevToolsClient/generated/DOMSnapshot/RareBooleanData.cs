namespace Zu.ChromeDevTools.DOMSnapshot
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class RareBooleanData
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
    }
}