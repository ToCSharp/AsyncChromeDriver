namespace Zu.ChromeDevTools.Overlay
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ContainerQueryContainerHighlightConfig
    {
        /// <summary>
        /// The style of the container border.
        ///</summary>
        [JsonProperty("containerBorder", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LineStyle ContainerBorder
        {
            get;
            set;
        }
        /// <summary>
        /// The style of the descendants' borders.
        ///</summary>
        [JsonProperty("descendantBorder", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LineStyle DescendantBorder
        {
            get;
            set;
        }
    }
}