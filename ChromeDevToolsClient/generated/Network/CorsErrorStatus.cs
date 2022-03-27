namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CorsErrorStatus
    {
        /// <summary>
        /// Gets or sets the corsError
        /// </summary>
        [JsonProperty("corsError")]
        public CorsError CorsError
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the failedParameter
        /// </summary>
        [JsonProperty("failedParameter")]
        public string FailedParameter
        {
            get;
            set;
        }
    }
}