namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// Clears the overridden Geolocation Position and Error.
    /// </summary>
    public sealed class ClearGeolocationOverrideCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Page.clearGeolocationOverride";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class ClearGeolocationOverrideCommandResponse : ICommandResponse<ClearGeolocationOverrideCommand>
    {
    }
}