namespace Zu.ChromeDevTools.Memory
{
    using Newtonsoft.Json;

    /// <summary>
    /// Array of heap profile samples.
    /// </summary>
    public sealed class SamplingProfile
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("samples")]
        public SamplingProfileNode[] Samples
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("modules")]
        public Module[] Modules
        {
            get;
            set;
        }
    }
}