namespace Zu.ChromeDevTools.Overlay
{
    using Newtonsoft.Json;

    /// <summary>
    /// Requests that backend shows hit-test borders on layers
    /// </summary>
    public sealed class SetShowHitTestBordersCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Overlay.setShowHitTestBorders";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// True for showing hit-test borders
        /// </summary>
        [JsonProperty("show")]
        public bool Show
        {
            get;
            set;
        }
    }

    public sealed class SetShowHitTestBordersCommandResponse : ICommandResponse<SetShowHitTestBordersCommand>
    {
    }
}