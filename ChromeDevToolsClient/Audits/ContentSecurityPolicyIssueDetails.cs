namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ContentSecurityPolicyIssueDetails
    {
        /// <summary>
        /// The url not included in allowed sources.
        ///</summary>
        [JsonProperty("blockedURL", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string BlockedURL
        {
            get;
            set;
        }
        /// <summary>
        /// Specific directive that is violated, causing the CSP issue.
        ///</summary>
        [JsonProperty("violatedDirective")]
        public string ViolatedDirective
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the isReportOnly
        /// </summary>
        [JsonProperty("isReportOnly")]
        public bool IsReportOnly
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the contentSecurityPolicyViolationType
        /// </summary>
        [JsonProperty("contentSecurityPolicyViolationType")]
        public ContentSecurityPolicyViolationType ContentSecurityPolicyViolationType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the frameAncestor
        /// </summary>
        [JsonProperty("frameAncestor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AffectedFrame FrameAncestor
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the sourceCodeLocation
        /// </summary>
        [JsonProperty("sourceCodeLocation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SourceCodeLocation SourceCodeLocation
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the violatingNodeId
        /// </summary>
        [JsonProperty("violatingNodeId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? ViolatingNodeId
        {
            get;
            set;
        }
    }
}