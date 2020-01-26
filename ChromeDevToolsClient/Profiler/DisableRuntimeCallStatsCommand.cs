namespace Zu.ChromeDevTools.Profiler
{
    using Newtonsoft.Json;

    /// <summary>
    /// Disable run time call stats collection.
    /// </summary>
    public sealed class DisableRuntimeCallStatsCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Profiler.disableRuntimeCallStats";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class DisableRuntimeCallStatsCommandResponse : ICommandResponse<DisableRuntimeCallStatsCommand>
    {
    }
}