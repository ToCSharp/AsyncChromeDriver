namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ReportingApiReportUpdatedEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the report
        /// </summary>
        [JsonProperty("report")]
        public ReportingApiReport Report
        {
            get;
            set;
        }
    }
}