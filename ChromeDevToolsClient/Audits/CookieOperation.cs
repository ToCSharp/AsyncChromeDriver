namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CookieOperation
    {
        [EnumMember(Value = "SetCookie")]
        SetCookie,
        [EnumMember(Value = "ReadCookie")]
        ReadCookie,
    }
}