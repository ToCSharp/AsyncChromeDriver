namespace Zu.ChromeDevTools.Storage
{
    using Newtonsoft.Json;

    /// <summary>
    /// Pair of issuer origin and number of available (signed, but not used) Trust
    /// Tokens from that issuer.
    /// </summary>
    public sealed class TrustTokens
    {
        /// <summary>
        /// Gets or sets the issuerOrigin
        /// </summary>
        [JsonProperty("issuerOrigin")]
        public string IssuerOrigin
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the count
        /// </summary>
        [JsonProperty("count")]
        public double Count
        {
            get;
            set;
        }
    }
}