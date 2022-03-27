namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// This struct holds a list of optional fields with additional information
    /// specific to the kind of issue. When adding a new issue code, please also
    /// add a new optional field to this type.
    /// </summary>
    public sealed class InspectorIssueDetails
    {
        /// <summary>
        /// Gets or sets the cookieIssueDetails
        /// </summary>
        [JsonProperty("cookieIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CookieIssueDetails CookieIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the mixedContentIssueDetails
        /// </summary>
        [JsonProperty("mixedContentIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MixedContentIssueDetails MixedContentIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the blockedByResponseIssueDetails
        /// </summary>
        [JsonProperty("blockedByResponseIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BlockedByResponseIssueDetails BlockedByResponseIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the heavyAdIssueDetails
        /// </summary>
        [JsonProperty("heavyAdIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public HeavyAdIssueDetails HeavyAdIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the contentSecurityPolicyIssueDetails
        /// </summary>
        [JsonProperty("contentSecurityPolicyIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ContentSecurityPolicyIssueDetails ContentSecurityPolicyIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the sharedArrayBufferIssueDetails
        /// </summary>
        [JsonProperty("sharedArrayBufferIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SharedArrayBufferIssueDetails SharedArrayBufferIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the twaQualityEnforcementDetails
        /// </summary>
        [JsonProperty("twaQualityEnforcementDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TrustedWebActivityIssueDetails TwaQualityEnforcementDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the lowTextContrastIssueDetails
        /// </summary>
        [JsonProperty("lowTextContrastIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LowTextContrastIssueDetails LowTextContrastIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the corsIssueDetails
        /// </summary>
        [JsonProperty("corsIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CorsIssueDetails CorsIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the attributionReportingIssueDetails
        /// </summary>
        [JsonProperty("attributionReportingIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AttributionReportingIssueDetails AttributionReportingIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the quirksModeIssueDetails
        /// </summary>
        [JsonProperty("quirksModeIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public QuirksModeIssueDetails QuirksModeIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the navigatorUserAgentIssueDetails
        /// </summary>
        [JsonProperty("navigatorUserAgentIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NavigatorUserAgentIssueDetails NavigatorUserAgentIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the genericIssueDetails
        /// </summary>
        [JsonProperty("genericIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public GenericIssueDetails GenericIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the deprecationIssueDetails
        /// </summary>
        [JsonProperty("deprecationIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DeprecationIssueDetails DeprecationIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the clientHintIssueDetails
        /// </summary>
        [JsonProperty("clientHintIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ClientHintIssueDetails ClientHintIssueDetails
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the federatedAuthRequestIssueDetails
        /// </summary>
        [JsonProperty("federatedAuthRequestIssueDetails", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public FederatedAuthRequestIssueDetails FederatedAuthRequestIssueDetails
        {
            get;
            set;
        }
    }
}