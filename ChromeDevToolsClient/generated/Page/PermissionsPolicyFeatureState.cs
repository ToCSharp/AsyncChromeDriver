namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class PermissionsPolicyFeatureState
    {
        /// <summary>
        /// Gets or sets the feature
        /// </summary>
        [JsonProperty("feature")]
        public PermissionsPolicyFeature Feature
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the allowed
        /// </summary>
        [JsonProperty("allowed")]
        public bool Allowed
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the locator
        /// </summary>
        [JsonProperty("locator", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PermissionsPolicyBlockLocator Locator
        {
            get;
            set;
        }
    }
}