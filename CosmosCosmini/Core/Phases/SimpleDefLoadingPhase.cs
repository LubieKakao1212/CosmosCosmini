using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Loading;

namespace CosmosCosmini.Core.Phases;

public class SimpleDefLoadingPhase<TDef>(string extension, string searchDir = "", ContentKey? dbKey = null) : ILoadingPhase where TDef : class {
    
    public void Load(ModLoaderSystem modLoader) {
        var fs = modLoader.GetModDirectory(searchDir);
        var db = modLoader.MasterDb.GetDatabase<TDef>(dbKey);
        modLoader.LoadFilesMono<TDef, TDef>(fs, extension, (def, key, path) => def, db);
    }
    
}