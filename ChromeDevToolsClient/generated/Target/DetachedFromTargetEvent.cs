namespace Zu.ChromeDevTools.Target
{
    using Newtonsoft.Json;

    /// <summary>
    /// Issued when detached from target for any reason (including `detachFromTarget` command). Can be
    /// issued multiple times per target if multiple sessions have been attached to it.
    /// </summary>
    public sealed class DetachedFromTargetEvent : IEvent
    {
        /// <summary>
        /// Detached session identifier.
        /// </summary>
        [JsonProperty("sessionId")]
        public string SessionId
        {
            get;
            set;
        }
        /// <summary>
        /// Deprecated.
        /// </summary>
        [JsonProperty("targetId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string TargetId
        {
            get;
            set;
        }
    }
}