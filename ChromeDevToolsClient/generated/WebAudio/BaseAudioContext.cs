namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Protocol object for BaseAudioContext
    /// </summary>
    public sealed class BaseAudioContext
    {
        /// <summary>
        /// Gets or sets the contextId
        /// </summary>
        [JsonProperty("contextId")]
        public string ContextId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the contextType
        /// </summary>
        [JsonProperty("contextType")]
        public ContextType ContextType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the contextState
        /// </summary>
        [JsonProperty("contextState")]
        public ContextState ContextState
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the realtimeData
        /// </summary>
        [JsonProperty("realtimeData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ContextRealtimeData RealtimeData
        {
            get;
            set;
        }
        /// <summary>
        /// Platform-dependent callback buffer size.
        ///</summary>
        [JsonProperty("callbackBufferSize")]
        public double CallbackBufferSize
        {
            get;
            set;
        }
        /// <summary>
        /// Number of output channels supported by audio hardware in use.
        ///</summary>
        [JsonProperty("maxOutputChannelCount")]
        public double MaxOutputChannelCount
        {
            get;
            set;
        }
        /// <summary>
        /// Context sample rate.
        ///</summary>
        [JsonProperty("sampleRate")]
        public double SampleRate
        {
            get;
            set;
        }
    }
}