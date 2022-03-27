namespace Zu.ChromeDevTools.Media
{
    using Newtonsoft.Json;

    /// <summary>
    /// Corresponds to kMediaError
    /// </summary>
    public sealed class PlayerError
    {
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [JsonProperty("type")]
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// When this switches to using media::Status instead of PipelineStatus
        /// we can remove "errorCode" and replace it with the fields from
        /// a Status instance. This also seems like a duplicate of the error
        /// level enum - there is a todo bug to have that level removed and
        /// use this instead. (crbug.com/1068454)
        ///</summary>
        [JsonProperty("errorCode")]
        public string ErrorCode
        {
            get;
            set;
        }
    }
}