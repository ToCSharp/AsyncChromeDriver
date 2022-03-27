namespace Zu.ChromeDevTools.Emulation
{
    using Newtonsoft.Json;

    /// <summary>
    /// Used to specify User Agent Cient Hints to emulate. See https://wicg.github.io/ua-client-hints
    /// Missing optional values will be filled in by the target with what it would normally use.
    /// </summary>
    public sealed class UserAgentMetadata
    {
        /// <summary>
        /// Gets or sets the brands
        /// </summary>
        [JsonProperty("brands", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserAgentBrandVersion[] Brands
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the fullVersionList
        /// </summary>
        [JsonProperty("fullVersionList", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserAgentBrandVersion[] FullVersionList
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the fullVersion
        /// </summary>
        [JsonProperty("fullVersion", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FullVersion
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the platform
        /// </summary>
        [JsonProperty("platform")]
        public string Platform
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the platformVersion
        /// </summary>
        [JsonProperty("platformVersion")]
        public string PlatformVersion
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the architecture
        /// </summary>
        [JsonProperty("architecture")]
        public string Architecture
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the model
        /// </summary>
        [JsonProperty("model")]
        public string Model
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the mobile
        /// </summary>
        [JsonProperty("mobile")]
        public bool Mobile
        {
            get;
            set;
        }
    }
}