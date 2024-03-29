namespace Zu.ChromeDevTools.Browser
{
    using Newtonsoft.Json;

    /// <summary>
    /// Grant specific permissions to the given origin and reject all others.
    /// </summary>
    public sealed class GrantPermissionsCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Browser.grantPermissions";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Gets or sets the permissions
        /// </summary>
        [JsonProperty("permissions")]
        public PermissionType[] Permissions
        {
            get;
            set;
        }
        /// <summary>
        /// Origin the permission applies to, all origins if not specified.
        /// </summary>
        [JsonProperty("origin", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Origin
        {
            get;
            set;
        }
        /// <summary>
        /// BrowserContext to override permissions. When omitted, default browser context is used.
        /// </summary>
        [JsonProperty("browserContextId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string BrowserContextId
        {
            get;
            set;
        }
    }

    public sealed class GrantPermissionsCommandResponse : ICommandResponse<GrantPermissionsCommand>
    {
    }
}