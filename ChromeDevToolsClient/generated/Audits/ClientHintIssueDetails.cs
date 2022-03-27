namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// This issue tracks client hints related issues. It's used to deprecate old
    /// features, encourage the use of new ones, and provide general guidance.
    /// </summary>
    public sealed class ClientHintIssueDetails
    {
        /// <summary>
        /// Gets or sets the sourceCodeLocation
        /// </summary>
        [JsonProperty("sourceCodeLocation")]
        public SourceCodeLocation SourceCodeLocation
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the clientHintIssueReason
        /// </summary>
        [JsonProperty("clientHintIssueReason")]
        public ClientHintIssueReason ClientHintIssueReason
        {
            get;
            set;
        }
    }
}