namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// Information about a request that is affected by an inspector issue.
    /// </summary>
    public sealed class AffectedRequest
    {
        /// <summary>
        /// The unique request id.
        ///</summary>
        [JsonProperty("requestId")]
        public string RequestId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the url
        /// </summary>
        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url
        {
            get;
            set;
        }
    }
}