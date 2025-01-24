using CosmosCosmini.Core.Phases;
using CosmosCosmini.JustLoadedEx;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Loading;
using JustLoaded.Util;

namespace CosmosCosmini.Core;

using static CoreMod.Keys.Phase;

public class CoreModInitializer : IModInitializer {

    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(ConstructFs, new ConstructFilesystemsPhase())
            .Register();
        
        phases.New(RegisterDb, new RegisterDbEntrypointLoadingPhase())
            .WithOrder(ConstructFs, Order.After)
            .Register();
        
        phases.New(RegisterDbReflect, new RegisterDbReflectLoadingPhase())
            .WithOrder(ConstructFs, Order.After)
            .Register();
        
        phases.New(ConstructDeserializer, new ConstructDeserializerPhase())
            .WithOrder(RegisterDb, Order.After)
            .WithOrder(RegisterDbReflect, Order.After)
            .Register();
        
        phases.New(RegisterContentReflect, new RegisterContentLoadingPhase())
            .WithOrder(ConstructDeserializer, Order.After)
            .Register();
        
        phases.New(LoadContentAuto, new ReflectedDefLoadingPhase())
            .WithOrder(RegisterContentReflect, Order.After)
            .Register();

        phases.New(LoadSprites, new LoadSpritesPhase())
            .AssetLoadPhase()
            .Register();
        
        phases.New(LoadAnimations, new LoadAnimationsPhase())
            .AssetLoadPhase()
            .WithOrder(LoadSprites, Order.After)
            .Register();
        
        phases.New(AssetLoadEnd, new DummyLoadingPhase())
            .Register();

        phases.New(RegisterSystems, new RegisterGameSystemsPhase())
            .WithOrder(AssetLoadEnd, Order.After)
            .Register();
    }
}