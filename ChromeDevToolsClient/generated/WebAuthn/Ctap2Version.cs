namespace Zu.ChromeDevTools.WebAuthn
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Ctap2Version
    {
        [EnumMember(Value = "ctap2_0")]
        Ctap2_0,
        [EnumMember(Value = "ctap2_1")]
        Ctap2_1,
    }
}