namespace Zu.ChromeDevTools.Security
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SafetyTipStatus
    {
        [EnumMember(Value = "badReputation")]
        BadReputation,
        [EnumMember(Value = "lookalike")]
        Lookalike,
    }
}