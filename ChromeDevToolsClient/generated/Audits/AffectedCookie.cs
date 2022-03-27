namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// Information about a cookie that is affected by an inspector issue.
    /// </summary>
    public sealed class AffectedCookie
    {
        /// <summary>
        /// The following three properties uniquely identify a cookie
        ///</summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the path
        /// </summary>
        [JsonProperty("path")]
        public string Path
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the domain
        /// </summary>
        [JsonProperty("domain")]
        public string Domain
        {
            get;
            set;
        }
    }
}