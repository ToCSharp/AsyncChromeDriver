namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// Details for a CORS related issue, e.g. a warning or error related to
    /// CORS RFC1918 enforcement.
    /// </summary>
    public sealed class CorsIssueDetails
    {
        /// <summary>
        /// Gets or sets the corsErrorStatus
        /// </summary>
        [JsonProperty("corsErrorStatus")]
        public Network.CorsErrorStatus CorsErrorStatus
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the isWarning
        /// </summary>
        [JsonProperty("isWarning")]
        public bool IsWarning
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the request
        /// </summary>
        [JsonProperty("request")]
        public AffectedRequest Request
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the location
        /// </summary>
        [JsonProperty("location", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SourceCodeLocation Location
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the initiatorOrigin
        /// </summary>
        [JsonProperty("initiatorOrigin", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string InitiatorOrigin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the resourceIPAddressSpace
        /// </summary>
        [JsonProperty("resourceIPAddressSpace", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Network.IPAddressSpace? ResourceIPAddressSpace
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the clientSecurityState
        /// </summary>
        [JsonProperty("clientSecurityState", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Network.ClientSecurityState ClientSecurityState
        {
            get;
            set;
        }
    }
}