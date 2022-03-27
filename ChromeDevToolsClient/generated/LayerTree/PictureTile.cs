namespace Zu.ChromeDevTools.LayerTree
{
    using Newtonsoft.Json;

    /// <summary>
    /// Serialized fragment of layer picture along with its offset within the layer.
    /// </summary>
    public sealed class PictureTile
    {
        /// <summary>
        /// Offset from owning layer left boundary
        ///</summary>
        [JsonProperty("x")]
        public double X
        {
            get;
            set;
        }
        /// <summary>
        /// Offset from owning layer top boundary
        ///</summary>
        [JsonProperty("y")]
        public double Y
        {
            get;
            set;
        }
        /// <summary>
        /// Base64-encoded snapshot data. (Encoded as a base64 string when passed over JSON)
        ///</summary>
        [JsonProperty("picture")]
        public string Picture
        {
            get;
            set;
        }
    }
}