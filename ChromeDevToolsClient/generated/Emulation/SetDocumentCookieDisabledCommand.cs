namespace Zu.ChromeDevTools.Emulation
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class SetDocumentCookieDisabledCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Emulation.setDocumentCookieDisabled";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Whether document.coookie API should be disabled.
        /// </summary>
        [JsonProperty("disabled")]
        public bool Disabled
        {
            get;
            set;
        }
    }

    public sealed class SetDocumentCookieDisabledCommandResponse : ICommandResponse<SetDocumentCookieDisabledCommand>
    {
    }
}