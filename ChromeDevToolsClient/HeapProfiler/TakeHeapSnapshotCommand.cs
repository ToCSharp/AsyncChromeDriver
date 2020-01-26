namespace Zu.ChromeDevTools.HeapProfiler
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class TakeHeapSnapshotCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "HeapProfiler.takeHeapSnapshot";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// If true 'reportHeapSnapshotProgress' events will be generated while snapshot is being taken.
        /// </summary>
        [JsonProperty("reportProgress", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ReportProgress
        {
            get;
            set;
        }
        /// <summary>
        /// If true, a raw snapshot without artifical roots will be generated
        /// </summary>
        [JsonProperty("treatGlobalObjectsAsRoots", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? TreatGlobalObjectsAsRoots
        {
            get;
            set;
        }
    }

    public sealed class TakeHeapSnapshotCommandResponse : ICommandResponse<TakeHeapSnapshotCommand>
    {
    }
}