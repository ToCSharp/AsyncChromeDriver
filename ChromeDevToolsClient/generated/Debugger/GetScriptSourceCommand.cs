namespace Zu.ChromeDevTools.Debugger
{
    using Newtonsoft.Json;

    /// <summary>
    /// Returns source for the script with given id.
    /// </summary>
    public sealed class GetScriptSourceCommand : ICommand
    {
        private const string ChromeRemoteInterface_CommandName = "Debugger.getScriptSource";
        
        [JsonIgnore]
        public string CommandName
        {
            get { return ChromeRemoteInterface_CommandName; }
        }

        /// <summary>
        /// Id of the script to get source for.
        /// </summary>
        [JsonProperty("scriptId")]
        public string ScriptId
        {
            get;
            set;
        }
    }

    public sealed class GetScriptSourceCommandResponse : ICommandResponse<GetScriptSourceCommand>
    {
        /// <summary>
        /// Script source (empty in case of Wasm bytecode).
        ///</summary>
        [JsonProperty("scriptSource")]
        public string ScriptSource
        {
            get;
            set;
        }
        /// <summary>
        /// Wasm bytecode. (Encoded as a base64 string when passed over JSON)
        ///</summary>
        [JsonProperty("bytecode", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Bytecode
        {
            get;
            set;
        }
    }
}