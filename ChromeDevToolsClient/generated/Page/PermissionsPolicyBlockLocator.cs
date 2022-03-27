namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class PermissionsPolicyBlockLocator
    {
        /// <summary>
        /// Gets or sets the frameId
        /// </summary>
        [JsonProperty("frameId")]
        public string FrameId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the blockReason
        /// </summary>
        [JsonProperty("blockReason")]
        public PermissionsPolicyBlockReason BlockReason
        {
            get;
            set;
        }
    }
}