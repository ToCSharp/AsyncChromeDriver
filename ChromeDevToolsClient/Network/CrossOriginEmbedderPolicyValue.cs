namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CrossOriginEmbedderPolicyValue
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Credentialless")]
        Credentialless,
        [EnumMember(Value = "RequireCorp")]
        RequireCorp,
    }
}