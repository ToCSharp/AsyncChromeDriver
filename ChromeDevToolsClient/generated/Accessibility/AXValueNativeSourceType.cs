namespace Zu.ChromeDevTools.Accessibility
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum of possible native property sources (as a subtype of a particular AXValueSourceType).
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AXValueNativeSourceType
    {
        [EnumMember(Value = "description")]
        Description,
        [EnumMember(Value = "figcaption")]
        Figcaption,
        [EnumMember(Value = "label")]
        Label,
        [EnumMember(Value = "labelfor")]
        Labelfor,
        [EnumMember(Value = "labelwrapped")]
        Labelwrapped,
        [EnumMember(Value = "legend")]
        Legend,
        [EnumMember(Value = "rubyannotation")]
        Rubyannotation,
        [EnumMember(Value = "tablecaption")]
        Tablecaption,
        [EnumMember(Value = "title")]
        Title,
        [EnumMember(Value = "other")]
        Other,
    }
}