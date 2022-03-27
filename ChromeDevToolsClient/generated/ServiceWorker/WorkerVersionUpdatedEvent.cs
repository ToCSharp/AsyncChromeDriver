namespace Zu.ChromeDevTools.ServiceWorker
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class WorkerVersionUpdatedEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the versions
        /// </summary>
        [JsonProperty("versions")]
        public ServiceWorkerVersion[] Versions
        {
            get;
            set;
        }
    }
}