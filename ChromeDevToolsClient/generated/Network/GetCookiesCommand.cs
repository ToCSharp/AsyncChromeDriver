namespace Zu.ChromeDevTools.Network
{
    using Newtonsoft.Json;

    /// <summary>
    /// Returns all browser cookies for the current URL. Depending on the backend support, will return
    /// detailed cookie information in the `cookies` field.
    /// </summary>
    public sealed class GetCookiesCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Network.getCookies";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// The list of URLs for which applicable cookies will be fetched.
        /// If not specified, it's assumed to be set to the list containing
        /// the URLs of the page and all of its subframes.
        /// </summary>
        [JsonProperty("urls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Urls
        {
            get;
            set;
        }
    }

    public sealed class GetCookiesCommandResponse : ICommandResponse<GetCookiesCommand>
    {
        /// <summary>
        /// Array of cookie objects.
        ///</summary>
        [JsonProperty("cookies")]
        public Cookie[] Cookies
        {
            get;
            set;
        }
    }
}