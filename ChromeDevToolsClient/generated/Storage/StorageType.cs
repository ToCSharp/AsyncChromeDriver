namespace Zu.ChromeDevTools.Storage
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum of possible storage types.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StorageType
    {
        [EnumMember(Value = "appcache")]
        Appcache,
        [EnumMember(Value = "cookies")]
        Cookies,
        [EnumMember(Value = "file_systems")]
        File_systems,
        [EnumMember(Value = "indexeddb")]
        Indexeddb,
        [EnumMember(Value = "local_storage")]
        Local_storage,
        [EnumMember(Value = "shader_cache")]
        Shader_cache,
        [EnumMember(Value = "websql")]
        Websql,
        [EnumMember(Value = "service_workers")]
        Service_workers,
        [EnumMember(Value = "cache_storage")]
        Cache_storage,
        [EnumMember(Value = "interest_groups")]
        Interest_groups,
        [EnumMember(Value = "all")]
        All,
        [EnumMember(Value = "other")]
        Other,
    }
}