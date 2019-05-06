namespace Zu.ChromeDevTools.WebAudio
{
    using Newtonsoft.Json;

    /// <summary>
    /// Fields in AudioContext that change in real-time. These are not updated
    /// on OfflineAudioContext.
    /// </summary>
    public sealed class ContextRealtimeData
    {
        /// <summary>
        /// The current context time in second in BaseAudioContext.
        ///</summary>
        [JsonProperty("currentTime", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? CurrentTime
        {
            get;
            set;
        }
        /// <summary>
        /// The time spent on rendering graph divided by render qunatum duration,
        /// and multiplied by 100. 100 means the audio renderer reached the full
        /// capacity and glitch may occur.
        ///</summary>
        [JsonProperty("renderCapacity", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? RenderCapacity
        {
            get;
            set;
        }
    }
}