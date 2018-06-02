namespace Zu.ChromeDevTools.Runtime
{
    using Newtonsoft.Json;

    /// <summary>
    /// Adds binding with the given name on the global objects of all inspected
    /// contexts, including those created later. Bindings survive reloads.
    /// Binding function takes exactly one argument, this argument should be string,
    /// in case of any other input, function throws an exception.
    /// Each binding function call produces Runtime.bindingCalled notification.
    /// </summary>
    public sealed class AddBindingCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Runtime.addBinding";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
    }

    public sealed class AddBindingCommandResponse : ICommandResponse<AddBindingCommand>
    {
    }
}