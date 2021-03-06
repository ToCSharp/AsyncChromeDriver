namespace Zu.ChromeDevTools.ServiceWorker
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class StopAllWorkersCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "ServiceWorker.stopAllWorkers";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class StopAllWorkersCommandResponse : ICommandResponse<StopAllWorkersCommand>
    {
    }
}