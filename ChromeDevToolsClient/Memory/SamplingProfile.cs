namespace Zu.ChromeDevTools.Memory
{
    using Newtonsoft.Json;

    /// <summary>
    /// Array of heap profile samples.
    /// </summary>
    public sealed class SamplingProfile
    {
        /// <summary>
        /// Gets or sets the samples
        /// </summary>
        [JsonProperty("samples")]
        public SamplingProfileNode[] Samples
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the modules
        /// </summary>
        [JsonProperty("modules")]
        public Module[] Modules
        {
            get;
            set;
        }
    }
}