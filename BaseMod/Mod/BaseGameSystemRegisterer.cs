using Base.Systems;
using CosmosCosmini;
using CosmosCosmini.Core;
using JustLoaded.Core;
using JustLoaded.Core.Reflect;

namespace Base.Mod;

using static BaseMod.Keys.GameSystems;

[FromMod(BaseMod.ModId)]
public class BaseGameSystemRegisterer : IGameSystemRegisterer {
    
    public void RegisterSystems(ModLoaderSystem modLoader, OrderedResolver<IGameSystem> systems) {
        systems.New(ShipController, new ShipControlSystem(modLoader))
            .Register();
    }
    
}