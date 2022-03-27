namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttributionReportingIssueType
    {
        [EnumMember(Value = "PermissionPolicyDisabled")]
        PermissionPolicyDisabled,
        [EnumMember(Value = "InvalidAttributionSourceEventId")]
        InvalidAttributionSourceEventId,
        [EnumMember(Value = "InvalidAttributionData")]
        InvalidAttributionData,
        [EnumMember(Value = "AttributionSourceUntrustworthyOrigin")]
        AttributionSourceUntrustworthyOrigin,
        [EnumMember(Value = "AttributionUntrustworthyOrigin")]
        AttributionUntrustworthyOrigin,
        [EnumMember(Value = "AttributionTriggerDataTooLarge")]
        AttributionTriggerDataTooLarge,
        [EnumMember(Value = "AttributionEventSourceTriggerDataTooLarge")]
        AttributionEventSourceTriggerDataTooLarge,
        [EnumMember(Value = "InvalidAttributionSourceExpiry")]
        InvalidAttributionSourceExpiry,
        [EnumMember(Value = "InvalidAttributionSourcePriority")]
        InvalidAttributionSourcePriority,
        [EnumMember(Value = "InvalidEventSourceTriggerData")]
        InvalidEventSourceTriggerData,
        [EnumMember(Value = "InvalidTriggerPriority")]
        InvalidTriggerPriority,
        [EnumMember(Value = "InvalidTriggerDedupKey")]
        InvalidTriggerDedupKey,
    }
}