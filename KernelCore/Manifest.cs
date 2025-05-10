using YamlDotNet.Serialization;

namespace Presentation;

public class Manifest
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; }

    [YamlMember(Alias = "permissions")]
    public List<string> Permissions { get; set; }

    public static Manifest LoadFromFile(string path)
    {
        var deserializer = new DeserializerBuilder().Build();
        using var reader = new StreamReader(path);
        return deserializer.Deserialize<Manifest>(reader);
    }
}