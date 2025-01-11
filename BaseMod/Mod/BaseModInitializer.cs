using CosmosCosmini.Core;
using JustLoaded.Content;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Core.Reflect;

namespace Base.Mod;

[FromMod(BaseMod.ModId)]
public class BaseModInitializer : IModInitializer {
    
    public void SystemInit(JustLoaded.Core.Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(BaseMod.MakeKey("load-player-def"), new PlayerDefLoadingPhase())
            .AssetLoadPhase()
            .Register();
    }
    
}