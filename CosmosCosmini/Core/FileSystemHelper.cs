using Custom2d_Engine.Util;
using JustLoaded.Core;
using JustLoaded.Filesystem;

namespace CosmosCosmini.Core;

public static class FileSystems {

    public static string SpritesDir = "sprites";
    
    private static readonly Dictionary<string, IFilesystem> Filesystems = new();
    
    public static IFilesystem GetModDirectory(this ModLoaderSystem mls, string dir) {
        //TODO use mls instead of CosmosGame
        return Filesystems.GetOrSetToDefaultLazy(dir, s => new RelativeFilesystem(CosmosGame.Filesystem, dir.AsPath()));
    }
    
    public static Stream OpenRequiredFile(this IFilesystem fs, ModAssetPath filePath) {
        var fileStream = fs.OpenFile(filePath);
        if (fileStream == null) {
            throw new IOException($"Could not open required file {filePath}");
        }
        return fileStream;
    }
}