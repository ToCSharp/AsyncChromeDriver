namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fired when page is about to start a download.
    /// </summary>
    public sealed class DownloadWillBeginEvent : IEvent
    {
        /// <summary>
        /// Id of the frame that caused download to begin.
        /// </summary>
        [JsonProperty("frameId")]
        public string FrameId
        {
            get;
            set;
        }
        /// <summary>
        /// URL of the resource being downloaded.
        /// </summary>
        [JsonProperty("url")]
        public string Url
        {
            get;
            set;
        }
    }
}