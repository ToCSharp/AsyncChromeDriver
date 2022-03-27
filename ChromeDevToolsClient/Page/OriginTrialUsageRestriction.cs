namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OriginTrialUsageRestriction
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Subset")]
        Subset,
    }
}