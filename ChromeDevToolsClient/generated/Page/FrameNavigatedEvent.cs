namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fired once navigation of the frame has completed. Frame is now associated with the new loader.
    /// </summary>
    public sealed class FrameNavigatedEvent : IEvent
    {
        /// <summary>
        /// Frame object.
        /// </summary>
        [JsonProperty("frame")]
        public Frame Frame
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [JsonProperty("type")]
        public NavigationType Type
        {
            get;
            set;
        }
    }
}