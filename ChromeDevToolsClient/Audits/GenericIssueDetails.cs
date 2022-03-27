namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// Depending on the concrete errorType, different properties are set.
    /// </summary>
    public sealed class GenericIssueDetails
    {
        /// <summary>
        /// Issues with the same errorType are aggregated in the frontend.
        ///</summary>
        [JsonProperty("errorType")]
        public GenericIssueErrorType ErrorType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the frameId
        /// </summary>
        [JsonProperty("frameId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FrameId
        {
            get;
            set;
        }
    }
}