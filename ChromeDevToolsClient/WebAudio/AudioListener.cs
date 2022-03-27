namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Protocol object for AudioListener
    /// </summary>
    public sealed class AudioListener
    {
        /// <summary>
        /// Gets or sets the listenerId
        /// </summary>
        [JsonProperty("listenerId")]
        public string ListenerId
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
    }
}