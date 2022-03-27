namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class OriginTrialTokenWithStatus
    {
        /// <summary>
        /// Gets or sets the rawTokenText
        /// </summary>
        [JsonProperty("rawTokenText")]
        public string RawTokenText
        {
            get;
            set;
        }
        /// <summary>
        /// `parsedToken` is present only when the token is extractable and
        /// parsable.
        ///</summary>
        [JsonProperty("parsedToken", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public OriginTrialToken ParsedToken
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the status
        /// </summary>
        [JsonProperty("status")]
        public OriginTrialTokenStatus Status
        {
            get;
            set;
        }
    }
}