namespace Zu.ChromeDevTools.Storage
{
    using Newtonsoft.Json;

    /// <summary>
    /// The full details of an interest group.
    /// </summary>
    public sealed class InterestGroupDetails
    {
        /// <summary>
        /// Gets or sets the ownerOrigin
        /// </summary>
        [JsonProperty("ownerOrigin")]
        public string OwnerOrigin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the expirationTime
        /// </summary>
        [JsonProperty("expirationTime")]
        public double ExpirationTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the joiningOrigin
        /// </summary>
        [JsonProperty("joiningOrigin")]
        public string JoiningOrigin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the biddingUrl
        /// </summary>
        [JsonProperty("biddingUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string BiddingUrl
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the biddingWasmHelperUrl
        /// </summary>
        [JsonProperty("biddingWasmHelperUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string BiddingWasmHelperUrl
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the updateUrl
        /// </summary>
        [JsonProperty("updateUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UpdateUrl
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the trustedBiddingSignalsUrl
        /// </summary>
        [JsonProperty("trustedBiddingSignalsUrl", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TrustedBiddingSignalsUrl
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the trustedBiddingSignalsKeys
        /// </summary>
        [JsonProperty("trustedBiddingSignalsKeys")]
        public string[] TrustedBiddingSignalsKeys
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the userBiddingSignals
        /// </summary>
        [JsonProperty("userBiddingSignals", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserBiddingSignals
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the ads
        /// </summary>
        [JsonProperty("ads")]
        public InterestGroupAd[] Ads
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the adComponents
        /// </summary>
        [JsonProperty("adComponents")]
        public InterestGroupAd[] AdComponents
        {
            get;
            set;
        }
    }
}