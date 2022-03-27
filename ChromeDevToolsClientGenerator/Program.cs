// See https://aka.ms/new-console-template for more information
using System.Text.Json;

JsonSerializerOptions options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true,
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    //IncludeFields = true,
};

var jsDef = JsonSerializer.Deserialize<ProtocolDefinition>(File.ReadAllText("js_protocol.json"), options);
var browserDef = JsonSerializer.Deserialize<ProtocolDefinition>(File.ReadAllText("browser_protocol.json"), options);

File.WriteAllText("js_protocol_parsed.json", JsonSerializer.Serialize(jsDef, options));
File.WriteAllText("browser_protocol_parsed.json", JsonSerializer.Serialize(browserDef, options));

Templates.Init(jsDef, browserDef);
var files = Templates.GetFiles(jsDef).Concat(Templates.GetFiles(browserDef)).Concat(Templates.GetFiles(jsDef, browserDef));
foreach (var file in files)
{
    if(file.Source == null)
        continue;
    var path = Path.Combine("ChromeDevToolsClient", file.Path);
    Directory.CreateDirectory(Path.GetDirectoryName(path));
    File.WriteAllText(path, file.Source);
}
//Console.WriteLine(jsDef);
//Console.ReadLine();