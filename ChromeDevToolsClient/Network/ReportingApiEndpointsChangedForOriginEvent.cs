namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ReportingApiEndpointsChangedForOriginEvent : IEvent
    {
        /// <summary>
        /// Origin of the document(s) which configured the endpoints.
        /// </summary>
        [JsonProperty("origin")]
        public string Origin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the endpoints
        /// </summary>
        [JsonProperty("endpoints")]
        public ReportingApiEndpoint[] Endpoints
        {
            get;
            set;
        }
    }
}