namespace Zu.ChromeDevTools.Target
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class TargetInfo
    {
        /// <summary>
        /// Gets or sets the targetId
        /// </summary>
        [JsonProperty("targetId")]
        public string TargetId
        {
            get;
            set;
        }
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
        /// Gets or sets the title
        /// </summary>
        [JsonProperty("title")]
        public string Title
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
        /// Whether the target has an attached client.
        ///</summary>
        [JsonProperty("attached")]
        public bool Attached
        {
            get;
            set;
        }
        /// <summary>
        /// Opener target Id
        ///</summary>
        [JsonProperty("openerId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OpenerId
        {
            get;
            set;
        }
        /// <summary>
        /// Whether the target has access to the originating window.
        ///</summary>
        [JsonProperty("canAccessOpener")]
        public bool CanAccessOpener
        {
            get;
            set;
        }
        /// <summary>
        /// Frame id of originating window (is only set if target has an opener).
        ///</summary>
        [JsonProperty("openerFrameId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OpenerFrameId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the browserContextId
        /// </summary>
        [JsonProperty("browserContextId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string BrowserContextId
        {
            get;
            set;
        }
    }
}