namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContentSecurityPolicyViolationType
    {
        [EnumMember(Value = "kInlineViolation")]
        KInlineViolation,
        [EnumMember(Value = "kEvalViolation")]
        KEvalViolation,
        [EnumMember(Value = "kURLViolation")]
        KURLViolation,
        [EnumMember(Value = "kTrustedTypesSinkViolation")]
        KTrustedTypesSinkViolation,
        [EnumMember(Value = "kTrustedTypesPolicyViolation")]
        KTrustedTypesPolicyViolation,
        [EnumMember(Value = "kWasmEvalViolation")]
        KWasmEvalViolation,
    }
}