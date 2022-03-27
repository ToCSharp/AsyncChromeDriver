namespace Zu.ChromeDevTools.Storage
{
    using Newtonsoft.Json;

    /// <summary>
    /// Ad advertising element inside an interest group.
    /// </summary>
    public sealed class InterestGroupAd
    {
        /// <summary>
        /// Gets or sets the renderUrl
        /// </summary>
        [JsonProperty("renderUrl")]
        public string RenderUrl
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the metadata
        /// </summary>
        [JsonProperty("metadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Metadata
        {
            get;
            set;
        }
    }
}