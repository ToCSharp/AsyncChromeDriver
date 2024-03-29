namespace Zu.ChromeDevTools.Audits
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CookieExclusionReason
    {
        [EnumMember(Value = "ExcludeSameSiteUnspecifiedTreatedAsLax")]
        ExcludeSameSiteUnspecifiedTreatedAsLax,
        [EnumMember(Value = "ExcludeSameSiteNoneInsecure")]
        ExcludeSameSiteNoneInsecure,
        [EnumMember(Value = "ExcludeSameSiteLax")]
        ExcludeSameSiteLax,
        [EnumMember(Value = "ExcludeSameSiteStrict")]
        ExcludeSameSiteStrict,
        [EnumMember(Value = "ExcludeInvalidSameParty")]
        ExcludeInvalidSameParty,
        [EnumMember(Value = "ExcludeSamePartyCrossPartyContext")]
        ExcludeSamePartyCrossPartyContext,
    }
}