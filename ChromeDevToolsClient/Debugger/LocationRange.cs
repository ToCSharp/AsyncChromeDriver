namespace Zu.ChromeDevTools.Debugger
{
    using Newtonsoft.Json;

    /// <summary>
    /// Location range within one script.
    /// </summary>
    public sealed class LocationRange
    {
        /// <summary>
        /// Gets or sets the scriptId
        /// </summary>
        [JsonProperty("scriptId")]
        public string ScriptId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the start
        /// </summary>
        [JsonProperty("start")]
        public ScriptPosition Start
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the end
        /// </summary>
        [JsonProperty("end")]
        public ScriptPosition End
        {
            get;
            set;
        }
    }
}