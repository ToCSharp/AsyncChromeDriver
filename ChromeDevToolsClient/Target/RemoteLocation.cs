namespace Zu.ChromeDevTools.Target
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class RemoteLocation
    {
        /// <summary>
        /// Gets or sets the host
        /// </summary>
        [JsonProperty("host")]
        public string Host
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the port
        /// </summary>
        [JsonProperty("port")]
        public long Port
        {
            get;
            set;
        }
    }
}