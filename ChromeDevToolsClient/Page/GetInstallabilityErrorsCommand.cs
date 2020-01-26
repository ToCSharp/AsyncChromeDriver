namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetInstallabilityErrorsCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Page.getInstallabilityErrors";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class GetInstallabilityErrorsCommandResponse : ICommandResponse<GetInstallabilityErrorsCommand>
    {
        /// <summary>
        /// Gets or sets the errors
        /// </summary>
        [JsonProperty("errors")]
        public string[] Errors
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the installabilityErrors
        /// </summary>
        [JsonProperty("installabilityErrors")]
        public InstallabilityError[] InstallabilityErrors
        {
            get;
            set;
        }
    }
}