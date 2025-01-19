using System.IO;
using Newtonsoft.Json;

namespace TwitchBot;

public static class ModulePersistence
{
    

    public static void SaveModule(Module module, string filePath)
    {
        var data = module.ToSerializableData();
        // Serialize the data with pretty-printing for readability
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static Module LoadModule(string filePath)
    {
        var json = File.ReadAllText(filePath);
        // Deserialize the JSON to ModuleData
        var data = JsonConvert.DeserializeObject<ModuleData>(json);
        if (data == null)
            throw new InvalidOperationException("Failed to deserialize ModuleData.");

        return Module.FromSerializableData(data);
    }

    public static void DeteleFile(string filePath)
    {
        if(File.Exists(filePath))
            File.Delete(filePath);
    }
}