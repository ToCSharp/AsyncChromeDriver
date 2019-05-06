namespace Zu.ChromeDevTools.BackgroundService
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Background Service that will be associated with the commands/events.
    /// Every Background Service operates independently, but they share the same
    /// API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServiceName
    {
        [EnumMember(Value = "backgroundFetch")]
        BackgroundFetch,
        [EnumMember(Value = "backgroundSync")]
        BackgroundSync,
    }
}