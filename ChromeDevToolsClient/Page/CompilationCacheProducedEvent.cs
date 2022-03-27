namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Issued for every compilation cache generated. Is only available
    /// if Page.setGenerateCompilationCache is enabled.
    /// </summary>
    public sealed class CompilationCacheProducedEvent : IEvent
    {
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
        /// Base64-encoded data (Encoded as a base64 string when passed over JSON)
        /// </summary>
        [JsonProperty("data")]
        public string Data
        {
            get;
            set;
        }
    }
}