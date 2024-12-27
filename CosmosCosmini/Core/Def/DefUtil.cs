using JustLoaded.Filesystem;

namespace CosmosCosmini.Core.Def;

public static class DefUtil {

    public static T DeserializeYamlFile<T>(this IFilesystem fs, ModAssetPath filePath) {
        using var fileReader = new StreamReader(fs.OpenFile(filePath)!);
        var parsed = Yaml.Deserializer.Deserialize<T>(fileReader);
        return parsed;
    }
    
    public static Stream OpenRequiredFile(this IFilesystem fs, ModAssetPath filePath) {
        var fileStream = fs.OpenFile(filePath);
        if (fileStream == null) {
            throw new IOException($"Could not open required file {filePath}");
        }
        return fileStream;
    }
    

}