namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Notifies that existing BaseAudioContext has been destroyed.
    /// </summary>
    public sealed class ContextDestroyedEvent : IEvent
    {
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