namespace Zu.ChromeDevTools.Emulation
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class MediaFeature
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
        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }
    }
}