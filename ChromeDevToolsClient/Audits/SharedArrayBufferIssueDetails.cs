namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// Details for a issue arising from an SAB being instantiated in, or
    /// transferred to a context that is not cross-origin isolated.
    /// </summary>
    public sealed class SharedArrayBufferIssueDetails
    {
        /// <summary>
        /// Gets or sets the sourceCodeLocation
        /// </summary>
        [JsonProperty("sourceCodeLocation")]
        public SourceCodeLocation SourceCodeLocation
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the isWarning
        /// </summary>
        [JsonProperty("isWarning")]
        public bool IsWarning
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [JsonProperty("type")]
        public SharedArrayBufferIssueType Type
        {
            get;
            set;
        }
    }
}