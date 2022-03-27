using System.Text.Json.Serialization;

record ProtocolDefinition(Version Version, List<DomainDefinition> Domains);
record Version(string Major, string Minor);
record DomainDefinition(
    string Domain,
    string Description,
    bool? Experimental,
    bool? Deprecated,
    List<string> Dependencies,
    List<TypeDefinition> Types,
    List<CommandDefinition> Commands,
    List<EventDefinition> Events
    );
record PropertiyDefinition(
    string Name,
    string Description,
    bool? Experimental,
    bool? Deprecated,
    bool? Optional,
    string Type,
    List<string> Enum,
    [property: JsonPropertyName("$ref")]
    string Ref,
    ItemDefinition Items
    );
record ItemDefinition(
    string Type,
    [property: JsonPropertyName("$ref")]
    string Ref);
record TypeDefinition(
    string Id,
    string Description,
    bool? Experimental,
    bool? Deprecated,
    bool? Optional,
    string Type,
    List<string> Enum,
    List<PropertiyDefinition> Properties,
    ItemDefinition Items
    );
record CommandDefinition(
    string Name,
    string Description,
    bool? Experimental,
    bool? Deprecated,
    string Redirect,
    List<PropertiyDefinition> Parameters,
    List<PropertiyDefinition> Returns
    );
record EventDefinition(
    string Name,
    string Description,
    bool? Experimental,
    bool? Deprecated,
    List<PropertiyDefinition> Parameters
    );
