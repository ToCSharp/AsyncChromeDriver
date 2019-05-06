namespace Zu.ChromeDevTools.Cast
{
    using Newtonsoft.Json;

    /// <summary>
    /// This is fired whenever the list of available sinks changes. A sink is a
        /// device or a software surface that you can cast to.
    /// </summary>
    public sealed class SinksUpdatedEvent : IEvent
    {
        /// <summary>
        /// Gets or sets the sinkNames
        /// </summary>
        [JsonProperty("sinkNames")]
        public string[] SinkNames
        {
            get;
            set;
        }
    }
}