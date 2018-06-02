namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// Allows overriding user agent with the given string.
    /// </summary>
    public sealed class SetUserAgentOverrideCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Network.setUserAgentOverride";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// User agent to use.
        /// </summary>
        [JsonProperty("userAgent")]
        public string UserAgent
        {
            get;
            set;
        }
        /// <summary>
        /// Browser langugage to emulate.
        /// </summary>
        [JsonProperty("acceptLanguage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AcceptLanguage
        {
            get;
            set;
        }
        /// <summary>
        /// The platform navigator.platform should return.
        /// </summary>
        [JsonProperty("platform", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Platform
        {
            get;
            set;
        }
    }

    public sealed class SetUserAgentOverrideCommandResponse : ICommandResponse<SetUserAgentOverrideCommand>
    {
    }
}