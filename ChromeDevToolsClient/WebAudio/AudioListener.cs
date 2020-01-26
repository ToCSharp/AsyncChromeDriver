namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Protocol object for AudioListner
    /// </summary>
    public sealed class AudioListener
    {
        /// <summary>
        /// 
        ///</summary>
        [JsonProperty("listenerId")]
        public string ListenerId
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
    }
}