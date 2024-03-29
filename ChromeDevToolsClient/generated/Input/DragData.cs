namespace Zu.ChromeDevTools.Input
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DragData
    {
        /// <summary>
        /// Gets or sets the items
        /// </summary>
        [JsonProperty("items")]
        public DragDataItem[] Items
        {
            get;
            set;
        }
        /// <summary>
        /// List of filenames that should be included when dropping
        ///</summary>
        [JsonProperty("files", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Files
        {
            get;
            set;
        }
        /// <summary>
        /// Bit field representing allowed drag operations. Copy = 1, Link = 2, Move = 16
        ///</summary>
        [JsonProperty("dragOperationsMask")]
        public long DragOperationsMask
        {
            get;
            set;
        }
    }
}