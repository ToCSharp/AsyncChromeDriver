namespace Zu.ChromeDevTools.DOMSnapshot
{
    using Newtonsoft.Json;

    /// <summary>
    /// Returns a document snapshot, including the full DOM tree of the root node (including iframes,
    /// template contents, and imported documents) in a flattened array, as well as layout and
    /// white-listed computed style information for the nodes. Shadow DOM in the returned DOM tree is
    /// flattened.
    /// </summary>
    public sealed class CaptureSnapshotCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "DOMSnapshot.captureSnapshot";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Whitelist of computed styles to return.
        /// </summary>
        [JsonProperty("computedStyles")]
        public string[] ComputedStyles
        {
            get;
            set;
        }
        /// <summary>
        /// Whether to include layout object paint orders into the snapshot.
        /// </summary>
        [JsonProperty("includePaintOrder", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IncludePaintOrder
        {
            get;
            set;
        }
        /// <summary>
        /// Whether to include DOM rectangles (offsetRects, clientRects, scrollRects) into the snapshot
        /// </summary>
        [JsonProperty("includeDOMRects", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IncludeDOMRects
        {
            get;
            set;
        }
    }

    public sealed class CaptureSnapshotCommandResponse : ICommandResponse<CaptureSnapshotCommand>
    {
        /// <summary>
        /// The nodes in the DOM tree. The DOMNode at index 0 corresponds to the root document.
        ///</summary>
        [JsonProperty("documents")]
        public DocumentSnapshot[] Documents
        {
            get;
            set;
        }
        /// <summary>
        /// Shared string table that all string properties refer to with indexes.
        ///</summary>
        [JsonProperty("strings")]
        public string[] Strings
        {
            get;
            set;
        }
    }
}