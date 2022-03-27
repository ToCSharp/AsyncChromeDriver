namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Indicates whether a frame has been identified as an ad and why.
    /// </summary>
    public sealed class AdFrameStatus
    {
        /// <summary>
        /// Gets or sets the adFrameType
        /// </summary>
        [JsonProperty("adFrameType")]
        public AdFrameType AdFrameType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the explanations
        /// </summary>
        [JsonProperty("explanations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AdFrameExplanation[] Explanations
        {
            get;
            set;
        }
    }
}