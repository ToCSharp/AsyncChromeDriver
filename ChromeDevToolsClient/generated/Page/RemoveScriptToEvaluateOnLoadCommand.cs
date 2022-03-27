namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Deprecated, please use removeScriptToEvaluateOnNewDocument instead.
    /// </summary>
    public sealed class RemoveScriptToEvaluateOnLoadCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Page.removeScriptToEvaluateOnLoad";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier
        {
            get;
            set;
        }
    }

    public sealed class RemoveScriptToEvaluateOnLoadCommandResponse : ICommandResponse<RemoveScriptToEvaluateOnLoadCommand>
    {
    }
}