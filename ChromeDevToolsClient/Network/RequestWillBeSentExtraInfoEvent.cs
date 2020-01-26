namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fired when additional information about a requestWillBeSent event is available from the
        /// network stack. Not every requestWillBeSent event will have an additional
        /// requestWillBeSentExtraInfo fired for it, and there is no guarantee whether requestWillBeSent
        /// or requestWillBeSentExtraInfo will be fired first for the same request.
    /// </summary>
    public sealed class RequestWillBeSentExtraInfoEvent : IEvent
    {
        /// <summary>
        /// Request identifier. Used to match this information to an existing requestWillBeSent event.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId
        {
            get;
            set;
        }
        /// <summary>
        /// A list of cookies which will not be sent with this request along with corresponding reasons
        /// for blocking.
        /// </summary>
        [JsonProperty("blockedCookies")]
        public BlockedCookieWithReason[] BlockedCookies
        {
            get;
            set;
        }
        /// <summary>
        /// Raw request headers as they will be sent over the wire.
        /// </summary>
        [JsonProperty("headers")]
        public Headers Headers
        {
            get;
            set;
        }
    }
}