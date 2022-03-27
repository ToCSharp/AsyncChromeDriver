namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// An options object that may be extended later to better support CORS,
    /// CORB and streaming.
    /// </summary>
    public sealed class LoadNetworkResourceOptions
    {
        /// <summary>
        /// Gets or sets the disableCache
        /// </summary>
        [JsonProperty("disableCache")]
        public bool DisableCache
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the includeCredentials
        /// </summary>
        [JsonProperty("includeCredentials")]
        public bool IncludeCredentials
        {
            get;
            set;
        }
    }
}