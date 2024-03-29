namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrivateNetworkRequestPolicy
    {
        [EnumMember(Value = "Allow")]
        Allow,
        [EnumMember(Value = "BlockFromInsecureToMorePrivate")]
        BlockFromInsecureToMorePrivate,
        [EnumMember(Value = "WarnFromInsecureToMorePrivate")]
        WarnFromInsecureToMorePrivate,
        [EnumMember(Value = "PreflightBlock")]
        PreflightBlock,
        [EnumMember(Value = "PreflightWarn")]
        PreflightWarn,
    }
}