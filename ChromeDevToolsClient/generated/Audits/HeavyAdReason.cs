namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HeavyAdReason
    {
        [EnumMember(Value = "NetworkTotalLimit")]
        NetworkTotalLimit,
        [EnumMember(Value = "CpuTotalLimit")]
        CpuTotalLimit,
        [EnumMember(Value = "CpuPeakLimit")]
        CpuPeakLimit,
    }
}