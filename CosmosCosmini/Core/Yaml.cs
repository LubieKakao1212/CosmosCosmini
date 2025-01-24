using System.Diagnostics.CodeAnalysis;
using JustLoaded.Content;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Filesystem;
using JustLoaded.Logger;
using PathLib;
using YamlDotNet.Serialization;

namespace CosmosCosmini.Core;

public static class Yaml {
    
    public static void LoadFilesMulti<TParsed, TContent>(this ModLoaderSystem modLoader, IFilesystem fs, string extension, Func<TParsed, ContentKey, IPurePath, IEnumerable<TContent>> processor, Func<TParsed, int, string> idSuffixProvider, IContentDatabase? customDb = null, IDeserializer? deserializer = null) where TContent : class {
        var db = customDb ?? modLoader.MasterDb.GetByContentType<TContent>() ?? throw new ApplicationException("Could not find requested db for loading");
        
        deserializer ??= modLoader.GetRequiredAttachment<IDeserializer>();
        
        fs.ParseFiles<TParsed>(extension, deserializer,(parsed, key, dir) => {
            int i = 0;
            foreach (var content in processor(parsed, key, dir)) {
                db.AddContent(new ContentKey(key.source, $"{key.path}_{idSuffixProvider(parsed, i++)}"), content);
            }
        });
    }
    
    public static void LoadFilesMono<TParsed, TContent>(this ModLoaderSystem modLoader, IFilesystem fs, string extension, Func<TParsed, ContentKey, IPurePath, TContent> processor, IContentDatabase? customDb = null, IDeserializer? deserializer = null) where TContent : class {
        var db = customDb ?? modLoader.MasterDb.GetByContentType<TContent>() ?? throw new ApplicationException("Could not find requested db for loading");
        deserializer ??= modLoader.GetRequiredAttachment<IDeserializer>();
        
        
        fs.ParseFiles<TParsed>(extension, deserializer, (parsed, key, dir) => {
            db.AddContent(key, processor(parsed, key, dir));
        });
    }
    
    public static void ParseFiles<TParsed>(this IFilesystem fs, string extension, IDeserializer deserializer, Action<TParsed, ContentKey, IPurePath> registrator) {
        var fullExtension = $".{extension}.yaml";
        var pattern = "*" + fullExtension;
        
        foreach (var file in
                 fs.ListFiles(
                     ".".AsPath().FromAnyMod(),
                     pattern, true)) {
            var parsed = fs.DeserializeYamlFile<TParsed>(file, deserializer);

            var idBase = file.path.ToPosix();
            idBase = idBase.Substring(0, idBase.Length - fullExtension.Length);

            registrator(parsed, new ContentKey(file.modSelector, idBase), file.path.Directory.AsPath());
        }
    }
    
    public static T DeserializeYamlFile<T>(this IFilesystem fs, ModAssetPath filePath, IDeserializer deserializer) {
        using var fileReader = new StreamReader(fs.OpenFile(filePath)!);
        var parsed = deserializer.Deserialize<T>(fileReader);
        return parsed;
    }

    #region Non-Generic
    public static object? DeserializeYamlFile(this IFilesystem fs, ModAssetPath filePath, IDeserializer deserializer, Type type) {
        using var fileReader = new StreamReader(fs.OpenFile(filePath)!);
        var parsed = deserializer.Deserialize(fileReader, type);
        return parsed;
    }
    
    public static void LoadDefsReflected(this ModLoaderSystem modLoader, IFilesystem fs, string extension, Type defType, Action<object, ContentKey> registrator, IDeserializer? deserializer = null, ILogger? logger = null) {
        deserializer ??= modLoader.GetRequiredAttachment<IDeserializer>();
        logger ??= modLoader.GetRequiredAttachment<ILogger>();
        
        var fullExtension = $".{extension}.yaml";
        var pattern = "*" + fullExtension;
        
        foreach (var file in
                 fs.ListFiles(
                     ".".AsPath().FromAnyMod(),
                     pattern, true)) {
            logger.Info($"Loading {file}");
            
            var parsed = fs.DeserializeYamlFile(file, deserializer, defType)!;

            var idBase = file.path.ToPosix();
            idBase = idBase.Substring(0, idBase.Length - fullExtension.Length);

            registrator(parsed, new ContentKey(file.modSelector, idBase));
        }
    }
    #endregion
    
}