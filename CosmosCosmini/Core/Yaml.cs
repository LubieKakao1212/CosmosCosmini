using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Filesystem;
using PathLib;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core;

public static class Yaml {

    public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
        .WithEnforceRequiredMembers()
        .WithEnforceNullability()
        //TODO With value deserializers
        .Build();

    public static void LoadFilesMulti<TParsed, TContent>(this ModLoaderSystem modLoader, IFilesystem fs, string extension, Func<TParsed, ContentKey, IPurePath, IEnumerable<TContent>> processor, Func<TParsed, int, string> idSuffixProvider, IContentDatabase? customDb = null) where TContent : class {
        var db = customDb ?? modLoader.MasterDb.GetByContentType<TContent>() ?? throw new ApplicationException("Could not find requested db for loading");
        
        fs.ParseFiles<TParsed>(extension, (parsed, key, dir) => {
            int i = 0;
            foreach (var content in processor(parsed, key, dir)) {
                db.AddContent(new ContentKey(key.source, $"{key.path}_{idSuffixProvider(parsed, i++)}"), content);
            }
        });
    }
    
    public static void LoadFilesMono<TParsed, TContent>(this ModLoaderSystem modLoader, IFilesystem fs, string extension, Func<TParsed, ContentKey, IPurePath, TContent> processor, IContentDatabase? customDb = null) where TContent : class {
        var db = customDb ?? modLoader.MasterDb.GetByContentType<TContent>() ?? throw new ApplicationException("Could not find requested db for loading");

        fs.ParseFiles<TParsed>(extension, (parsed, key, dir) => {
            db.AddContent(key, processor(parsed, key, dir));
        });
    }
    
    public static void ParseFiles<TParsed>(this IFilesystem fs, string extension, Action<TParsed, ContentKey, IPurePath> registrator) {
        var fullExtension = $".{extension}.yaml";
        var pattern = "*" + extension;
        
        foreach (var file in
                 fs.ListFiles(
                     ".".AsPath().FromAnyMod(),
                     pattern, true)) {
            var parsed = fs.DeserializeYamlFile<TParsed>(file);

            var idBase = file.path.ToPosix();
            idBase = idBase.Substring(0, idBase.Length - fullExtension.Length);

            registrator(parsed, new ContentKey(file.modSelector, idBase), file.path.Directory.AsPath());
        }
    }
    
    public static T DeserializeYamlFile<T>(this IFilesystem fs, ModAssetPath filePath) {
        using var fileReader = new StreamReader(fs.OpenFile(filePath)!);
        var parsed = Deserializer.Deserialize<T>(fileReader);
        return parsed;
    }
    
    
}