namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// Details for issues about documents in Quirks Mode
    /// or Limited Quirks Mode that affects page layouting.
    /// </summary>
    public sealed class QuirksModeIssueDetails
    {
        /// <summary>
        /// If false, it means the document's mode is "quirks"
        /// instead of "limited-quirks".
        ///</summary>
        [JsonProperty("isLimitedQuirksMode")]
        public bool IsLimitedQuirksMode
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the documentNodeId
        /// </summary>
        [JsonProperty("documentNodeId")]
        public long DocumentNodeId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the url
        /// </summary>
        [JsonProperty("url")]
        public string Url
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the frameId
        /// </summary>
        [JsonProperty("frameId")]
        public string FrameId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the loaderId
        /// </summary>
        [JsonProperty("loaderId")]
        public string LoaderId
        {
            get;
            set;
        }
    }
}