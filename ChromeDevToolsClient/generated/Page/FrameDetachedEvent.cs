namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fired when frame has been detached from its parent.
    /// </summary>
    public sealed class FrameDetachedEvent : IEvent
    {
        /// <summary>
        /// Id of the frame that has been detached.
        /// </summary>
        [JsonProperty("frameId")]
        public string FrameId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the reason
        /// </summary>
        [JsonProperty("reason")]
        public string Reason
        {
            get;
            set;
        }
    }
}