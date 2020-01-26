namespace Zu.ChromeDevTools.Profiler
{
    using Newtonsoft.Json;

    /// <summary>
    /// Retrieve run time call stats.
    /// </summary>
    public sealed class GetRuntimeCallStatsCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Profiler.getRuntimeCallStats";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class GetRuntimeCallStatsCommandResponse : ICommandResponse<GetRuntimeCallStatsCommand>
    {
        /// <summary>
        /// Collected counter information.
        ///</summary>
        [JsonProperty("result")]
        public CounterInfo[] Result
        {
            get;
            set;
        }
    }
}