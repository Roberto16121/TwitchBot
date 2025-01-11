using System.IO;
using System.Text.Json;

namespace TwitchBot;

public static class ModulePersistence
{
    
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true // Pretty-print JSON for readability
    };

    public static void SaveModule(Module module, string filePath)
    {
        var data = module.ToSerializableData();
        var json = JsonSerializer.Serialize(data, JsonOptions);
        File.WriteAllText(filePath, json);
    }

    public static Module LoadModule(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<ModuleData>(json);
        if (data == null)
            throw new InvalidOperationException("Failed to deserialize ModuleData.");

        return Module.FromSerializableData(data);
    }
}