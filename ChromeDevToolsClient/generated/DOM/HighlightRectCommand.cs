namespace Zu.ChromeDevTools.DOM
{
    using Newtonsoft.Json;

    /// <summary>
    /// Highlights given rectangle.
    /// </summary>
    public sealed class HighlightRectCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "DOM.highlightRect";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

    }

    public sealed class HighlightRectCommandResponse : ICommandResponse<HighlightRectCommand>
    {
    }
}