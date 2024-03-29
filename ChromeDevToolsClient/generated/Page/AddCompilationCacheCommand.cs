namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Seeds compilation cache for given url. Compilation cache does not survive
    /// cross-process navigation.
    /// </summary>
    public sealed class AddCompilationCacheCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Page.addCompilationCache";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Gets or sets the url
        /// </summary>
        [JsonProperty("url")]
        public string Url
        {
            get;
            set;
        }
        /// <summary>
        /// Base64-encoded data (Encoded as a base64 string when passed over JSON)
        /// </summary>
        [JsonProperty("data")]
        public string Data
        {
            get;
            set;
        }
    }

    public sealed class AddCompilationCacheCommandResponse : ICommandResponse<AddCompilationCacheCommand>
    {
    }
}