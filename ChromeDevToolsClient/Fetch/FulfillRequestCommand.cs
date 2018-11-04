namespace Zu.ChromeDevTools.Fetch
{
    using Newtonsoft.Json;

    /// <summary>
    /// Provides response to the request.
    /// </summary>
    public sealed class FulfillRequestCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Fetch.fulfillRequest";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// An id the client received in requestPaused event.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId
        {
            get;
            set;
        }
        /// <summary>
        /// An HTTP response code.
        /// </summary>
        [JsonProperty("responseCode")]
        public long ResponseCode
        {
            get;
            set;
        }
        /// <summary>
        /// Response headers.
        /// </summary>
        [JsonProperty("responseHeaders")]
        public HeaderEntry[] ResponseHeaders
        {
            get;
            set;
        }
        /// <summary>
        /// A response body.
        /// </summary>
        [JsonProperty("body", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Body
        {
            get;
            set;
        }
        /// <summary>
        /// A textual representation of responseCode.
        /// If absent, a standard phrase mathcing responseCode is used.
        /// </summary>
        [JsonProperty("responsePhrase", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ResponsePhrase
        {
            get;
            set;
        }
    }

    public sealed class FulfillRequestCommandResponse : ICommandResponse<FulfillRequestCommand>
    {
    }
}