namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Protocol object for AudioParam
    /// </summary>
    public sealed class AudioParam
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("paramId")]
        public string ParamId
        {
            get;
            set;
        }
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
        [JsonProperty("paramType")]
        public string ParamType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("rate")]
        public AutomationRate Rate
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("defaultValue")]
        public double DefaultValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("minValue")]
        public double MinValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("maxValue")]
        public double MaxValue
        {
            get;
            set;
        }
    }
}