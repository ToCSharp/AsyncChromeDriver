namespace Zu.ChromeDevTools.Overlay
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContrastAlgorithm
    {
        [EnumMember(Value = "aa")]
        Aa,
        [EnumMember(Value = "aaa")]
        Aaa,
        [EnumMember(Value = "apca")]
        Apca,
    }
}