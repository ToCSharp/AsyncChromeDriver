namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ClientSecurityState
    {
        /// <summary>
        /// Gets or sets the initiatorIsSecureContext
        /// </summary>
        [JsonProperty("initiatorIsSecureContext")]
        public bool InitiatorIsSecureContext
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the initiatorIPAddressSpace
        /// </summary>
        [JsonProperty("initiatorIPAddressSpace")]
        public IPAddressSpace InitiatorIPAddressSpace
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the privateNetworkRequestPolicy
        /// </summary>
        [JsonProperty("privateNetworkRequestPolicy")]
        public PrivateNetworkRequestPolicy PrivateNetworkRequestPolicy
        {
            get;
            set;
        }
    }
}