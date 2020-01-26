namespace Zu.ChromeDevTools.WebAuthn
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class VirtualAuthenticatorOptions
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("protocol")]
        public AuthenticatorProtocol Protocol
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("transport")]
        public AuthenticatorTransport Transport
        {
            get;
            set;
        }
        /// <summary>
        /// Defaults to false.
        ///</summary>
        [JsonProperty("hasResidentKey", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? HasResidentKey
        {
            get;
            set;
        }
        /// <summary>
        /// Defaults to false.
        ///</summary>
        [JsonProperty("hasUserVerification", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? HasUserVerification
        {
            get;
            set;
        }
        /// <summary>
        /// If set to true, tests of user presence will succeed immediately.
        /// Otherwise, they will not be resolved. Defaults to true.
        ///</summary>
        [JsonProperty("automaticPresenceSimulation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? AutomaticPresenceSimulation
        {
            get;
            set;
        }
        /// <summary>
        /// Sets whether User Verification succeeds or fails for an authenticator.
        /// Defaults to false.
        ///</summary>
        [JsonProperty("isUserVerified", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsUserVerified
        {
            get;
            set;
        }
    }
}