namespace Zu.ChromeDevTools.PerformanceTimeline
{
    using Newtonsoft.Json;

    /// <summary>
    /// See https://wicg.github.io/layout-instability/#sec-layout-shift and layout_shift.idl
    /// </summary>
    public sealed class LayoutShift
    {
        /// <summary>
        /// Score increment produced by this event.
        ///</summary>
        [JsonProperty("value")]
        public double Value
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the hadRecentInput
        /// </summary>
        [JsonProperty("hadRecentInput")]
        public bool HadRecentInput
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the lastInputTime
        /// </summary>
        [JsonProperty("lastInputTime")]
        public double LastInputTime
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the sources
        /// </summary>
        [JsonProperty("sources")]
        public LayoutShiftAttribution[] Sources
        {
            get;
            set;
        }
    }
}