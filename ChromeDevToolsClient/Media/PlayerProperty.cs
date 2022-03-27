namespace Zu.ChromeDevTools.Media
{
    using Newtonsoft.Json;

    /// <summary>
    /// Corresponds to kMediaPropertyChange
    /// </summary>
    public sealed class PlayerProperty
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }
    }
}