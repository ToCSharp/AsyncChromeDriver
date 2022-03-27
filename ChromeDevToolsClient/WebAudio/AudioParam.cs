namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Protocol object for AudioParam
    /// </summary>
    public sealed class AudioParam
    {
        /// <summary>
        /// Gets or sets the paramId
        /// </summary>
        [JsonProperty("paramId")]
        public string ParamId
        {
            get;
            set;
        }
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
        /// Gets or sets the paramType
        /// </summary>
        [JsonProperty("paramType")]
        public string ParamType
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the rate
        /// </summary>
        [JsonProperty("rate")]
        public AutomationRate Rate
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the defaultValue
        /// </summary>
        [JsonProperty("defaultValue")]
        public double DefaultValue
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the minValue
        /// </summary>
        [JsonProperty("minValue")]
        public double MinValue
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the maxValue
        /// </summary>
        [JsonProperty("maxValue")]
        public double MaxValue
        {
            get;
            set;
        }
    }
}