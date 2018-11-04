namespace Zu.ChromeDevTools.Accessibility
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fetches the entire accessibility tree
    /// </summary>
    public sealed class GetFullAXTreeCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Accessibility.getFullAXTree";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class GetFullAXTreeCommandResponse : ICommandResponse<GetFullAXTreeCommand>
    {
        /// <summary>
        /// Gets or sets the nodes
        /// </summary>
        [JsonProperty("nodes")]
        public AXNode[] Nodes
        {
            get;
            set;
        }
    }
}