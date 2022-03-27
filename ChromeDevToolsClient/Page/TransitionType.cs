namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// Transition type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransitionType
    {
        [EnumMember(Value = "link")]
        Link,
        [EnumMember(Value = "typed")]
        Typed,
        [EnumMember(Value = "address_bar")]
        Address_bar,
        [EnumMember(Value = "auto_bookmark")]
        Auto_bookmark,
        [EnumMember(Value = "auto_subframe")]
        Auto_subframe,
        [EnumMember(Value = "manual_subframe")]
        Manual_subframe,
        [EnumMember(Value = "generated")]
        Generated,
        [EnumMember(Value = "auto_toplevel")]
        Auto_toplevel,
        [EnumMember(Value = "form_submit")]
        Form_submit,
        [EnumMember(Value = "reload")]
        Reload,
        [EnumMember(Value = "keyword")]
        Keyword,
        [EnumMember(Value = "keyword_generated")]
        Keyword_generated,
        [EnumMember(Value = "other")]
        Other,
    }
}