namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class OriginTrialToken
    {
        /// <summary>
        /// Gets or sets the origin
        /// </summary>
        [JsonProperty("origin")]
        public string Origin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the matchSubDomains
        /// </summary>
        [JsonProperty("matchSubDomains")]
        public bool MatchSubDomains
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the trialName
        /// </summary>
        [JsonProperty("trialName")]
        public string TrialName
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the expiryTime
        /// </summary>
        [JsonProperty("expiryTime")]
        public double ExpiryTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the isThirdParty
        /// </summary>
        [JsonProperty("isThirdParty")]
        public bool IsThirdParty
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the usageRestriction
        /// </summary>
        [JsonProperty("usageRestriction")]
        public OriginTrialUsageRestriction UsageRestriction
        {
            get;
            set;
        }
    }
}