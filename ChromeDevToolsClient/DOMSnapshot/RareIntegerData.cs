namespace Zu.ChromeDevTools.DOMSnapshot
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class RareIntegerData
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