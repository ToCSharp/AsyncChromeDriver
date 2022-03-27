namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CrossOriginOpenerPolicyValue
    {
        [EnumMember(Value = "SameOrigin")]
        SameOrigin,
        [EnumMember(Value = "SameOriginAllowPopups")]
        SameOriginAllowPopups,
        [EnumMember(Value = "UnsafeNone")]
        UnsafeNone,
        [EnumMember(Value = "SameOriginPlusCoep")]
        SameOriginPlusCoep,
        [EnumMember(Value = "SameOriginAllowPopupsPlusCoep")]
        SameOriginAllowPopupsPlusCoep,
    }
}