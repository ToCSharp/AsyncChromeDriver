namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class IssueAddedEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the issue
        /// </summary>
        [JsonProperty("issue")]
        public InspectorIssue Issue
        {
            get;
            set;
        }
    }
}