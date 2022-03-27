namespace Zu.ChromeDevTools.Page
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class BackForwardCacheNotRestoredExplanationTree
    {
        /// <summary>
        /// URL of each frame
        ///</summary>
        [JsonProperty("url")]
        public string Url
        {
            get;
            set;
        }
        /// <summary>
        /// Not restored reasons of each frame
        ///</summary>
        [JsonProperty("explanations")]
        public BackForwardCacheNotRestoredExplanation[] Explanations
        {
            get;
            set;
        }
        /// <summary>
        /// Array of children frame
        ///</summary>
        [JsonProperty("children")]
        public BackForwardCacheNotRestoredExplanationTree[] Children
        {
            get;
            set;
        }
    }
}