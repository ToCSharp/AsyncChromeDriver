namespace Zu.ChromeDevTools.ServiceWorker
{
    using Newtonsoft.Json;

    /// <summary>
    /// ServiceWorker registration.
    /// </summary>
    public sealed class ServiceWorkerRegistration
    {
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
        /// Gets or sets the scopeURL
        /// </summary>
        [JsonProperty("scopeURL")]
        public string ScopeURL
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the isDeleted
        /// </summary>
        [JsonProperty("isDeleted")]
        public bool IsDeleted
        {
            get;
            set;
        }
    }
}