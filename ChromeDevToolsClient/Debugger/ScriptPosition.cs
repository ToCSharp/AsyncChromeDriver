namespace Zu.ChromeDevTools.Debugger
{
    using Newtonsoft.Json;

    /// <summary>
    /// Location in the source code.
    /// </summary>
    public sealed class ScriptPosition
    {
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