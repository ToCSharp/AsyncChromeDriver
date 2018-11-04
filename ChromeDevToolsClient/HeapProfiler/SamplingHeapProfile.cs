namespace Zu.ChromeDevTools.HeapProfiler
{
    using Newtonsoft.Json;

    /// <summary>
    /// Sampling profile.
    /// </summary>
    public sealed class SamplingHeapProfile
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("head")]
        public SamplingHeapProfileNode Head
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("samples")]
        public SamplingHeapProfileSample[] Samples
        {
            get;
            set;
        }
    }
}