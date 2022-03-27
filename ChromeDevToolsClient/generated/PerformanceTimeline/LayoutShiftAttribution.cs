namespace Zu.ChromeDevTools.PerformanceTimeline
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class LayoutShiftAttribution
    {
        /// <summary>
        /// Gets or sets the previousRect
        /// </summary>
        [JsonProperty("previousRect")]
        public DOM.Rect PreviousRect
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the currentRect
        /// </summary>
        [JsonProperty("currentRect")]
        public DOM.Rect CurrentRect
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the nodeId
        /// </summary>
        [JsonProperty("nodeId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? NodeId
        {
            get;
            set;
        }
    }
}