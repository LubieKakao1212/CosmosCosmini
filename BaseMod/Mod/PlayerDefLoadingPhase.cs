using Base.Def;
using CosmosCosmini.Core;
using JustLoaded.Core;
using JustLoaded.Core.Loading;

namespace Base.Mod;

public class PlayerDefLoadingPhase : ILoadingPhase {
    
    public void Load(ModLoaderSystem modLoader) {
        var spritesFs = modLoader.GetModDirectory("player");

        modLoader.LoadFilesMono<PlayerDef, PlayerDef>(spritesFs, "player", (def, _, _) => def);
    }
    
}