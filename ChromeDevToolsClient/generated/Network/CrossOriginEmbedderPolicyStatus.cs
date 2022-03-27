namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CrossOriginEmbedderPolicyStatus
    {
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [JsonProperty("value")]
        public CrossOriginEmbedderPolicyValue Value
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the reportOnlyValue
        /// </summary>
        [JsonProperty("reportOnlyValue")]
        public CrossOriginEmbedderPolicyValue ReportOnlyValue
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the reportingEndpoint
        /// </summary>
        [JsonProperty("reportingEndpoint", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ReportingEndpoint
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the reportOnlyReportingEndpoint
        /// </summary>
        [JsonProperty("reportOnlyReportingEndpoint", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ReportOnlyReportingEndpoint
        {
            get;
            set;
        }
    }
}