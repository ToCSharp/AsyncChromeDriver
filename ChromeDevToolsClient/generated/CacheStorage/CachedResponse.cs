namespace Zu.ChromeDevTools.CacheStorage
{
    using Newtonsoft.Json;

    /// <summary>
    /// Cached response
    /// </summary>
    public sealed class CachedResponse
    {
        /// <summary>
        /// Entry content, base64-encoded. (Encoded as a base64 string when passed over JSON)
        ///</summary>
        [JsonProperty("body")]
        public string Body
        {
            get;
            set;
        }
    }
}