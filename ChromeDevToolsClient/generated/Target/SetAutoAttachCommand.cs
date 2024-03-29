namespace Zu.ChromeDevTools.Target
{
    using Newtonsoft.Json;

    /// <summary>
    /// Controls whether to automatically attach to new targets which are considered to be related to
    /// this one. When turned on, attaches to all existing related targets as well. When turned off,
    /// automatically detaches from all currently attached targets.
    /// This also clears all targets added by `autoAttachRelated` from the list of targets to watch
    /// for creation of related targets.
    /// </summary>
    public sealed class SetAutoAttachCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Target.setAutoAttach";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Whether to auto-attach to related targets.
        /// </summary>
        [JsonProperty("autoAttach")]
        public bool AutoAttach
        {
            get;
            set;
        }
        /// <summary>
        /// Whether to pause new targets when attaching to them. Use `Runtime.runIfWaitingForDebugger`
        /// to run paused targets.
        /// </summary>
        [JsonProperty("waitForDebuggerOnStart")]
        public bool WaitForDebuggerOnStart
        {
            get;
            set;
        }
        /// <summary>
        /// Enables "flat" access to the session via specifying sessionId attribute in the commands.
        /// We plan to make this the default, deprecate non-flattened mode,
        /// and eventually retire it. See crbug.com/991325.
        /// </summary>
        [JsonProperty("flatten", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Flatten
        {
            get;
            set;
        }
    }

    public sealed class SetAutoAttachCommandResponse : ICommandResponse<SetAutoAttachCommand>
    {
    }
}