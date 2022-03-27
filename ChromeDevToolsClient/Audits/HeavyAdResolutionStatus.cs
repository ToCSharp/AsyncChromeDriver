namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HeavyAdResolutionStatus
    {
        [EnumMember(Value = "HeavyAdBlocked")]
        HeavyAdBlocked,
        [EnumMember(Value = "HeavyAdWarning")]
        HeavyAdWarning,
    }
}