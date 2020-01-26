namespace Zu.ChromeDevTools.Media
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// Break out events into different types
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlayerEventType
    {
        [EnumMember(Value = "errorEvent")]
        ErrorEvent,
        [EnumMember(Value = "triggeredEvent")]
        TriggeredEvent,
        [EnumMember(Value = "messageEvent")]
        MessageEvent,
    }
}