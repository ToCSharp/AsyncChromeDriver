static class Templates
{
    const string rootNamespace = "Zu.ChromeDevTools";
    static Dictionary<string, string> simpleTypes;
    static List<string> enums;

    static string FirstToUpper(this string str) => str.Substring(0, 1).ToUpper() + str.Substring(1);
    static string ToRn(this string str) => str.Replace("\r\n", "\n").Replace("\n", "\r\n");

    // "User-agent" -> "UserAgent"
    static string EnumFix(this string str) => string.Join("", str.Split('-').Select(v => v.FirstToUpper()));

    internal static void Init(ProtocolDefinition jsDef, ProtocolDefinition browserDef)
    {
        simpleTypes = jsDef.Domains.Concat(browserDef.Domains)
            .Where(x => x.Types != null)
            .SelectMany(x => x.Types.Select(v => (Domain: x, Type: v)).Where(x => x.Type.Type != "object" && x.Type.Enum == null))
            .SelectMany(x => new[] { (Id: x.Type.Id, Type: GetType(x.Type)), (Id: $"{x.Domain.Domain}.{x.Type.Id}", Type: GetType(x.Type)) })
            .Distinct()
            .ToDictionary(x => x.Id, x => x.Type);
        simpleTypes["ArrayOfStrings"] = "long[]";
        enums = jsDef.Domains.Concat(browserDef.Domains)
            .Where(x => x.Types != null)
            .SelectMany(x => x.Types.Select(v => (Domain: x, Type: v)).Where(x => x.Type.Enum != null))
            .SelectMany(x => new[] { x.Type.Id, $"{x.Domain.Domain}.{x.Type.Id}" })
            .Distinct()
            .ToList();
    }

    static string GetType(TypeDefinition def)
    {
        string convert(string t) =>
            t == "integer" ? "long"
            : t == "number" ? "double"
            : t == "boolean" ? "bool"
            : null;
        string convert2(string t) =>
            t == "any" ? "object"
            : null;
        var op = def.Optional == true ? "?" : "";
        switch (def.Type)
        {
            case "integer":
            case "number":
            case "boolean":
                return convert(def.Type) + op;
            case "any":
                return "object";
            case "string":
            case "object":
                return def.Type;
            case "array":
                var v = def.Items.Ref ?? convert(def.Items.Type) ?? convert2(def.Items.Type) ?? def.Items.Type;
                return $"{v}[]";
            case null:
                throw new NotImplementedException();
            default:
                {
                    if (simpleTypes.TryGetValue(def.Type, out var t))
                    {
                        var c = convert(t);
                        if (c != null)
                            return c + op;
                        return t;
                    }
                }
                if (enums.Contains(def.Type))
                    return def.Type + op;
                return def.Type;
        }
    }

    internal static List<(string Path, string Source)> GetFiles(ProtocolDefinition def)
    {
        var res = def.Domains.SelectMany(x =>
        {
            var res = new List<(string Path, string Source)> {
                (Path.Combine(x.Domain, x.Domain.FirstToUpper()) + "Adapter.cs", GetSource(x))
            };
            if (x.Types != null)
                res.AddRange(x.Types.Select(v => (Path.Combine(x.Domain, v.Id.FirstToUpper()) + ".cs", GetSource(v, x.Domain))));
            if (x.Commands != null)
                res.AddRange(x.Commands.Select(v => (Path.Combine(x.Domain, v.Name.FirstToUpper()) + "Command.cs", GetSource(v, x.Domain))));
            if (x.Events != null)
                res.AddRange(x.Events.Select(v => (Path.Combine(x.Domain, v.Name.FirstToUpper()) + "Event.cs", GetSource(v, x.Domain))));
            return res;

        }
        ).ToList();
        return res;
    }

    internal static List<(string Path, string Source)> GetFiles(ProtocolDefinition jsDef, ProtocolDefinition browserDef)
    {
        var domains = browserDef.Domains.Concat(jsDef.Domains).ToList();
        var res = new List<(string Path, string Source)> ();
        res.Add(("ChromeSession_Domains.cs", GetChromeSession_Domains(domains)));
        res.Add(("CommandResponseTypeMap.cs", GetCommandResponseTypeMap(domains)));
        res.Add(("EventTypeMap.cs", GetEventTypeMap(domains)));
        return res;
    }

    static string GetChromeSession_Domains(List<DomainDefinition> domains)
    {
        var domainsNames = domains.Select(v => v.Domain.FirstToUpper()).ToList();
        return $@"namespace {rootNamespace}
{{
    using System;

    public partial class ChromeSession
    {{
{string.Join(Environment.NewLine, domainsNames.Select(v => $"        private Lazy<{v}.{v}Adapter> m_{v};"))}

        public ChromeSession()
        {{
{string.Join(Environment.NewLine, domainsNames.Select(v => $"            m_{v} = new Lazy<{v}.{v}Adapter>(() => new {v}.{v}Adapter(this));"))}
        }}

{string.Join(Environment.NewLine, domainsNames.Select(v => @$"        /// <summary>
        /// Gets the adapter for the {v} domain.
        /// </summary>
        public {v}.{v}Adapter {v}
        {{
            get {{ return m_{v}.Value; }}
        }}
"))}
    }}
}}
";
    }

    static string GetCommandResponseTypeMap(List<DomainDefinition> domains)
    {
        var fullTypeNames = domains.SelectMany(x =>
        {
            var res = new List<string>();
            if (x.Commands != null)
                res.AddRange(x.Commands.Select(v => $"{x.Domain}.{v.Name.FirstToUpper()}"));
            return res;
        }
        ).ToList();
        return $@"namespace {rootNamespace}
{{
    using System;
    using System.Collections.Generic;

    public static class CommandResponseTypeMap
    {{
        private readonly static IDictionary<Type, Type> s_commandResponseTypeDictionary;

        static CommandResponseTypeMap()
        {{
            s_commandResponseTypeDictionary = new Dictionary<Type, Type>()
            {{
{string.Join(Environment.NewLine, fullTypeNames.Select(v => $"                {{ typeof({v}Command), typeof({v}CommandResponse) }},"))}
            }};
        }}

        /// <summary>
        /// Gets the command response type corresponding to the specified command type
        /// </summary>
        public static bool TryGetCommandResponseType<T>(out Type commandResponseType)
            where T : ICommand
        {{
            return s_commandResponseTypeDictionary.TryGetValue(typeof(T), out commandResponseType);
        }}
    }}
}}";
    }

    static string GetEventTypeMap(List<DomainDefinition> domains)
    {
        var fullTypeNames = domains.SelectMany(x =>
        {
            var res = new List<(string, string)>();
            if (x.Events != null)
                res.AddRange(x.Events.Select(v => ($"{x.Domain}.{v.Name}", $"{x.Domain}.{v.Name.FirstToUpper()}")));
            return res;
        }
        ).ToList();
        return $@"namespace {rootNamespace}
{{
    using System;
    using System.Collections.Generic;

    public static class EventTypeMap
    {{
        private readonly static IDictionary<string, Type> s_methodNameEventTypeDictionary;
        private readonly static IDictionary<Type, string> s_eventTypeMethodNameDictionary;

        static EventTypeMap()
        {{
            s_methodNameEventTypeDictionary = new Dictionary<string, Type>()
            {{
{string.Join(Environment.NewLine, fullTypeNames.Select(v => $"                {{ \"{v.Item1}\", typeof({v.Item2}Event) }},"))}
            }};

            s_eventTypeMethodNameDictionary = new Dictionary<Type, string>()
            {{
{string.Join(Environment.NewLine, fullTypeNames.Select(v => $"                {{ typeof({v.Item2}Event), \"{v.Item1}\" }},"))}
            }};
        }}

        /// <summary>
        /// Gets the event type corresponding to the specified method name.
        /// </summary>
        public static bool TryGetTypeForMethodName(string methodName, out Type eventType)
        {{
            return s_methodNameEventTypeDictionary.TryGetValue(methodName, out eventType);
        }}

        /// <summary>
        /// Gets the method name corresponding to the specified event type.
        /// </summary>
        public static bool TryGetMethodNameForType<TEvent>(out string methodName)
            where TEvent : IEvent
        {{
            return s_eventTypeMethodNameDictionary.TryGetValue(typeof(TEvent), out methodName);
        }}
    }}
}}";
    }

    static string GetSource(DomainDefinition def)
    {
        var name = def.Domain.FirstToUpper();
        return $@"namespace {rootNamespace}.{def.Domain}
{{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an adapter for the {def.Domain} domain to simplify the command interface.
    /// </summary>
    public class {name}Adapter
    {{
        private readonly ChromeSession m_session;
        
        public {name}Adapter(ChromeSession session)
        {{
            m_session = session ?? throw new ArgumentNullException(nameof(session));
        }}

        /// <summary>
        /// Gets the ChromeSession associated with the adapter.
        /// </summary>
        public ChromeSession Session
        {{
            get {{ return m_session; }}
        }}
{(def.Commands == null ? "" : Environment.NewLine + string.Join(Environment.NewLine, def.Commands.Select(v => GetSource(v))))}
{(def.Events == null ? "" : Environment.NewLine + string.Join(Environment.NewLine, def.Events.Select(v => GetSource(v))))}
    }}
}}";
    }

    static string GetSource(CommandDefinition def)
    {
        var name = def.Name.FirstToUpper();
        return
    $@"        /// <summary>
        /// {def.Description?.ToRn().Replace("\n", "\n        /// ")}
        /// </summary>
        public async Task<{name}CommandResponse> {name}({name}Command command{(def.Parameters == null ? " = null" : "")}, CancellationToken cancellationToken = default(CancellationToken), int? millisecondsTimeout = null, bool throwExceptionIfResponseNotReceived = true)
        {{
            return await m_session.SendCommand<{name}Command, {name}CommandResponse>(command{(def.Parameters == null ? $" ?? new {name}Command()" : "")}, cancellationToken, millisecondsTimeout, throwExceptionIfResponseNotReceived);
        }}";
    }

    static string GetSource(EventDefinition def)
    {
        var name = def.Name.FirstToUpper();
        return
    $@"        /// <summary>
        /// {def.Description?.ToRn().Replace("\n", "\n        /// ")}
        /// </summary>
        public void SubscribeTo{name}Event(Action<{name}Event> eventCallback)
        {{
            m_session.Subscribe(eventCallback);
        }}";
    }

    static string GetSource(TypeDefinition def, string domain)
    {
        if (def.Type == "object")
        {
            return $@"namespace {rootNamespace}.{domain}
{{
    using Newtonsoft.Json;

    /// <summary>
    /// {def.Description?.ToRn().Replace("\n", "\n    /// ")}
    /// </summary>
    public sealed class {def.Id.FirstToUpper()}
    {{{(def.Properties == null ? "" : Environment.NewLine + string.Join(Environment.NewLine, def.Properties.Select(v => GetSource(v, true))))}
    }}
}}";
        }
        if (def.Enum != null && def.Type == "string")
        {
            return $@"namespace {rootNamespace}.{domain}
{{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// {def.Description?.ToRn().Replace("\n", "\n    /// ")}
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum {def.Id.FirstToUpper()}
    {{
{ string.Join(Environment.NewLine, def.Enum.Select(v => @$"        [EnumMember(Value = ""{v}"")]
        {v.FirstToUpper().EnumFix()},"))}
    }}
}}";

        }
        if (def.Type == "string" || def.Type == "integer" || def.Type == "number" || def.Type == "array")
        {
            // TODO
            return null;
        }
        throw new NotImplementedException();
    }

    static string GetSource(PropertiyDefinition def, bool crutch = false)
    {
        return (def.Description == null
            ? $@"        /// <summary>
        /// Gets or sets the {def.Name}
        /// </summary>"
            : $@"        /// <summary>
        /// {def.Description.ToRn().Replace("\n", "\n        /// ")}
        ///{(crutch ? "" : " ")}</summary>") + $@"
        [JsonProperty(""{def.Name}""{(def.Optional == true ? ", DefaultValueHandling = DefaultValueHandling.Ignore" : "")})]
        public {GetTypeStr(def)} {def.Name.FirstToUpper()}
        {{
            get;
            set;
        }}";
    }

    static string GetTypeStr(PropertiyDefinition def)
    {
        string convert(string t) =>
            t == "integer" ? "long"
            : t == "long" ? "long"
            : t == "number" ? "double"
            : t == "double" ? "double"
            : t == "boolean" ? "bool"
            : null;
        string convert2(string t) =>
            t == "any" ? "object"
            : null;
        var op = def.Optional == true ? "?" : "";
        switch (def.Type)
        {
            case "integer":
            case "number":
            case "boolean":
                return convert(def.Type) + op;
            case "any":
                return "object";
            case "string":
            case "object":
                return def.Type;
            case "array":
                {
                    if (simpleTypes.TryGetValue(def.Items.Ref ?? def.Items.Type, out var t))
                        return $"{convert(t) ?? t}[]";
                }
                var v = def.Items.Ref ?? convert(def.Items.Type) ?? convert2(def.Items.Type) ?? def.Items.Type;
                return $"{v}[]";
            case null:
                {
                    if (simpleTypes.TryGetValue(def.Ref, out var t))
                    {
                        var c = convert(t);
                        if (c != null)
                            return c + op;
                        return t;
                    }
                }
                if (enums.Contains(def.Ref))
                    return def.Ref + op;
                return def.Ref;
            default:
                {
                    if (simpleTypes.TryGetValue(def.Type, out var t))
                    {
                        var c = convert(t);
                        if (c != null)
                            return c + op;
                        return t;
                    }
                }
                if (enums.Contains(def.Type))
                    return def.Type + op;
                return def.Type;
        }
    }

    static string GetSource(CommandDefinition def, string domain)
    {
        return
    $@"namespace {rootNamespace}.{domain}
{{
    using Newtonsoft.Json;

    /// <summary>
    /// {def.Description?.ToRn().Replace("\n", "\n    /// ")}
    /// </summary>
    public sealed class {def.Name.FirstToUpper()}Command : ICommand
    {{
        private const string ChromeRemoteInterface_CommandName = ""{domain}.{ def.Name}"";
        
        [JsonIgnore]
        public string CommandName
        {{
            get {{ return ChromeRemoteInterface_CommandName; }}
        }}
{ (def.Parameters == null ? "" : Environment.NewLine + string.Join(Environment.NewLine, def.Parameters.Select(v => GetSource(v))))}
    }}

    public sealed class {def.Name.FirstToUpper()}CommandResponse : ICommandResponse<{def.Name.FirstToUpper()}Command>
    {{{ (def.Returns == null ? "" : Environment.NewLine + string.Join(Environment.NewLine, def.Returns.Select(v => GetSource(v, true))))}
    }}
}}";
    }

    static string GetSource(EventDefinition def, string domain)
    {
        return
    $@"namespace {rootNamespace}.{domain}
{{
    using Newtonsoft.Json;

    /// <summary>
    /// {def.Description?.ToRn().Replace("\n", "\n    /// ")}
    /// </summary>
    public sealed class {def.Name.FirstToUpper()}Event : IEvent
    {{{ (def.Parameters == null ? "" : Environment.NewLine + string.Join(Environment.NewLine, def.Parameters.Select(v => GetSource(v))))}
    }}
}}";
    }
}
