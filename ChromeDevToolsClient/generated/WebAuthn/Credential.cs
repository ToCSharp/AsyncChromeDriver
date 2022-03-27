namespace Zu.ChromeDevTools.WebAuthn
{
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public sealed class Credential
    {
        /// <summary>
        /// Gets or sets the credentialId
        /// </summary>
        [JsonProperty("credentialId")]
        public string CredentialId
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the isResidentCredential
        /// </summary>
        [JsonProperty("isResidentCredential")]
        public bool IsResidentCredential
        {
            get;
            set;
        }
        /// <summary>
        /// Relying Party ID the credential is scoped to. Must be set when adding a
        /// credential.
        ///</summary>
        [JsonProperty("rpId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RpId
        {
            get;
            set;
        }
        /// <summary>
        /// The ECDSA P-256 private key in PKCS#8 format. (Encoded as a base64 string when passed over JSON)
        ///</summary>
        [JsonProperty("privateKey")]
        public string PrivateKey
        {
            get;
            set;
        }
        /// <summary>
        /// An opaque byte sequence with a maximum size of 64 bytes mapping the
        /// credential to a specific user. (Encoded as a base64 string when passed over JSON)
        ///</summary>
        [JsonProperty("userHandle", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserHandle
        {
            get;
            set;
        }
        /// <summary>
        /// Signature counter. This is incremented by one for each successful
        /// assertion.
        /// See https://w3c.github.io/webauthn/#signature-counter
        ///</summary>
        [JsonProperty("signCount")]
        public long SignCount
        {
            get;
            set;
        }
        /// <summary>
        /// The large blob associated with the credential.
        /// See https://w3c.github.io/webauthn/#sctn-large-blob-extension (Encoded as a base64 string when passed over JSON)
        ///</summary>
        [JsonProperty("largeBlob", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string LargeBlob
        {
            get;
            set;
        }
    }
}