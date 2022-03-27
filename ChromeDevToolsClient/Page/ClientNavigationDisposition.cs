namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ClientNavigationDisposition
    {
        [EnumMember(Value = "currentTab")]
        CurrentTab,
        [EnumMember(Value = "newTab")]
        NewTab,
        [EnumMember(Value = "newWindow")]
        NewWindow,
        [EnumMember(Value = "download")]
        Download,
    }
}