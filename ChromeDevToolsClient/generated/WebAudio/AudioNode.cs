namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Protocol object for AudioNode
    /// </summary>
    public sealed class AudioNode
    {
        /// <summary>
        /// Gets or sets the nodeId
        /// </summary>
        [JsonProperty("nodeId")]
        public string NodeId
        {
            get;
            set;
        }
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
        /// Gets or sets the nodeType
        /// </summary>
        [JsonProperty("nodeType")]
        public string NodeType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the numberOfInputs
        /// </summary>
        [JsonProperty("numberOfInputs")]
        public double NumberOfInputs
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the numberOfOutputs
        /// </summary>
        [JsonProperty("numberOfOutputs")]
        public double NumberOfOutputs
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the channelCount
        /// </summary>
        [JsonProperty("channelCount")]
        public double ChannelCount
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the channelCountMode
        /// </summary>
        [JsonProperty("channelCountMode")]
        public ChannelCountMode ChannelCountMode
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the channelInterpretation
        /// </summary>
        [JsonProperty("channelInterpretation")]
        public ChannelInterpretation ChannelInterpretation
        {
            get;
            set;
        }
    }
}