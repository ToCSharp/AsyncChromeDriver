namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class FederatedAuthRequestIssueDetails
    {
        /// <summary>
        /// Gets or sets the federatedAuthRequestIssueReason
        /// </summary>
        [JsonProperty("federatedAuthRequestIssueReason")]
        public FederatedAuthRequestIssueReason FederatedAuthRequestIssueReason
        {
            get;
            set;
        }
    }
}