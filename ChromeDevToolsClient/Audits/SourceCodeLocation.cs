namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class SourceCodeLocation
    {
        /// <summary>
        /// Gets or sets the scriptId
        /// </summary>
        [JsonProperty("scriptId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ScriptId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the url
        /// </summary>
        [JsonProperty("url")]
        public string Url
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the lineNumber
        /// </summary>
        [JsonProperty("lineNumber")]
        public long LineNumber
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the columnNumber
        /// </summary>
        [JsonProperty("columnNumber")]
        public long ColumnNumber
        {
            get;
            set;
        }
    }
}