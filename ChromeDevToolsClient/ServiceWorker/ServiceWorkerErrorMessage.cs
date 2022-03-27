namespace Zu.ChromeDevTools.ServiceWorker
{
    using Newtonsoft.Json;

    /// <summary>
    /// ServiceWorker error message.
    /// </summary>
    public sealed class ServiceWorkerErrorMessage
    {
        /// <summary>
        /// Gets or sets the errorMessage
        /// </summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the registrationId
        /// </summary>
        [JsonProperty("registrationId")]
        public string RegistrationId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the versionId
        /// </summary>
        [JsonProperty("versionId")]
        public string VersionId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the sourceURL
        /// </summary>
        [JsonProperty("sourceURL")]
        public string SourceURL
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