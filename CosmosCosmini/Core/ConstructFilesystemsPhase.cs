using CosmosCosmini.JustLoadedEx;
using JustLoaded.Content.Database;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Filesystem;
using JustLoaded.Logger;

namespace CosmosCosmini.Core;

public class ConstructFilesystemsPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var logger = modLoader.GetRequiredAttachment<ILogger>();
        
        List<IFilesystem> filesystems = new();

        var db = (IContentDatabase<Mod>)modLoader.MasterDb.GetByContentType<Mod>()!;
        
        //INFO: Last mod is searched first for files TODO add do modding doc
        foreach (var mod in db.ContentValues) {
            var fs = mod.GetGlobalObject<ModLocalFilesystemContainer>(typeof(ModLocalFilesystemContainer), () => null!)
                .ModLocalFileSystem;
            var cfs = new CombinedFilesystem();
            foreach (var path in fs.ListPaths(".".AsPath().FromAnyMod())) {
                //Filename is also a top directory name
                logger.Info("Detected path: " + path.path.Filename, LogFilter.Debug);
                cfs.AddFileSystem(path.path.Filename, new RelativeFilesystem(fs, path.path));
            }
            filesystems.Add(cfs);
        }
        filesystems.Reverse();

        var modFileSystem = new UnifiedFileSystem(filesystems);

        modLoader.AddAttachment(new ModFileSystem(modFileSystem));
    }
}