using System.Reflection;
using CosmosCosmini.Core.Serialization;
using JustLoaded.Core;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace CosmosCosmini.Core.Phases;

public class ReflectedDefLoadingPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        foreach (var (modId, defType) in modLoader.GetAllModTypesByAttribute<DefAttribute>()) {
            var attrib = defType.GetCustomAttribute<DefAttribute>()!;

            var db = modLoader.MasterDb.GetByContentType(defType)!;
            
            modLoader.LoadDefsReflected(modLoader.GetModDirectory(attrib.SearchDir), attrib.Extension, defType, (def, key) => db.AddContent(key, def, defType));
        }
    }
    
}