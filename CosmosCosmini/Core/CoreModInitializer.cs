using CosmosCosmini.Core.Phases;
using CosmosCosmini.JustLoadedEx;
using JustLoaded.Core;
using JustLoaded.Core.Entrypoint;
using JustLoaded.Core.Loading;
using JustLoaded.Util;

namespace CosmosCosmini.Core;

using static CoreMod.Keys.Phase;

public class CoreModInitializer : IModInitializer {

    public void SystemInit(Mod thisMod, OrderedResolver<ILoadingPhase> phases) {
        phases.New(ConstructFs, new ConstructFilesystemsPhase())
            .Register();
        
        phases.New(ConstructDeserializer, new ConstructDeserializerPhase())
            .WithOrder(ConstructFs, Order.After)
            .Register();
        
        phases.New(RegisterDb, new DatabaseRegistrationLoadingPhase())
            .WithOrder(ConstructDeserializer, Order.After)
            .Register();
        
        phases.New(RegisterContentAuto, new RegisterContentLoadingPhase())
            .WithOrder(RegisterDb, Order.After)
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