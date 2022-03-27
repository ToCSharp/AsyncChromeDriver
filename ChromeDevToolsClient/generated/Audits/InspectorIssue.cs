namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// An inspector issue reported from the back-end.
    /// </summary>
    public sealed class InspectorIssue
    {
        /// <summary>
        /// Gets or sets the code
        /// </summary>
        [JsonProperty("code")]
        public InspectorIssueCode Code
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the details
        /// </summary>
        [JsonProperty("details")]
        public InspectorIssueDetails Details
        {
            get;
            set;
        }
        /// <summary>
        /// A unique id for this issue. May be omitted if no other entity (e.g.
        /// exception, CDP message, etc.) is referencing this issue.
        ///</summary>
        [JsonProperty("issueId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string IssueId
        {
            get;
            set;
        }
    }
}