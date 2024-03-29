namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fired when page is about to send HTTP request.
    /// </summary>
    public sealed class RequestWillBeSentEvent : IEvent
    {
        /// <summary>
        /// Request identifier.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId
        {
            get;
            set;
        }
        /// <summary>
        /// Loader identifier. Empty string if the request is fetched from worker.
        /// </summary>
        [JsonProperty("loaderId")]
        public string LoaderId
        {
            get;
            set;
        }
        /// <summary>
        /// URL of the document this request is loaded for.
        /// </summary>
        [JsonProperty("documentURL")]
        public string DocumentURL
        {
            get;
            set;
        }
        /// <summary>
        /// Request data.
        /// </summary>
        [JsonProperty("request")]
        public Request Request
        {
            get;
            set;
        }
        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonProperty("timestamp")]
        public double Timestamp
        {
            get;
            set;
        }
        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonProperty("wallTime")]
        public double WallTime
        {
            get;
            set;
        }
        /// <summary>
        /// Request initiator.
        /// </summary>
        [JsonProperty("initiator")]
        public Initiator Initiator
        {
            get;
            set;
        }
        /// <summary>
        /// In the case that redirectResponse is populated, this flag indicates whether
        /// requestWillBeSentExtraInfo and responseReceivedExtraInfo events will be or were emitted
        /// for the request which was just redirected.
        /// </summary>
        [JsonProperty("redirectHasExtraInfo")]
        public bool RedirectHasExtraInfo
        {
            get;
            set;
        }
        /// <summary>
        /// Redirect response data.
        /// </summary>
        [JsonProperty("redirectResponse", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Response RedirectResponse
        {
            get;
            set;
        }
        /// <summary>
        /// Type of this resource.
        /// </summary>
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResourceType? Type
        {
            get;
            set;
        }
        /// <summary>
        /// Frame identifier.
        /// </summary>
        [JsonProperty("frameId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FrameId
        {
            get;
            set;
        }
        /// <summary>
        /// Whether the request is initiated by a user gesture. Defaults to false.
        /// </summary>
        [JsonProperty("hasUserGesture", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? HasUserGesture
        {
            get;
            set;
        }
    }
}