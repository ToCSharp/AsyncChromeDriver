namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetManifestIconsCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Page.getManifestIcons";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class GetManifestIconsCommandResponse : ICommandResponse<GetManifestIconsCommand>
    {
        /// <summary>
        /// Gets or sets the primaryIcon
        /// </summary>
        [JsonProperty("primaryIcon", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PrimaryIcon
        {
            get;
            set;
        }
    }
}