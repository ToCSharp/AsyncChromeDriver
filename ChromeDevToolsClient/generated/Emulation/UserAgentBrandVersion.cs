namespace Zu.ChromeDevTools.Emulation
{
    using Newtonsoft.Json;

    /// <summary>
    /// Used to specify User Agent Cient Hints to emulate. See https://wicg.github.io/ua-client-hints
    /// </summary>
    public sealed class UserAgentBrandVersion
    {
        /// <summary>
        /// Gets or sets the brand
        /// </summary>
        [JsonProperty("brand")]
        public string Brand
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the version
        /// </summary>
        [JsonProperty("version")]
        public string Version
        {
            get;
            set;
        }
    }
}