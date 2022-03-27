namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class LowTextContrastIssueDetails
    {
        /// <summary>
        /// Gets or sets the violatingNodeId
        /// </summary>
        [JsonProperty("violatingNodeId")]
        public long ViolatingNodeId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the violatingNodeSelector
        /// </summary>
        [JsonProperty("violatingNodeSelector")]
        public string ViolatingNodeSelector
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the contrastRatio
        /// </summary>
        [JsonProperty("contrastRatio")]
        public double ContrastRatio
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the thresholdAA
        /// </summary>
        [JsonProperty("thresholdAA")]
        public double ThresholdAA
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the thresholdAAA
        /// </summary>
        [JsonProperty("thresholdAAA")]
        public double ThresholdAAA
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the fontSize
        /// </summary>
        [JsonProperty("fontSize")]
        public string FontSize
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the fontWeight
        /// </summary>
        [JsonProperty("fontWeight")]
        public string FontWeight
        {
            get;
            set;
        }
    }
}