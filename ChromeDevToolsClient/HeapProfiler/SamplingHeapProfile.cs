namespace Zu.ChromeDevTools.HeapProfiler
{
    using Newtonsoft.Json;

    /// <summary>
    /// Sampling profile.
    /// </summary>
    public sealed class SamplingHeapProfile
    {
        /// <summary>
        /// Gets or sets the head
        /// </summary>
        [JsonProperty("head")]
        public SamplingHeapProfileNode Head
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the samples
        /// </summary>
        [JsonProperty("samples")]
        public SamplingHeapProfileSample[] Samples
        {
            get;
            set;
        }
    }
}