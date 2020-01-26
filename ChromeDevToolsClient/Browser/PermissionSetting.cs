namespace Zu.ChromeDevTools.Browser
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissionSetting
    {
        [EnumMember(Value = "granted")]
        Granted,
        [EnumMember(Value = "denied")]
        Denied,
        [EnumMember(Value = "prompt")]
        Prompt,
    }
}