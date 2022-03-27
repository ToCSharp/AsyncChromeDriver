namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class OriginTrial
    {
        /// <summary>
        /// Gets or sets the trialName
        /// </summary>
        [JsonProperty("trialName")]
        public string TrialName
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [JsonProperty("status")]
        public OriginTrialStatus Status
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the tokensWithStatus
        /// </summary>
        [JsonProperty("tokensWithStatus")]
        public OriginTrialTokenWithStatus[] TokensWithStatus
        {
            get;
            set;
        }
    }
}