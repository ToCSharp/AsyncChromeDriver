namespace Zu.ChromeDevTools.Profiler
{
    using Newtonsoft.Json;

    /// <summary>
    /// Enable run time call stats collection.
    /// </summary>
    public sealed class EnableRuntimeCallStatsCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Profiler.enableRuntimeCallStats";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class EnableRuntimeCallStatsCommandResponse : ICommandResponse<EnableRuntimeCallStatsCommand>
    {
    }
}