namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Protocol object for AudioNode
    /// </summary>
    public sealed class AudioNode
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("nodeId")]
        public string NodeId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("contextId")]
        public string ContextId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("nodeType")]
        public string NodeType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("numberOfInputs")]
        public double NumberOfInputs
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("numberOfOutputs")]
        public double NumberOfOutputs
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("channelCount")]
        public double ChannelCount
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("channelCountMode")]
        public ChannelCountMode ChannelCountMode
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("channelInterpretation")]
        public ChannelInterpretation ChannelInterpretation
        {
            get;
            set;
        }
    }
}