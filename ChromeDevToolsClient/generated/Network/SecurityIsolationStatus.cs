namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class SecurityIsolationStatus
    {
        /// <summary>
        /// Gets or sets the coop
        /// </summary>
        [JsonProperty("coop", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CrossOriginOpenerPolicyStatus Coop
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the coep
        /// </summary>
        [JsonProperty("coep", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CrossOriginEmbedderPolicyStatus Coep
        {
            get;
            set;
        }
    }
}