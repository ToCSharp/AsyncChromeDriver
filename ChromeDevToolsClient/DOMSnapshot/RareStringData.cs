namespace Zu.ChromeDevTools.DOMSnapshot
{
    using Newtonsoft.Json;

    /// <summary>
    /// Data that is only present on rare nodes.
    /// </summary>
    public sealed class RareStringData
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("index")]
        public long[] Index
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("value")]
        public long[] Value
        {
            get;
            set;
        }
    }
}